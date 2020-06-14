using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialWeb.API.Data;
using SocialWeb.API.Models;
using SocialWeb.API.DTOs;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace SocialWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserToRegisterDTO user)
        {
            user.Username = user.Username.ToLower();
            if (await _repo.UserExists(user.Username))
                return BadRequest("User already exists.");

            await _repo.Register(new User() { Username = user.Username }, user.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserToLogin userToLogin)
        {
            var loggedUser = await _repo.Login(userToLogin.Username.ToLower(), userToLogin.Password);
            if (loggedUser == null)
                return Unauthorized();
            // create JWT
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier,loggedUser.Id.ToString() ),
                new Claim(ClaimTypes.Name, loggedUser.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(15),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}