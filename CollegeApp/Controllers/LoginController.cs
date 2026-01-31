using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CollegeApp.DTOs;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        [HttpPost]
        public ActionResult Login(LoginDTO model )
        {
            LoginResponseDTO response = new()
            {
                UserName = model.UserName
            };
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }
            if(model.UserName == "admin" && model.Password == "password")
            {
                var JWTSecret = _configuration.GetValue< string>("JWTSecret");
                if(string.IsNullOrEmpty(JWTSecret))
                {
                    return StatusCode(500, "JWT Secret is not configured.");
                }
                var key = Encoding.ASCII.GetBytes(JWTSecret);
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                        //Username 
                        new (ClaimTypes.Name, model.UserName),
                        //Role
                        new (ClaimTypes.Role, "Admin")



                    }),
                    Expires = DateTime.Now.AddHours(4),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token= tokenHandler.CreateToken(tokenDescriptor);
                response.Token= tokenHandler.WriteToken(token);
                return Ok(response);

            }
            else
            {
                return Unauthorized("Invalid username or password.");
            } 


        }

    }
  
   
}
