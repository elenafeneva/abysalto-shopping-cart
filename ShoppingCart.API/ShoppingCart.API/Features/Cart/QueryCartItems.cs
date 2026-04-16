using FluentValidation;
using MediatR;
using ShoppingCart.API.Services;
using ShoppingCart.Domain.Entities.DTOs;

namespace ShoppingCart.API.Features
{
    public class QueryCartItems
    {
        public class Request : IRequest<Response>
        {
            public Guid UserId { get; set; }
        }

        public class Response
        {
            public List<ProductDto> Items { get; set; } = new List<ProductDto>();
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
            private readonly ICartService _cartService;

            public Handler(ICartService cartService)
            {
                _cartService = cartService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var products = await _cartService.GetCartProductsByUserId(request.UserId);
                return new Response()
                {
                    Items = products
                };
            }
        }

    }
}
