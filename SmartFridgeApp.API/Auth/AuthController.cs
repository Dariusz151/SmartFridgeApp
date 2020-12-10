
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SmartFridgeApp.API.Auth
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        public IConfiguration _configuration;

        public AuthController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetToken(string username, string password)
        {
            if (username is null || password is null)
            {
                return BadRequest("Username or password is null.");
            }

            if (username.Equals(_configuration["AuthUsername"]) && password.Equals(_configuration["AuthPassword"])){
                return Ok(GenerateJSONWebToken());
            }
            return BadRequest("Invalid user or password");
        }

        private string GenerateJSONWebToken()
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(4),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

