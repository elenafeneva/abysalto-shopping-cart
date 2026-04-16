using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.API.Entities.Enums;
using ShoppingCart.API.Features;

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
        {
            var response = await _mediator.Send(request);
            if (response?.AuthResult?.FailureReason == AuthFailureReason.None)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginUser.Response>> LoginAsync([FromBody] LoginUser.Request request)
        {
            var response = await _mediator.Send(request);
            if (response?.AuthResult?.FailureReason == AuthFailureReason.None)
                return Ok(response);
            return BadRequest(response);
        }

        [Authorize]
        [HttpGet("currentUser")]
        public async Task<QueryUser.Response> GetCurrentUser()
        {
            if(User?.Identity?.IsAuthenticated != true && string.IsNullOrWhiteSpace(User?.Identity?.Name))
                return new QueryUser.Response();
            return await _mediator.Send(new QueryUser.Request { Id = Guid.Parse(User.Identity.Name) });
        }
    }
}
