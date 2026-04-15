using FluentValidation;
using MediatR;
using ShoppingCart.API.Services;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.API.Features
{
    public class DeleteCartItem
    {
        public class Request : IRequest<Response>
        {
            public Guid CartItemId { get; set; }
        }

        public class Response
        {
            public bool CartItemDeleted { get; set; } = false;
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.CartItemId)
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
                var cartItemDeleted = await _cartService.DeleteCartItemAsync(request.CartItemId);
                response.CartItemDeleted = cartItemDeleted;
                return response;
            }
        }
    }
}
