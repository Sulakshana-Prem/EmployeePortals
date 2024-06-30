
using EmployeeAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeAPI.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //[Authorize]
    public class TokenController : ControllerBase
    {
        public IConfiguration _config;
        private readonly IEmployeeRepo _Repo;
        public TokenController(IConfiguration config, IEmployeeRepo repo)
        {
            _config = config;
            _Repo = repo;

        }

        [HttpGet]
        public async Task<UserInfo> GetUser(string username, string Password)
        {
            return await _Repo.GetUser(username, Password);
        }


        [Route("/api/GetToken")]
        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] UserInfo model)
        {
            if (model != null && model.UserName != null && model.Password != null)
            {
                var user = await GetUser(model.UserName, model.Password);
                if (user != null)
                {
                var claims = new[] {
                new Claim("Id", user.UserId.ToString()),
                new Claim("UserName", model.UserName)
            };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
                    var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    TimeZoneInfo indianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime utcExpirationTime = DateTime.UtcNow.AddMinutes(60);
                    DateTime istExpirationTime = TimeZoneInfo.ConvertTimeFromUtc(utcExpirationTime, indianTimeZone);


                    var token = new JwtSecurityToken(
                        issuer: _config.GetSection("Jwt:Issuer").Value,
                        audience: _config.GetSection("Jwt:Audience").Value,
                        claims: claims,
                        expires: istExpirationTime,
                        signingCredentials: signingCredentials);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid Credentials");
                }
            }
            else
            {
                return BadRequest("Invalid Input");
            }
        }



    }

}
