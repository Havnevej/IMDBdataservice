using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IMDBdataservice;
using IMDBdataservice.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebServiceToken.Attributes;
using WebServiceToken.Models;
using WebServiceToken.Services;

namespace WebServiceToken.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IbaseService _dataService;
        private readonly IConfiguration _configuration;

        public UsersController(IbaseService dataService, IConfiguration configuration)
        {
            _dataService = dataService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody]User dto)
        {
            /*if (_dataService.GetUser(dto.Username) != null)
            {
                return BadRequest();
            }*/
            if (_dataService.GetImdbContext().Users.ToList().Any(x => x.Username == dto.Username))
            {
                return BadRequest();
            }

            int.TryParse(_configuration.GetSection("Auth:PasswordSize").Value, out int pwdSize);

            if (pwdSize == 0)
            {
                return BadRequest("No password size");
            }

            var salt = PasswordService.GenerateSalt(pwdSize);
            var pwd = PasswordService.HashPassword(dto.Password, salt, pwdSize);

            _dataService.CreateUser(dto.Username, pwd, salt);

            return CreatedAtRoute(null, dto);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _dataService.GetUser(dto.Username);
            if (user == null)
            {
                return BadRequest();
            }

            int.TryParse(_configuration.GetSection("Auth:PasswordSize").Value, out int pwdSize);

            if (pwdSize == 0)
            {
                throw new ArgumentException("No password size");
            }

            string secret = _configuration.GetSection("Auth:Secret").Value;
            if (string.IsNullOrEmpty(secret))
            {
                throw new ArgumentException("No secret");
            }

            var password = PasswordService.HashPassword(dto.Password, user.Salt, pwdSize);

            if (password != user.Password)
            {
                return BadRequest();
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(secret);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new [] { new Claim("for_user", user.Username.ToString()) }),
                Expires = DateTime.Now.AddMinutes(60),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescription);
            var token = tokenHandler.WriteToken(securityToken);
            
            return Ok(new {dto.Username, token});
        }
        [Authorization]
        [HttpPost("delete")]
        public IActionResult Delete([FromBody]LoginDto dto)
        {
            
            if (!_dataService.GetImdbContext().Users.ToList().Any(x => x.Username == dto.Username))
            {
                return BadRequest();
            }

            _dataService.DeleteUser(dto.Username);

            return Ok(new {message="Deleted User!"});
        }

        
        [HttpGet("get/{id}")]
        public IActionResult Get(string id)
        {

            if (!_dataService.GetImdbContext().Users.ToList().Any(x => x.Username == id))
            {
                return BadRequest();
            }

            var user = _dataService.GetUser(id);

            return Ok(user);
        }


    }
}
