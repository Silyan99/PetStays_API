using Microsoft.IdentityModel.Tokens;
using PetStays.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetStays.App.Helpers
{
    public static class HelperFunction
    {
        public static string GenerateToken(User user, string tokenKey, int tokenExpiryTime)
        {
            JwtSecurityTokenHandler val = new JwtSecurityTokenHandler();
            byte[] bytes = Encoding.ASCII.GetBytes(tokenKey);
            SecurityTokenDescriptor val2 = new SecurityTokenDescriptor();
            val2.Subject = new ClaimsIdentity(new Claim[2]
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("userName", user.UserName),
            });
            val2.Expires = (DateTime?)DateTime.UtcNow.AddMinutes(tokenExpiryTime);
            val2.SigningCredentials = new SigningCredentials((SecurityKey)new SymmetricSecurityKey(bytes), "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256");
            SecurityTokenDescriptor val3 = val2;
            SecurityToken val4 = ((SecurityTokenHandler)val).CreateToken(val3);
            return ((SecurityTokenHandler)val).WriteToken(val4);
        }
    }
}
