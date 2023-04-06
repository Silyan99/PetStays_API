using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PetStays_API.DBModels;
using PetStays_API.Interfaces;
using PetStays_API.Models;
using PetStays_API.Repositories;
using PetStays_API.Utility;
using System.Net;

namespace PetStays_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PetStaysController : ControllerBase
    {
        private readonly IPetStaysRepository _petStaysRepository;

        public PetStaysController(IPetStaysRepository petStaysRepository)
        {
            _petStaysRepository = petStaysRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Signup")]
        public async Task<IActionResult> Signup(Signup details)
        {
            var result = await _petStaysRepository.SignUp(details);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(AuthVM), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login(Login details)
        {
            var result = await _petStaysRepository.Login(details);
            return Ok(result);
        }
    }
}
