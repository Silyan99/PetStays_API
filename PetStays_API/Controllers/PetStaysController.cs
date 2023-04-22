using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PetStays_API.Interfaces;
using PetStays_API.Models;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PetStays_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PetStaysController : ControllerBase
    {
        private readonly IPetStaysRepository _petStaysRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PetStaysController(IPetStaysRepository petStaysRepository, IMailService mailService, IWebHostEnvironment webHostEnvironment)
        {
            _petStaysRepository = petStaysRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SignUpUser(Signup details)
        {
            var result = await _petStaysRepository.SignUpUser(details);
            return Ok(result);
        }

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("[action]")]
        //[ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> SignUpAdmin(Signup details)
        //{
        //    var result = await _petStaysRepository.SignUpAdmin(details);
        //    return Ok(result);
        //}

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(AuthVM), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login(Login details)
        {
            var result = await _petStaysRepository.Login(details);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Contact")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Contact(ContactInfo details)
        {
            var result = await _petStaysRepository.Contact(details);
            return Ok(result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddRequest([FromForm] PetDetail details)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var availabilities = await _petStaysRepository.GetAdminAvailability();

            var dateFrom = Convert.ToDateTime(details.DateFrom);
            var dateTo = Convert.ToDateTime(details.DateTo);
            var TimeStart = TimeSpan.Parse(details.TimeFrom);
            var TimeTo = TimeSpan.Parse(details.TimeTo);
            var DefaultStart = new TimeSpan(10, 0, 0);
            var DefaultEnd = new TimeSpan(15, 0, 0);

            bool isValidDropTiming = true;
            bool isValidPickupTiming = true;
            if (availabilities.Count > 0)
            {
                var dropAvailability = availabilities.Where(x => DateTime.Parse(x.Date) == dateFrom).FirstOrDefault();
                var pickAvailability = availabilities.Where(x => DateTime.Parse(x.Date) == dateTo).FirstOrDefault();

                if (dropAvailability != null)
                {
                    isValidDropTiming = (!dropAvailability.FullDay) && TimeSpan.Parse(dropAvailability.TimeStart) <= TimeStart
                        && TimeSpan.Parse(dropAvailability.TimeEnd) >= TimeStart;
                }
                else
                {
                    isValidDropTiming = TimeStart <= DefaultEnd && TimeStart >= DefaultStart;
                }

                if (pickAvailability != null)
                {
                    isValidPickupTiming = (!pickAvailability.FullDay) && TimeSpan.Parse(pickAvailability.TimeStart) <= TimeTo
                       && TimeSpan.Parse(pickAvailability.TimeEnd) >= TimeTo;
                }
                else
                {
                    isValidDropTiming = TimeTo <= DefaultEnd && TimeTo >= DefaultStart;
                }
            }

            if (!isValidDropTiming)
            {
                return Ok(new Result { Status = false, Message = "Date start is invalid." });
            }

            if (!isValidPickupTiming)
            {
                return Ok(new Result { Status = false, Message = "Date end is invalid." });
            }
            

            // getting file original name
            string FileName = details.PhotoFile.FileName;

            // combining GUID to create unique name before saving in wwwroot
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + FileName;
            var directory = Directory.GetCurrentDirectory() + "/Images/";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // getting full path inside wwwroot/images
            var imagePath = Path.Combine(directory, uniqueFileName);
            var filePath = Path.Combine("/Images/", uniqueFileName);

            // copying file
            if (!System.IO.File.Exists(imagePath))
            {
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    details.PhotoFile.CopyTo(stream);
                }
            }
            details.Photo = filePath;
            var result = await _petStaysRepository.AddRequest(details, identity);
            return Ok(result);
        }

        //[HttpPost]
        //[Route("[action]")]
        //[ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> Request(RequestDetail details)
        //{
        //    var result = await _petStaysRepository.AddRequest(details);
        //    return Ok(result);
        //}

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Availability(AvailabilityDetail details)
        {
            var result = await _petStaysRepository.AddAvailability(details);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<RequestsVM>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllRequest()
        {
            var result = await _petStaysRepository.GetAllRequest();
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{Id}")]
        [ProducesResponseType(typeof(RequestsVM), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRequest(int Id)
        {
            var result = await _petStaysRepository.GetRequest(Id);
            return Ok(result);
        }

        [HttpPut]
        [Route("[action]/{Id}")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Request(int Id, UpdateRequest data)
        {
            data.Id = Id;
            var result = await _petStaysRepository.UpdateRequest(data);
            return Ok(result);
        }

        [HttpPost]
        [Route("[action]/{Id}")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdatePetRequest(int Id, [FromForm]PetDetail details)
        {
            details.Id = Id;
            //// getting file original name
            //string FileName = data.PhotoFile.FileName;

            //// combining GUID to create unique name before saving in wwwroot
            //string uniqueFileName = Guid.NewGuid().ToString() + "_" + FileName;
            //var directory = Directory.GetCurrentDirectory() + "/Images/";

            //if (!Directory.Exists(directory))
            //{
            //    Directory.CreateDirectory(directory);
            //}

            //// getting full path inside wwwroot/images
            //var imagePath = Path.Combine(directory, uniqueFileName);
            //var filePath = Path.Combine("/Images/", uniqueFileName);

            //// copying file
            //if (!System.IO.File.Exists(imagePath))
            //{
            //    using (var stream = new FileStream(imagePath, FileMode.Create))
            //    {
            //        data.PhotoFile.CopyTo(stream);
            //    }
            //}
            //data.Photo = filePath;
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var availabilities = await _petStaysRepository.GetAdminAvailability();

            var dateFrom = Convert.ToDateTime(details.DateFrom);
            var dateTo = Convert.ToDateTime(details.DateTo);
            var TimeStart = TimeSpan.Parse(details.TimeFrom);
            var TimeTo = TimeSpan.Parse(details.TimeTo);
            var DefaultStart = new TimeSpan(10, 0, 0);
            var DefaultEnd = new TimeSpan(15, 0, 0);

            bool isValidDropTiming = true;
            bool isValidPickupTiming = true;
            if (availabilities.Count > 0)
            {
                var dropAvailability = availabilities.Where(x => DateTime.Parse(x.Date) == dateFrom).FirstOrDefault();
                var pickAvailability = availabilities.Where(x => DateTime.Parse(x.Date) == dateTo).FirstOrDefault();

                if (dropAvailability != null)
                {
                    isValidDropTiming = (!dropAvailability.FullDay) && TimeSpan.Parse(dropAvailability.TimeStart) <= TimeStart
                        && TimeSpan.Parse(dropAvailability.TimeEnd) >= TimeStart;
                }
                else
                {
                    isValidDropTiming = TimeStart <= DefaultEnd && TimeStart >= DefaultStart;
                }

                if (pickAvailability != null)
                {
                    isValidPickupTiming = (!pickAvailability.FullDay) && TimeSpan.Parse(pickAvailability.TimeStart) <= TimeTo
                       && TimeSpan.Parse(pickAvailability.TimeEnd) >= TimeTo;
                }
                else
                {
                    isValidDropTiming = TimeTo <= DefaultEnd && TimeTo >= DefaultStart;
                }
            }

            if (!isValidDropTiming)
            {
                return Ok(new Result { Status = false, Message = "Date start is invalid." });
            }

            if (!isValidPickupTiming)
            {
                return Ok(new Result { Status = false, Message = "Date end is invalid." });
            }
            var result = await _petStaysRepository.UpdatePetRequest(details);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<RequestsVM>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllUserRequest()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var result = await _petStaysRepository.GetAllUserRequest(identity);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{Id}")]
        [ProducesResponseType(typeof(RequestsVM), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserRequest(int Id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var result = await _petStaysRepository.GetUserRequest(Id, identity);
            return Ok(result);
        }

        [HttpDelete]
        [Route("[action]/{Id}")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteRequest(int Id)
        {
            var result = await _petStaysRepository.DeleteRequest(Id);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{Id}")]
        [ProducesResponseType(typeof(IList<AvailabilityDetail>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAvailability(int Id)
        {
            var result = await _petStaysRepository.GetAvailability(Id);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{Id}")]
        [ProducesResponseType(typeof(IList<AvailabilityDetail>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAvailabilityForCustomer(int Id)
        {
            var result = await _petStaysRepository.GetAvailabilityForCustomer(Id);
            return Ok(result);
        }

        [HttpDelete]
        [Route("[action]/{Id}")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAvailability(int Id)
        {
            var result = await _petStaysRepository.DeleteAvailability(Id);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{Id}")]
        [ProducesResponseType(typeof(UserDetail), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserById(int Id)
        {
            var result = await _petStaysRepository.GetUserById(Id);
            return Ok(result);
        }

        [HttpPut]
        [Route("[action]/{Id}")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAvailability(int Id, AvailabilityDetail data)
        {
            data.Id = Id;
            var result = await _petStaysRepository.UpdateAvailability(data);
            return Ok(result);
        }
    }
}
