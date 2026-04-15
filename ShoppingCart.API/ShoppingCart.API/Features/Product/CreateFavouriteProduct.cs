using FluentValidation;
using MediatR;
using ShoppingCart.API.Services;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.API.Features
{
    public class CreateFavouriteProduct
    {
        public class Request : IRequest<Response>
        {
            public int ProductId { get; set; }
            public Guid UserId { get; set; }
        }

        public class Response
        {
            public bool FavoriteProductCreated { get; set; } = false;
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.ProductId)
                    .NotEmpty()
                    .GreaterThan(0);
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
                FavoriteProduct favoriteProduct = new FavoriteProduct(request.ProductId, request.UserId);
                var response = new Response();
                response.FavoriteProductCreated = await _productService.CreateFavoriteProduct(favoriteProduct);
                
                return response;
            }
        }
    }
}
