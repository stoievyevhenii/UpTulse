using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using UpTulse.Core.Exceptions;
using UpTulse.DataAccess.EnvironmentVariables;
using UpTulse.DataAccess.Identity;

namespace UpTulse.Application.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateToken(ApplicationUser user, IConfiguration configuration)
        {
            var secretKey = Environment.GetEnvironmentVariable(SecurityEnv.JWT_SECRET)
                ?? throw new ConfigurationNotFoundException("SecretKey not found");

            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName ?? throw new DbRecordNotFoundException("UserName not found")),
                    new Claim(ClaimTypes.Role, user.Role)
                ]),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}