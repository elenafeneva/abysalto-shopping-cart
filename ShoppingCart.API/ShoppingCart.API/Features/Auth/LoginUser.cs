using FluentValidation;
using MediatR;
using ShoppingCart.API.Entities.DTOs;
using ShoppingCart.API.Services;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.API.Features.Auth
{
    public class LoginUser 
    {
        public class Request : IRequest<Response>
        {
            [Required]
            public string Email { get; set; } = string.Empty;
            [Required]
            public string Password { get; set; } = string.Empty;
        }

        public class Response
        {
            public AuthResultDto AuthResult { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress();
                RuleFor(x => x.Password)
                    .NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IAuthService _authService;

            public Handler(IAuthService authService)
            {
                _authService = authService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var result = await _authService.LoginAsync(request.Email, request.Password);
                return new Response
                {
                    AuthResult = result
                };
            }
        }

    }
}
