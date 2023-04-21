using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetStays_API.DBModels;
using PetStays_API.Exceptions;
using PetStays_API.Helpers;
using PetStays_API.Interfaces;
using PetStays_API.Models;
using PetStays_API.Utility;
using System.Linq;
using System.Security.Claims;

namespace PetStays_API.Repositories
{
    public class PetStaysRepository : IPetStaysRepository
    {
        private readonly Config _jwtToken;
        public readonly PetStaysContext _ctx;
        private readonly IMailService _mailService;

        public PetStaysRepository(IOptions<Config> config, IMailService mailService)
        {
            _jwtToken = config.Value;
            _ctx = new PetStaysContext(config.Value.ConnectionStrings);
            _mailService = mailService;
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
                Role = "User",
                Address = details.Address
            };
            _ctx.Users.Add(user);
            _ctx.SaveChanges();
            result.Status = true;
            result.Message = "User added successfully";
            return result;
        }

        //public async Task<Result> SignUpAdmin(Signup details)
        //{
        //    Result result = new Result();
        //    Base64Helper base64 = new Base64Helper();
        //    User dbUser = await _ctx.Users.Where(x => x.Email == details.Email).FirstOrDefaultAsync();
        //    if (dbUser != null) throw new ConflictException(ErrorMessages.EmailAlreadyExist);
        //    var user = new User()
        //    {
        //        Email = details.Email,
        //        Password = base64.Base64Encode(details.Password),
        //        FullName = details.FullName,
        //        Mobile = details.Mobile,
        //        Role = "Admin"
        //    };

        //    _ctx.Users.Add(user);
        //    _ctx.SaveChanges();
            
        //    result.Status = true;
        //    result.Message = "Admin added successfully";
        //    return result;
        //}

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
            User user = await _ctx.Users.Where(x => x.Role == "Admin").FirstOrDefaultAsync();
            if (user == null) throw new BadReqException(ErrorMessages.AdminNotFound);
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
                PetId = pet.Id,
                Status = "pending",
                DateFrom = Convert.ToDateTime(data.DateFrom),
                DateTo = Convert.ToDateTime(data.DateTo),
                TimeFrom = TimeSpan.Parse(data.TimeFrom),
                TimeTo = TimeSpan.Parse(data.TimeTo)
            };
            _ctx.Requests.Add(req);

            _ctx.SaveChanges();



#if !DEBUG

            MailRequest request = new MailRequest()
            {
                MailTo = user.Email, // Admin Mail
                Subject = "New Request",
                Body = $"Hi! You have recieved new request with details " +
                $"\n Email: {username}" +
                $"\n Pet name: {data.Name}"
            };
            await _mailService.SendEmailAsync(request);
#endif

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
            var dateAlreadyExists = _ctx.Availabilities.Any(x => x.Date == DateTime.Parse(data.Date));
            if (dateAlreadyExists)
            {
                result.Status = false;
                result.Message = $"Schedule for {data.Date} already exists.";
                return result;
            }
            
            if(!IsEmptyTime(data.TimeStart) && !IsEmptyTime(data.TimeEnd))
            {
                data.FullDay = false;
            }
            var availability = new Availability()
            {
                Date = Convert.ToDateTime(data.Date),
                TimeStart = TimeSpan.Parse(data.TimeStart),
                TimeEnd = TimeSpan.Parse(data.TimeEnd),
                FullDay = data.FullDay,
                AdminId = data.AdminId
            };
            _ctx.Availabilities.Add(availability);

            _ctx.SaveChanges();
            result.Status = true;
            result.Message = "Availability detail added successfully";
            return result;
        }

        private bool IsEmptyTime(string timespanString)
        {
            return timespanString.IsNullOrEmpty() || timespanString.Equals("00:00:00");
        }

        public async Task<List<RequestsVM>> GetAllRequest()
        {
            var item = (
                from p in _ctx.Pets
                join r in _ctx.Requests on p.Id equals r.PetId 
                join u in _ctx.Users on p.OwnerId equals u.Id
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
                    DateFrom = r.DateFrom.ToString(),
                    DateTo = r.DateTo.ToString(),
                    TimeFrom = r.TimeFrom.ToString(),
                    TimeTo = r.TimeTo.ToString(),
                    PetId = Convert.ToInt32(r.PetId),
                    IsPaymentDone = Convert.ToBoolean(r.IsPaymentDone),
                    Status = Convert.ToString(r.Status),
                    Remarks = Convert.ToString(r.Remarks),
                    OwnerName = u.FullName,
                    Address = u.Address

                });

            return await item.ToListAsync();
        }

        public async Task<RequestsVM> GetRequest(int id)
        {
            var item = (
                from p in _ctx.Pets
                join r in _ctx.Requests on p.Id equals r.PetId
                join u in _ctx.Users on p.OwnerId equals u.Id
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
                    DateFrom = r.DateFrom.ToString(),
                    DateTo = r.DateTo.ToString(),
                    TimeFrom = r.TimeFrom.ToString(),
                    TimeTo = r.TimeTo.ToString(),
                    PetId = Convert.ToInt32(r.PetId),
                    IsPaymentDone = Convert.ToBoolean(r.IsPaymentDone),
                    Status = Convert.ToString(r.Status),
                    Remarks = Convert.ToString(r.Remarks),
                    Address = u.Address,
                    OwnerName = $"Name: {u.FullName},\n Phone: {u.Mobile}"
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
            User user = await _ctx.Users.Where(x => x.Id == res.MadeBy).FirstOrDefaultAsync();
            if (user == null) throw new BadReqException(ErrorMessages.UserNotExist);
            Pet pet = await _ctx.Pets.Where(x => x.Id == res.PetId).FirstOrDefaultAsync();
            if (pet == null) throw new BadReqException(ErrorMessages.PetNotFound);
            var text = data.Status;
#if !DEBUG
            MailRequest request = new MailRequest()
            {
                MailTo = user.Email, // User Mail
                Subject = $"Request Status for pet {pet.Name} ",
                Body = $"Hi! Your request for pet {pet.Name} has been {text} with remarks {data.Remarks}."
            };
            await _mailService.SendEmailAsync(request);
#endif

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
                    DateFrom = r.DateFrom.ToString(),
                    DateTo = r.DateTo.ToString(),
                    TimeFrom = r.TimeFrom.ToString(),
                    TimeTo = r.TimeTo.ToString(),
                    PetId = Convert.ToInt32(r.PetId),
                    IsPaymentDone = Convert.ToBoolean(r.IsPaymentDone),
                    Status = Convert.ToString(r.Status),
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
                    DateFrom = r.DateFrom.ToString(),
                    DateTo = r.DateTo.ToString(),
                    TimeFrom = r.TimeFrom.ToString(),
                    TimeTo = r.TimeTo.ToString(),
                    PetId = Convert.ToInt32(r.PetId),
                    IsPaymentDone = Convert.ToBoolean(r.IsPaymentDone),
                    Status = Convert.ToString(r.Status),
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

        public async Task<List<AvailabilityDetail>> GetAvailabilityForCustomer(int id)
        {
            DateTime todayDate = DateTime.Now.Date;
            DateTime twoWeeks = DateTime.Now.AddDays(14);
            var item = (
                from a in _ctx.Availabilities
                where (a.AdminId == id && a.Date >= todayDate && a.Date <= twoWeeks)
                select new AvailabilityDetail
                {
                    Date = a.Date.ToString(),
                    TimeStart = a.TimeStart.ToString(),
                    TimeEnd = a.TimeEnd.ToString(),
                    FullDay = Convert.ToBoolean(a.FullDay),
                    AdminId = Convert.ToInt32(a.AdminId),
                    Id = a.Id
                });
            var dbList = item.ToList();
            var availableTimes = new List<AvailabilityDetail>();
            for (int i = 0; i < 14; i++)
            {
                var day = DateTime.Now.AddDays(i);
                var exisitngDay = dbList.FirstOrDefault(x => Convert.ToDateTime(x?.Date) == day.Date);
                if (exisitngDay != null)
                {
                    availableTimes.Add(exisitngDay);
                }
                else
                {
                    availableTimes.Add(new AvailabilityDetail() { 
                        Date = day.Date.ToString("yyyy-MM-dd"),
                        AdminId=id,
                        TimeStart = new TimeSpan(10,0,0).ToString(),
                        TimeEnd = new TimeSpan(15, 0, 0).ToString(),
                        FullDay = false
                    });
                }
            }

            return availableTimes;
        }

        public async Task<List<AvailabilityDetail>> GetAvailability(int id)
        {
            var item = (
                from a in _ctx.Availabilities
                where (a.AdminId == id)
                select new AvailabilityDetail
                {
                    Date = a.Date.ToString(),
                    TimeStart = a.TimeStart.ToString(),
                    TimeEnd = a.TimeEnd.ToString(),
                    FullDay = Convert.ToBoolean(a.FullDay),
                    AdminId = Convert.ToInt32(a.AdminId)
                });

            return item.ToList().Where(x => Convert.ToDateTime(x.Date) > DateTime.Now.Date && Convert.ToDateTime(x.Date) < DateTime.Now.AddDays(14)).ToList();
        }

        public async Task<List<AvailabilityDetail>> GetAdminAvailability()
        {
            var item = (
                from a in _ctx.Availabilities
                select new AvailabilityDetail
                {
                    Date = a.Date.ToString(),
                    TimeStart = a.TimeStart.ToString(),
                    TimeEnd = a.TimeEnd.ToString(),
                    FullDay = Convert.ToBoolean(a.FullDay)
                });

            return item.ToList().Where(x => Convert.ToDateTime(x.Date) > DateTime.Now.Date && Convert.ToDateTime(x.Date) < DateTime.Now.AddDays(14)).ToList();
        }

        public async Task<UserDetail> GetUserById(int id)
        {
            User user = await _ctx.Users.Where(x => x.Id ==id).FirstOrDefaultAsync();
            UserDetail userDetail = new()
            {
                Email = user.Email,
                FullName = user.FullName,
                Mobile = user.Mobile,
                Address = user.Address
            };
            return userDetail;
        }

        public async Task<Result> DeleteAvailability(int id)
        {
            Availability availability = await _ctx.Availabilities.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(availability == null) throw new NotFoundException(ErrorMessages.IdNotFound);
            _ctx.Availabilities.Remove(availability);
            _ctx.SaveChanges();
            Result result = new Result();
            result.Status = true;
            result.Message = "Availability deleted successfully";
            return result;
        }
    }
}
