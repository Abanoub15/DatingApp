using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
     
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> Register([FromBody] UserForRegister userForRegister)
        {
            //validate requst
            userForRegister.UserName = userForRegister.UserName.ToLower();
            if (await _repo.UserExists(userForRegister.UserName))
                return BadRequest("the user name already exist");

            var userToCrate = new User { UserName = userForRegister.UserName };
            var createdUser = await _repo.Register(userToCrate, userForRegister.Password);
            return StatusCode(201);
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(UserForLogin userForLogin)
        {
            var userFromRepo = await _repo.Login(userForLogin.UserName.ToLower(), userForLogin.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var clamis = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name,userFromRepo.UserName)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                        .GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds= new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                    Subject=new ClaimsIdentity(clamis),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials=creds
            };
            var tokenHandeler=new JwtSecurityTokenHandler();
            var token = tokenHandeler.CreateToken(tokenDescriptor);
            return Ok(new{
                token=tokenHandeler.WriteToken(token)
            });
        }
    }
}