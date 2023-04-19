using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetStays_API.Interfaces;
using PetStays_API.Models;
using System.Net;
using System.Security.Claims;

namespace PetStays_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PetStaysController : ControllerBase
    {
        private readonly IPetStaysRepository _petStaysRepository;

        public PetStaysController(IPetStaysRepository petStaysRepository, IMailService mailService)
        {
            _petStaysRepository = petStaysRepository;
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
        public async Task<IActionResult> AddRequest(PetDetail details)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
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
        [ProducesResponseType(typeof(UserDetail), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserById(int Id)
        {
            var result = await _petStaysRepository.GetUserById(Id);
            return Ok(result);
        }
    }
}
