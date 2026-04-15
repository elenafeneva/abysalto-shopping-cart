using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.API.Features;

namespace ShoppingCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<CreateCartItem.Response> CreateCartItemAsync([FromBody] CreateCartItem.Request request)
        {
            request.UserId = Guid.Parse(User?.Identity?.Name);
            return await _mediator.Send(request);
        }

        [HttpDelete("{cartItemId}")]
        public async Task<DeleteCartItem.Response> DeleteCartItemAsync(Guid cartItemId)
        {
            var request = new DeleteCartItem.Request { CartItemId = cartItemId };
            return await _mediator.Send(request);
        }

        //Query cart items for a user -- TO IMPLEMENT
    }
}
