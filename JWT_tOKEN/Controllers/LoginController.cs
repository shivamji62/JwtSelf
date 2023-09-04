using JWT_tOKEN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JWT_tOKEN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public IConfiguration _config { get; set; }
        public LoginController(IConfiguration configuration)
        {
            _config = configuration;    
        }

        private Users AuthenticateUser(Users users)
        {
            Users? _user = null;
            if(users.UserName == "ShivamD" && users.Password == "12345")
            {
                _user = new Users { UserName = "Shivam Dongre" };
            }
            return _user;
        }
        private string GenerateToken(Users users)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credientials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], null,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credientials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);    
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Users users)
        {
            IActionResult response = Unauthorized();
            var user_ = AuthenticateUser(users);
            if(user_ != null) 
            {
                var token = GenerateToken(user_);
                response = Ok(new {token=token});
            }
            return response;
        }
    }
}
