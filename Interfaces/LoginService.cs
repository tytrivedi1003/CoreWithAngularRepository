using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Data;
using Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Implementations
{
    public class LoginService : ILoginService
    {
        IConfiguration _config; 
        public LoginService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<string> CreateToken(UserDetails userDetails)
        {
            var claims = new[]
           {
                new Claim(ClaimTypes.Email, userDetails.EmailId)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("Jwt:Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var generatedToken = tokenHandler.WriteToken(token);
            return generatedToken;
        }
    }
}
