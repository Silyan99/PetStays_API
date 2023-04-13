using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PetStays_API.DBModels;
using PetStays_API.Exceptions;
using PetStays_API.Helpers;
using PetStays_API.Interfaces;
using PetStays_API.Models;
using PetStays_API.Utility;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            if (dbUser != null) throw new ConflictException(ErrorMessages.EmailAlreadyExist);
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
            var token = UserManager.GenerateJWTToken(details.Email, user.Id, _jwtToken, user.Role);
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

        //public async Task<Result> AddPet(PetDetail data)
        //{
        //    Result result = new Result();
        //    var pet = new Pet()
        //    {
        //        Category = data.Category,
        //        Uid = data.Uid,
        //        Name = data.Name,
        //        Age = data.Age,
        //        Gender = data.Gender,
        //        Vaccinated = data.Vaccinated,
        //        Color = data.Color,
        //        Breed = data.Breed,
        //        Details = data.Details,
        //        OwnerId = data.OwnerId,
        //        Photo = data.Photo
        //    };
        //    _ctx.Pets.Add(pet);

        //    _ctx.SaveChanges();
        //    result.Status = true;
        //    result.Message = "Pet detail added successfully";
        //    return result;
        //}

        public async Task<Result> AddRequest(PetDetail data, ClaimsIdentity identity)
        {
            Result result = new Result();
            var id = Convert.ToInt32(identity.FindFirst("Id").Value);
            var pet = new Pet()
            {
                Category = data.Category,
                Uid = data.Uid,
                Name = data.Name,
                Age = data.Age,
                Gender = data.Gender,
                Vaccinated = data.Vaccinated,
                Color = data.Color,
                Breed = data.Breed,
                Details = data.Details,
                OwnerId = id,
                Photo = data.Photo
            };
            _ctx.Pets.Add(pet);
            _ctx.SaveChanges();

            var req = new Request()
            {
                MadeBy = id,
                Date = data.Date,
                TimeFrom = data.TimeFrom,
                TimeTo = data.TimeTo,
                PetId = pet.Id
            };
            _ctx.Requests.Add(req);

            _ctx.SaveChanges();
            result.Status = true;
            result.Message = "Request detail added successfully";
            return result;
        }

        //public async Task<Result> AddRequest(RequestDetail data)
        //{
        //    Result result = new Result();
        //    var req = new Request()
        //    {
        //        MadeBy = data.MadeBy,
        //        Date = data.Date,
        //        TimeFrom = data.TimeFrom,
        //        TimeTo = data.TimeTo,
        //        PetId = data.PetId,
        //        IsPaymentDone = data.IsPaymentDone,
        //        Status = data.Status,
        //        Remarks = data.Remarks
        //    };
        //    _ctx.Requests.Add(req);

        //    _ctx.SaveChanges();
        //    result.Status = true;
        //    result.Message = "Request detail added successfully";
        //    return result;
        //}

        public async Task<Result> AddAvailability(AvailabilityDetail data)
        {
            Result result = new Result();
            var availability = new Availability()
            {
                Date = data.Date,
                TimeStart = data.TimeStart,
                TimeEnd = data.TimeEnd,
                FullDay = data.FullDay,
                AdminId = data.AdminId
            };
            _ctx.Availabilities.Add(availability);

            _ctx.SaveChanges();
            result.Status = true;
            result.Message = "Availability detail added successfully";
            return result;
        }

        public async Task<List<RequestsVM>> GetAllRequest()
        {
            var item = (
                from p in _ctx.Pets
                join r in _ctx.Requests on p.Id equals r.PetId
                select new RequestsVM
                {
                    Category = p.Category,
                    Uid = p.Uid,
                    Name = p.Name,
                    Age = Convert.ToInt32(p.Age),
                    Gender = p.Gender,
                    Vaccinated = Convert.ToBoolean(p.Vaccinated),
                    Color = p.Color,
                    Breed = p.Breed,
                    Details = p.Details,
                    OwnerId = Convert.ToInt32(p.OwnerId),
                    Photo = p.Photo,
                    Date = Convert.ToDateTime(r.Date),
                    TimeFrom = (TimeSpan)r.TimeFrom,
                    TimeTo = (TimeSpan)r.TimeTo,
                    PetId = Convert.ToInt32(r.PetId),
                    IsPaymentDone = Convert.ToBoolean(r.IsPaymentDone),
                    Status = Convert.ToBoolean(r.Status),
                    Remarks = Convert.ToString(r.Remarks)
                });

            return item.ToList();
        }

        public async Task<RequestsVM> GetRequest(int id)
        {
            var item = (
                from p in _ctx.Pets
                join r in _ctx.Requests on p.Id equals r.PetId
                where (p.Id == id)
                select new RequestsVM
                {
                    Category = p.Category,
                    Uid = p.Uid,
                    Name = p.Name,
                    Age = Convert.ToInt32(p.Age),
                    Gender = p.Gender,
                    Vaccinated = Convert.ToBoolean(p.Vaccinated),
                    Color = p.Color,
                    Breed = p.Breed,
                    Details = p.Details,
                    OwnerId = Convert.ToInt32(p.OwnerId),
                    Photo = p.Photo,
                    Date = Convert.ToDateTime(r.Date),
                    TimeFrom = (TimeSpan)r.TimeFrom,
                    TimeTo = (TimeSpan)r.TimeTo,
                    PetId = Convert.ToInt32(r.PetId),
                    IsPaymentDone = Convert.ToBoolean(r.IsPaymentDone),
                    Status = Convert.ToBoolean(r.Status),
                    Remarks = Convert.ToString(r.Remarks)
                }).FirstOrDefault();

            return item;
        }

        public async Task<Result> UpdateRequest(UpdateRequest data)
        {
            Result result = new Result();
            Request res = await _ctx.Requests.Where(x => x.PetId == data.Id).FirstOrDefaultAsync();
            if(res == null) throw new NotFoundException(ErrorMessages.DataNotFound);
            res.Status = data.Status;
            res.Remarks = data.Remarks;
            res.IsPaymentDone = data.IsPaymentDone;
            _ctx.Requests.Update(res);
            _ctx.SaveChanges();
            result.Status = true;
            result.Message = "Detail updated successfully";
            return result;
        }

        public async Task<List<RequestsVM>> GetAllUserRequest(ClaimsIdentity identity)
        {
            var id = Convert.ToInt32(identity.FindFirst("Id").Value);
            var item = (
                from p in _ctx.Pets
                join r in _ctx.Requests on p.Id equals r.PetId
                where (p.OwnerId == id)
                select new RequestsVM
                {
                    Category = p.Category,
                    Uid = p.Uid,
                    Name = p.Name,
                    Age = Convert.ToInt32(p.Age),
                    Gender = p.Gender,
                    Vaccinated = Convert.ToBoolean(p.Vaccinated),
                    Color = p.Color,
                    Breed = p.Breed,
                    Details = p.Details,
                    OwnerId = Convert.ToInt32(p.OwnerId),
                    Photo = p.Photo,
                    Date = Convert.ToDateTime(r.Date),
                    TimeFrom = (TimeSpan)r.TimeFrom,
                    TimeTo = (TimeSpan)r.TimeTo,
                    PetId = Convert.ToInt32(r.PetId),
                    IsPaymentDone = Convert.ToBoolean(r.IsPaymentDone),
                    Status = Convert.ToBoolean(r.Status),
                    Remarks = Convert.ToString(r.Remarks)
                });

            return item.ToList();
        }

        public async Task<RequestsVM> GetUserRequest(int id, ClaimsIdentity identity)
        {
            var userId = Convert.ToInt32(identity.FindFirst("Id").Value);
            var item = (
                from p in _ctx.Pets
                join r in _ctx.Requests on p.Id equals r.PetId
                where (p.Id == id && p.OwnerId == userId)
                select new RequestsVM
                {
                    Category = p.Category,
                    Uid = p.Uid,
                    Name = p.Name,
                    Age = Convert.ToInt32(p.Age),
                    Gender = p.Gender,
                    Vaccinated = Convert.ToBoolean(p.Vaccinated),
                    Color = p.Color,
                    Breed = p.Breed,
                    Details = p.Details,
                    OwnerId = Convert.ToInt32(p.OwnerId),
                    Photo = p.Photo,
                    Date = Convert.ToDateTime(r.Date),
                    TimeFrom = (TimeSpan)r.TimeFrom,
                    TimeTo = (TimeSpan)r.TimeTo,
                    PetId = Convert.ToInt32(r.PetId),
                    IsPaymentDone = Convert.ToBoolean(r.IsPaymentDone),
                    Status = Convert.ToBoolean(r.Status),
                    Remarks = Convert.ToString(r.Remarks)
                }).FirstOrDefault();

            return item;
        }

        public async Task<Result> DeleteRequest(int id)
        {
            Result result = new Result();
            Pet res = await _ctx.Pets.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (res == null) throw new NotFoundException(ErrorMessages.IdNotFound);
            _ctx.Pets.Remove(res);
            _ctx.SaveChanges();
            result.Status = true;
            result.Message = "Request deleted successfully";
            return result;
        }
    }
}
