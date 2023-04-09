using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PetStays_API.DBModels;
using PetStays_API.Exceptions;
using PetStays_API.Helpers;
using PetStays_API.Interfaces;
using PetStays_API.Models;
using PetStays_API.Utility;

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
                return result;
            }

        }

        public async Task<AuthVM> Login(Login details)
        {
            using (PetStaysContext con = new PetStaysContext())
            {
                Base64Helper base64 = new Base64Helper();
                User user = await con.Users.Where(x => x.Email == details.Email).FirstOrDefaultAsync();
                if (user == null) throw new NotFoundException(ErrorMessages.UserNotExist);
                if (user.Password != details.Password)
                    throw new BadReqException(ErrorMessages.EmailPasswordNotCorrect);
                var token = UserManager.GenerateJWTToken(details.Email, _jwtToken);
                return new AuthVM { Token = token };
            }
        }

        public async Task<Result> Contact(ContactInfo data)
        {
            Result result = new Result();
            using (PetStaysContext con = new PetStaysContext())
            {
                var contact = new Contact()
                {
                    Email = data.Email,
                    FullName = data.FullName,
                    Mobile = data.Mobile,
                    Message= data.Message
                };
                con.Contacts.Add(contact);

                con.SaveChanges();
                result.Status = true;
                result.Message = "Contact saved successfully";
                return result;
            }
        }
    }
}
