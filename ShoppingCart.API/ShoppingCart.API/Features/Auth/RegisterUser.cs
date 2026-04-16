using FluentValidation;
using MediatR;
using ShoppingCart.API.Entities.DTOs;
using ShoppingCart.API.Services;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.API.Features
{
    public class RegisterUser
    {
        public class Request : IRequest<Response>
        {
            [Required]
            public string FirstName { get; set; } = string.Empty;
            [Required]
            public string LastName { get; set; } = string.Empty;
            [Required]
            public string Email { get; set; } = string.Empty;
            [Required]
            public string Password { get; set; } = string.Empty;
            [Required]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public class Response
        {
            public AuthResultDto AuthResult { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.FirstName)
                    .NotEmpty();
                RuleFor(x => x.LastName)
                    .NotEmpty();
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress();
                RuleFor(x => x.Password)
                    .NotEmpty()
                    .Must(p => p.Any(char.IsLetter))
                    .WithMessage("Password must contain at least one letter.")
                    .Must(p => p.Any(char.IsDigit))
                    .WithMessage("Password must contain at least one number.")
                    .Must(p => p.Any(c => !char.IsLetterOrDigit(c)))
                    .WithMessage("Password must contain at least one special character.")
                    .MinimumLength(6);
                RuleFor(x => x.ConfirmPassword)
                    .NotEmpty()
                    .Equal(x => x.Password)
                    .WithMessage("Passwords do not match.");
            }
        }


        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IAuthService _authService;

            public Handler(IAuthService authService )
            {
                _authService = authService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            { 
                var authResult = await _authService.RegisterAsync(request.FirstName, request.LastName, request.Email, request.Password);
                return new Response{
                    AuthResult = authResult
                };
            }
        }
    }
}
           
