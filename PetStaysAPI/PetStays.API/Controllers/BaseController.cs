using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PetStays.API.Controllers
{
    /// <summary>
    /// Base controller for all API's.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
    }
}
