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


        [HttpGet]
        public async Task<QueryCartItems.Response> GetCartItems()
        {
            var request = new QueryCartItems.Request { UserId = Guid.Parse(User?.Identity?.Name)};
            return await _mediator.Send(request);
        }

        [HttpPost]
        public async Task<CreateCartItem.Response> CreateCartItemAsync([FromBody] CreateCartItem.Request request)
        {
            request.UserId = Guid.Parse(User?.Identity?.Name);
            return await _mediator.Send(request);
        }

        [HttpDelete("{productId}")]
        public async Task<DeleteCartItem.Response> DeleteCartItemAsync([FromRoute] int productId)
        {
            var request = new DeleteCartItem.Request { ProductId = productId, UserId = Guid.Parse(User?.Identity?.Name) };
            return await _mediator.Send(request);
        }
    }
}
