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
        private readonly Config _jwtToken;
        public readonly PetStaysContext _ctx;
        public PetStaysRepository(IOptions<Config> config)
        {
            _jwtToken = config.Value;
            _ctx = new PetStaysContext(config.Value.ConnectionStrings);
        }

        public async Task<Result> SignUpUser(Signup details)
        {
            Result result = new Result();
            Base64Helper base64 = new Base64Helper();
            User dbUser = await _ctx.Users.Where(x => x.Email == details.Email).FirstOrDefaultAsync();
            if(dbUser != null) throw new ConflictException(ErrorMessages.EmailAlreadyExist);
            var user = new User()
            {
                Email = details.Email,
                Password = base64.Base64Encode(details.Password),
                FullName = details.FullName,
                Mobile = details.Mobile,
                Role = "User"
            };
            _ctx.Users.Add(user);

            _ctx.SaveChanges();
            result.Status = true;
            result.Message = "User added successfully";
            return result;
        }

        public async Task<Result> SignUpAdmin(Signup details)
        {
            Result result = new Result();
            Base64Helper base64 = new Base64Helper();
            User dbUser = await _ctx.Users.Where(x => x.Email == details.Email).FirstOrDefaultAsync();
            if (dbUser != null) throw new ConflictException(ErrorMessages.EmailAlreadyExist);
            var user = new User()
            {
                Email = details.Email,
                Password = base64.Base64Encode(details.Password),
                FullName = details.FullName,
                Mobile = details.Mobile,
                Role = "Admin"
            };
            _ctx.Users.Add(user);

            _ctx.SaveChanges();
            result.Status = true;
            result.Message = "Admin added successfully";
            return result;
        }

        public async Task<AuthVM> Login(Login details)
        {

            Base64Helper base64 = new Base64Helper();
            User user = await _ctx.Users.Where(x => x.Email == details.Email).FirstOrDefaultAsync();
            if (user == null) throw new NotFoundException(ErrorMessages.UserNotExist);
            if (user.Password != details.Password)
                throw new BadReqException(ErrorMessages.EmailPasswordNotCorrect);
            var token = UserManager.GenerateJWTToken(details.Email, _jwtToken, user.Role);
            return new AuthVM { Token = token };
        }

        public async Task<Result> Contact(ContactInfo data)
        {
            Result result = new Result();
            var contact = new Contact()
            {
                Email = data.Email,
                FullName = data.FullName,
                Mobile = data.Mobile,
                Message = data.Message
            };
            _ctx.Contacts.Add(contact);

            _ctx.SaveChanges();
            result.Status = true;
            result.Message = "Contact saved successfully";
            return result;
        }
    }
}
