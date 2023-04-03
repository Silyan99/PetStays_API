
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetStays.App.Models;
using PetStays.App.UserManagement.Command.Authenticate;
using System.Net;

namespace PetStays.API.Controllers
{
    /// <summary>
    /// Users Controller
    /// </summary>
    public class UsersController : BaseController
    {
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(AuthResult), (int)HttpStatusCode.OK)]
        [HttpPost]
        public async Task<ActionResult> Authenticate([FromBody] AuthenticateCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}