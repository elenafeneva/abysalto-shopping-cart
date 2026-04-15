using FluentValidation;
using MediatR;
using ShoppingCart.API.Services;
using ShoppingCart.Domain.Entities.DTOs;

namespace ShoppingCart.API.Features
{
    public class QueryProducts
    {
        public class Request : IRequest<Response>
        {
            public int Limit { get; set; } = 0;
            public int Skip { get; set; } = 0;
            public Guid UserId { get; set; }

        }

        public class Response
        {
            public IEnumerable<ProductDto> Items { get; set; } = Array.Empty<ProductDto>();
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.UserId)
                    .NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IProductService _productService;

            public Handler(IProductService productService)
            {
                _productService = productService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var products = await _productService.GetProductsAsync(request.Limit, request.Skip, request.UserId);
                return new Response
                {
                    Items = products
                };
            }
        }
    }
}
