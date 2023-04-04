using Microsoft.AspNetCore.Mvc;
using PetStays_API.DBModels;
using PetStays_API.Interfaces;
using PetStays_API.Models;
using PetStays_API.Repositories;

namespace PetStays_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetStaysController : ControllerBase
    {
        private readonly IPetStaysRepository _petStaysRepository;
        public PetStaysController(IPetStaysRepository petStaysRepository) {
            _petStaysRepository = petStaysRepository;
        }

        [HttpPost]
        [Route("Signup")]
        public async Task<IActionResult> Signup(Signup details)
        {
            var result = await _petStaysRepository.SignUp(details);
            return Ok(result);
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(Login details)
        {
            return Ok();
        }
    }
}
