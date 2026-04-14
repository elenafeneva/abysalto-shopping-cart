using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.API.Features.Auth;

namespace ShoppingCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterUser.Response>> RegisterAsync([FromBody] RegisterUser.Request request)
            => await _mediator.Send(request);

        [HttpPost("login")]
        public async Task<ActionResult<LoginUser.Response>> LoginAsync([FromBody] LoginUser.Request request)
            => await _mediator.Send(request);
    }
}
