using DTOs;
using LoginContract;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Service
{
    public class LoginService : ILoginServices
    {
        IConfiguration _config;
        public LoginService(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateJSONWebToken(UserDetails userModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public UserModel Authenticate(UserDetails userModel)
        {
            UserModel user = null;
            if (userModel.FirstName == "Tejas")
            {
                user = new UserModel { FirstName = "Tejas", LastName = "Trivedi", EmailId = "test.btest@gmail.com" };
            }
            return user;
        }
    }
}
