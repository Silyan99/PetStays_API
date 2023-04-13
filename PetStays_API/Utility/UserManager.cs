using Microsoft.IdentityModel.Tokens;
using PetStays_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetStays_API.Utility
{
    public static class UserManager
    {
        const string _username = "Username";

        public static string GenerateJWTToken(string username, int id, Config jwtConfig, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(_username, username),
                new Claim("Id", id.ToString()),
                role == "Admin" ? new Claim("AdminKey", "7NKBuMfwTM") : new Claim("CustomerKey", "wfZmflTyvR"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var token = new JwtSecurityToken(
            issuer: jwtConfig.Issuer,
            audience: jwtConfig.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
