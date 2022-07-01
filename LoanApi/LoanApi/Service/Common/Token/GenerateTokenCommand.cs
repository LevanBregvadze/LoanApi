using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoanApi.Domain;
using LoanApi.Helper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LoanApi.Service.Common.Token
{
    public class GenerateTokenCommand : IGenerateTokenCommand
    {
        private readonly AppSettings _appSettings;
        

        public GenerateTokenCommand(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

      

        public string GenerateToken(SystemUser systemUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, systemUser.ID.ToString()),
                    new Claim(ClaimTypes.Role, systemUser.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
