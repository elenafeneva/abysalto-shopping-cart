using FluentValidation;
using MediatR;
using ShoppingCart.API.Services;
using ShoppingCart.Domain.Entities.DTOs;

namespace ShoppingCart.API.Features
{
    public class QueryProduct 
    {
        public class Request : IRequest<Response> 
        { 
            public int Id { get; set; }
        }

        public class Response 
        {
            public ProductDto Product { get; set; } = new ProductDto(); 
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .NotEmpty()
                    .GreaterThan(0);
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
                var product = await _productService.GetProductByIdAsync(request.Id);
                return new Response
                {
                    Product = product
                };
            }
        }
    }
}
