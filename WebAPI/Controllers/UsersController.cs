using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
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
        private readonly BaseService _dataService;
        private readonly IConfiguration _configuration;

        public UsersController( IConfiguration configuration)
        {
            _dataService = new BaseService();
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User dto)
        {
            //https://www.rhyous.com/2010/06/15/csharp-email-regular-expression/
            String theEmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z"; //email regex
            if (_dataService.GetImdbContext().Users.ToList().Any(x => x.Username == dto.Username))
            {
                return BadRequest(new { ERROR = "user already exists", ERROR_TYPE = "BAD_DATA" });
            }
            int.TryParse(_configuration.GetSection("Auth:PasswordSize").Value, out int pwdSize); //get the max password size to be set in the database
            if (dto.Password.Length < 8 || dto.Password.Length > 100) // Password length check
            {
                return BadRequest(new { ERROR = "Password size invalid", ERROR_TYPE = "BAD_FORMAT" });
            }
            if (dto.Username.Length < 5 || dto.Username.Length >= pwdSize) // Username length check also matches the max size of the username in the database.
            {
                return BadRequest(new { ERROR = "Invalid username size", ERROR_TYPE = "BAD_FORMAT" });
            }
            if (!Regex.IsMatch(dto.Username, theEmailPattern)) //Email Pattern check
            {
                return BadRequest(new { ERROR = "Invalid email", ERROR_TYPE = "BAD_FORMAT" });
            }
            var salt = PasswordService.GenerateSalt(pwdSize);
            var pwd = PasswordService.HashPassword(dto.Password, salt, pwdSize);

            _dataService.CreateUser(dto.Username, pwd, salt);

            return CreatedAtAction(nameof(Get), new { id = dto.Username }, new { created_user = dto.Username });
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _dataService.GetUser(dto.Username);
            if (user == null)
            {
                return BadRequest(new { ERROR = "no user", ERROR_TYPE = "BAD_DATA" });
            }

            int.TryParse(_configuration.GetSection("Auth:PasswordSize").Value, out int pwdSize);

            if (pwdSize == 0)
            {
                throw new ArgumentException("No password size set in config");
            }

            string secret = _configuration.GetSection("Auth:Secret").Value;
            if (string.IsNullOrEmpty(secret))
            {
                throw new ArgumentException("No secret set in config");
            }

            var password = PasswordService.HashPassword(dto.Password, user.Salt, pwdSize);

            if (password != user.Password)
            {
                return BadRequest( new {ERROR="username/password incorrect", ERROR_TYPE="BAD_DATA"});
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
                return StatusCode(405, new { ERROR = "not allowed", DESC = "cant get other users than yourself", ERROR_TYPE = "BAD_DATA" });
            }

            _dataService.DeleteUser(dto.Username);

            return Delete(dto);
        }

        [Authorization]
        [HttpGet("get/{id}")]
        public IActionResult Get(string id)
        {
            User logged_in_user = (User)HttpContext.Items["User"];
            var user_to_get = _dataService.GetImdbContext().Users.Find(id);
            if (user_to_get == null || logged_in_user.Password != user_to_get.Password)
            {
                return BadRequest(new { ERROR = "not allowed", ERROR_TYPE = "BAD_DATA" }); // Cant get users that are not yourself (logged in) or user doesnt exist.
            }
            var user = _dataService.GetUser(id);
            user.BookmarkTitles = _dataService.GetBookmarksForUser(user.Username);
            Console.WriteLine(user.BookmarkTitles.Count);
            foreach (var item in user.BookmarkTitles)
            {
                item.Title = new();
                item.Title = _dataService.ctx.Titles.FirstOrDefault(title => title.TitleId == item.TitleId);
                //item.Title = new _dataService.ctx.Titles.FirstOrDefault(title => title.TitleId == item.TitleId);
            }
            user.SearchHistories = _dataService.GetSearchHistory(user.Username, new QueryStringOur { });
            user.Comments = _dataService.GetCommentsByUser(user.Username, new QueryStringOur { });

            return Ok(user);
        }


    }
}
