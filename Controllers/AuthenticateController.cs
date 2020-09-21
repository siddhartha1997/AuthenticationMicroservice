using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationMicroservice.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AuthenticateController));
        private IConfiguration _config;
        public AuthenticateController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public IActionResult Login([FromBody] Authenticate login)
        {

            _log4net.Info("Authentication initiated");
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }
        private string GenerateJSONWebToken(Authenticate userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                null,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private Authenticate AuthenticateUser(Authenticate login)
        {
            Authenticate user = null;

            /*if (login.Name == "himanshu" && login.Password == "himanshu")
            {
                user = new Authenticate { Name = "himanshu", Password = "himanshu" };
            }
            else if (login.Name == "namit" && login.Password == "namit")
            {
                user = new Authenticate { Name = "namit", Password = "namit" };
            }
            else if (login.Name == "anjali" && login.Password == "anjali")
            {
                user = new Authenticate { Name = "anjali", Password = "anjali" };
            }
            else if (login.Name == "nitesh" && login.Password == "nitesh")
            {
                user = new Authenticate { Name = "nitesh", Password = "nitesh" };
            }*/
            Dictionary<string, string> ValidUsersDictionary = new Dictionary<string, string>()
           {
               {"Siddhartha","Banerjee"},
               {"Piyush","Gupta"},
               {"Sahil","Kumar"},
               {"Arnabha","Sen"},
               {"Devashish","Jitendra" }
           };

            if (ValidUsersDictionary.Any(u => u.Key == login.UserName && u.Value == login.Password))
            {
                user = new Authenticate { UserName = login.UserName, Password = login.Password };
            }

            return user;
        }
    }
}
