using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace StudentAPI.Services
{
    public class JwtService
    {
        private readonly string _secretKey;
        private readonly int _durationInMinutes;


        public JwtService(string secretKey, int durationInMinutes = 60)
        {
            _secretKey = secretKey;
            _durationInMinutes = durationInMinutes;
        }

        public string GenerateToken(string email, string role)
        {
            var claims = new[]
            {
                 new Claim(ClaimTypes.Email, email),
                 new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "StudentAPI",
                audience: "StudentAPP",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_durationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
