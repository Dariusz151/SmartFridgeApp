
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartFridgeApp.Core.Contracts.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartFridgeApp.API.Auth
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAppUserService _appUserService;
        private readonly IRefreshTokenService _refreshTokenService;

        private const string RefreshTokenCookie = "refreshToken";

        public AuthController(IConfiguration config, IAppUserService appUserService, IRefreshTokenService refreshTokenService)
        {
            _configuration = config;
            _appUserService = appUserService;
            _refreshTokenService = refreshTokenService;
        }

        // ──────────────────────────────────────────────
        //  Email + Password
        // ──────────────────────────────────────────────

        /// <summary>
        /// Register a new account with email and password.
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Email, name and password are required.");
            }

            if (request.Password.Length < 6)
            {
                return BadRequest("Password must be at least 6 characters.");
            }

            var created = await _appUserService.RegisterAsync(request.Email, request.Name, request.Password);

            if (!created)
            {
                return Conflict("An account with this email already exists.");
            }

            return Created(string.Empty, null);
        }

        /// <summary>
        /// Login with email and password. Returns a JWT token.
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var (success, role, name) = await _appUserService.ValidateCredentialsAsync(request.Email, request.Password);

            if (!success)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = GenerateJSONWebToken(request.Email, name, role);
            var refreshToken = await _refreshTokenService.CreateTokenAsync(request.Email);
            SetRefreshTokenCookie(refreshToken);

            return Ok(new AuthResponse
            {
                Token = token,
                Email = request.Email,
                Name = name,
                Role = role
            });
        }

        // ──────────────────────────────────────────────
        //  Google OAuth
        // ──────────────────────────────────────────────

        /// <summary>
        /// Initiates Google OAuth login. Redirects the user to Google's consent screen.
        /// </summary>
        [HttpGet("google-login")]
        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleCallback)),
                Items = { { "scheme", GoogleDefaults.AuthenticationScheme } }
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Handles the callback from Google after user authentication.
        /// </summary>
        [HttpGet("google-response")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                return Unauthorized("Google authentication failed.");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Could not retrieve email from Google.");
            }

            // Ensure user exists in AppUsers (auto-register on first Google login)
            await _appUserService.EnsureGoogleUserAsync(email, name);

            var role = await _appUserService.GetRoleAsync(email);
            var token = GenerateJSONWebToken(email, name, role);
            var refreshToken = await _refreshTokenService.CreateTokenAsync(email);
            SetRefreshTokenCookie(refreshToken);

            var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000";
            var redirectUrl = $"{frontendUrl}/auth/google-callback" +
                              $"?token={Uri.EscapeDataString(token)}" +
                              $"&email={Uri.EscapeDataString(email)}" +
                              $"&name={Uri.EscapeDataString(name ?? string.Empty)}" +
                              $"&role={Uri.EscapeDataString(role)}";

            return Redirect(redirectUrl);
        }

        // ──────────────────────────────────────────────
        //  Logout
        // ──────────────────────────────────────────────

        /// <summary>
        /// Logs the user out and revokes all refresh tokens.
        /// </summary>
        [HttpPost("logout")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            // Revoke refresh tokens if the user has a valid JWT
            var email = User?.FindFirst(ClaimTypes.Email)?.Value;
            if (!string.IsNullOrEmpty(email))
            {
                await _refreshTokenService.RevokeAllAsync(email);
            }

            // Clear the cookie
            Response.Cookies.Delete(RefreshTokenCookie, new CookieOptions
            {
                Path = "/api/auth",
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            await HttpContext.SignOutAsync(GoogleDefaults.AuthenticationScheme);
            return Ok("Logged out successfully.");
        }

        // ──────────────────────────────────────────────
        //  Token refresh
        // ──────────────────────────────────────────────

        /// <summary>
        /// Uses the httpOnly refresh-token cookie to issue a new JWT + rotated refresh token.
        /// </summary>
        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh()
        {
            if (!Request.Cookies.TryGetValue(RefreshTokenCookie, out var rawToken) || string.IsNullOrEmpty(rawToken))
            {
                return Unauthorized("No refresh token.");
            }

            var result = await _refreshTokenService.RotateTokenAsync(rawToken);
            if (result is null)
            {
                Response.Cookies.Delete(RefreshTokenCookie, new CookieOptions
                {
                    Path = "/api/auth",
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });
                return Unauthorized("Invalid or expired refresh token.");
            }

            var (email, newRefreshToken) = result.Value;
            var role = await _appUserService.GetRoleAsync(email);
            var name = User?.FindFirst(ClaimTypes.Name)?.Value;
            var accessToken = GenerateJSONWebToken(email, name, role);
            SetRefreshTokenCookie(newRefreshToken);

            return Ok(new AuthResponse
            {
                Token = accessToken,
                Email = email,
                Name = name,
                Role = role
            });
        }

        // ──────────────────────────────────────────────
        //  JWT generation
        // ──────────────────────────────────────────────

        private string GenerateJSONWebToken(string email, string name, string role)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("role", role)
            };

            if (!string.IsNullOrEmpty(name))
            {
                claims.Add(new Claim(ClaimTypes.Name, name));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void SetRefreshTokenCookie(string token)
        {
            Response.Cookies.Append(RefreshTokenCookie, token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api/auth",
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
        }
    }
}


