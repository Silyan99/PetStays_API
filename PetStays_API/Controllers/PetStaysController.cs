using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PetStays_API.DBModels;
using PetStays_API.Interfaces;
using PetStays_API.Models;
using PetStays_API.Repositories;
using PetStays_API.Utility;

namespace PetStays_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PetStaysController : ControllerBase
    {
        private readonly IPetStaysRepository _petStaysRepository;
        private readonly JwtConfig _jwtToken;
        public PetStaysController(IPetStaysRepository petStaysRepository, IOptions<JwtConfig> tokenConfig) {
            _petStaysRepository = petStaysRepository;
            _jwtToken = tokenConfig.Value;
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
        [AllowAnonymous]
        public IActionResult Login(Login details)
        {
            var token = UserManager.GenerateJWTToken(details.Email, _jwtToken);
            return Ok(token);
        }
    }
}
