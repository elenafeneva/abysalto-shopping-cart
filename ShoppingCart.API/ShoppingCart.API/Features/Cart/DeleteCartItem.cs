using FluentValidation;
using MediatR;
using ShoppingCart.API.Services;

namespace ShoppingCart.API.Features
{
    public class DeleteCartItem
    {
        public class Request : IRequest<Response>
        {
            public int ProductId { get; set; }
            public Guid UserId { get; set; }
        }

        public class Response
        {
            public bool CartItemDeleted { get; set; } = false;
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
                var cartItemDeleted = await _cartService.DeleteCartItemAsync(request.ProductId, request.UserId);
                response.CartItemDeleted = cartItemDeleted;
                return response;
            }
        }
    }
}
