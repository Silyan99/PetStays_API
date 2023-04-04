using Microsoft.AspNetCore.Mvc;
using PetStays_API.DBModels;
using PetStays_API.Models;

namespace PetStays_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetStaysController : ControllerBase
    {
        public PetStaysController() { }

        [HttpPost]
        [Route("Signup")]
        public IActionResult Signup(Signup details)
        {
            using (PetStaysContext con = new PetStaysContext())
            {
                var user = new User()
                {
                    Email = details.Email,
                    Password = details.Password,
                    FullName= details.FullName,
                    Mobile= details.Mobile                
                };
                con.Users.Add(user);

                con.SaveChanges();
            }

            return Ok(new Result { Status = true, Message = "User Added" });
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(Login details)
        {
            return Ok();
        }
    }
}
