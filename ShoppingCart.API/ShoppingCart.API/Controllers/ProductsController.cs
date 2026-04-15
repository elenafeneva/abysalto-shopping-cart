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
        public async Task<QueryProducts.Response> GetProductsAsync(int limit, int skip)
        {
            var request = new QueryProducts.Request
            {
                Limit = limit,
                Skip = skip
            };
            return await _mediator.Send(request);
        }

        [HttpGet("{id}")]
        public async Task<QueryProduct.Response> GetProductByIdAsync([FromRoute] int id)
            => await _mediator.Send(new QueryProduct.Request { Id = id });
    }
}
