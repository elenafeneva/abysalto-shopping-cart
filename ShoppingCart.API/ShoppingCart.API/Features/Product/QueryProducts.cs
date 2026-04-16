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
            public string SortField { get; set; } = string.Empty;
            public string SortOrder { get; set; } = string.Empty;
            public Guid UserId { get; set; }

        }

        public class Response
        {
            public IEnumerable<ProductDto> Items { get; set; } = Array.Empty<ProductDto>();
            public int Total { get; set; } = 0;
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
                var responseProducts = await _productService.GetProductsAsync(request.Limit, request.Skip, request.SortField, request.SortOrder, request.UserId);
                return new Response
                {
                    Items = responseProducts.Products,
                    Total = responseProducts.TotalNumberOfProducts
                };
            }
        }
    }
}
