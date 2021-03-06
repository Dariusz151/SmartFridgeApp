﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
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

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetToken([FromBody]AuthRequest authRequest)
        {
            if (authRequest.Login is null || authRequest.Password is null)
            {
                return BadRequest("Username or password is null.");
            }

            if (authRequest.Login.Equals(_configuration["AuthUsername"]) && authRequest.Password.Equals(_configuration["AuthPassword"])){
                return Ok(Json(GenerateJSONWebToken()));
            }
            return Unauthorized();
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

