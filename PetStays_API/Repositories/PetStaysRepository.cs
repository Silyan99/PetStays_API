using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PetStays_API.DBModels;
using PetStays_API.Helpers;
using PetStays_API.Interfaces;
using PetStays_API.Models;
using PetStays_API.Utility;
using System.Buffers.Text;
using System.Net;
using System.Web.Http;

namespace PetStays_API.Repositories
{
    public class PetStaysRepository : IPetStaysRepository
    {
        private readonly JwtConfig _jwtToken;
        public PetStaysRepository(IOptions<JwtConfig> tokenConfig) 
        {
            _jwtToken = tokenConfig.Value;
        }

        public async Task<Result> SignUp(Signup details)
        {
            Result result = new Result();
            try
            {
                using (PetStaysContext con = new PetStaysContext())
                {
                    Base64Helper base64 = new Base64Helper();
                    var user = new User()
                    {
                        Email = details.Email,
                        Password = base64.Base64Encode(details.Password),
                        FullName = details.FullName,
                        Mobile = details.Mobile
                    };
                    con.Users.Add(user);

                    con.SaveChanges();
                    result.Status = true;
                    result.Message = "User added successfully";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }

            return result;

        }

        public async Task<AuthVM> Login(Login details)
        {
            Result result = new Result();
            try
            {
                using (PetStaysContext con = new PetStaysContext())
                {
                    Base64Helper base64 = new Base64Helper();
                    User user = await con.Users.Where(x => x.Email == details.Email).FirstOrDefaultAsync();
                    if (user == null) throw new Exception("User not exist");
                    if(user.Password == details.Password)
                    {
                        var token = UserManager.GenerateJWTToken(details.Email, _jwtToken);
                        return new AuthVM { Token = token };
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }
    }
}
