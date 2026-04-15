using FluentValidation;
using MediatR;
using ShoppingCart.API.Services;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.API.Features
{
    public class CreateCartItem
    {
        public class Request : IRequest<Response>
        {
            public int ProductId { get; set; }
            public Guid UserId { get; set; }
            public int Quantity { get; set; } = 1;
        }

        public class Response
        {
            public bool CartItemCreated { get; set; } = false;
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
            private readonly ICartService _cartService;

            public Handler(ICartService cartService)
            {
                _cartService = cartService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var response = new Response();
                CartItem cartItem = new CartItem(request.UserId, request.ProductId, request.Quantity);
                var cartItemCreated = await _cartService.CreateCartItemAsync(cartItem);
                response.CartItemCreated = cartItemCreated;
                return response;
            }
        }
    }
}
