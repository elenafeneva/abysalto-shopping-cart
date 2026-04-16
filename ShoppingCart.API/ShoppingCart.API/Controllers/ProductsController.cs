using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.API.Features;

namespace ShoppingCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<QueryProducts.Response> GetProductsAsync([FromQuery] int limit, int skip, string sortField, string sortOrder)
        {
            var request = new QueryProducts.Request
            {
                Limit = limit,
                Skip = skip,
                SortField = sortField,
                SortOrder = sortOrder,
                UserId = Guid.Parse(User?.Identity?.Name)
            };
            var response = await _mediator.Send(request);
            return response;
        }

        [HttpGet("{id}")]
        public async Task<QueryProduct.Response> GetProductByIdAsync([FromRoute] int id)
            => await _mediator.Send(new QueryProduct.Request { Id = id, UserId = Guid.Parse(User?.Identity?.Name)});

        [HttpPost("{productId}/favorites")]
        public async Task<CreateFavouriteProduct.Response> CreateFavouriteProduct([FromRoute] int productId)
        {
            var userId = Guid.Parse(User?.Identity?.Name);
            var request = new CreateFavouriteProduct.Request
            {
                ProductId = productId,
                UserId = userId
            };
            return await _mediator.Send(request);
        }
    }
}
