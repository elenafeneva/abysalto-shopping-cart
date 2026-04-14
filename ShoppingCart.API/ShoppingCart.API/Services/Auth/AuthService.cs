using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ShoppingCart.API.Entities.DTOs;
using ShoppingCart.API.Entities.Enums;
using ShoppingCart.Domain.Entities;
using System.Security.Claims;
using System.Text;

namespace ShoppingCart.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;
        public AuthService(IUserService userService, AppSettings appSettings)
        {
            _userService = userService;
            _appSettings = appSettings;
        }

        public async Task<AuthResultDto> LoginAsync(string email, string password)
        {
            var user = await _userService.GetByEmailAsync(email);

            if (user is null)
            {
                return new AuthResultDto(false, string.Empty, AuthFailureReason.InvalidCredentials);
            }

            var passwordVerificationResult = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Failed;
            if (passwordVerificationResult)
            {
                return new AuthResultDto(false, string.Empty, AuthFailureReason.InvalidCredentials);
            }

            string token = CreateToken(user);
            return new AuthResultDto(true, token, AuthFailureReason.None);
        }

        private string CreateToken(User user)
        {
            string secretKey = _appSettings.Jwt.Secret;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                ]),
                Expires = DateTime.UtcNow.AddMinutes(_appSettings.Jwt.ExpiresMinutes),
                SigningCredentials = credentials,
                Issuer = _appSettings.Jwt.Issuer,
                Audience = _appSettings.Jwt.Audience
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptor);
            return token;
        }

        public async Task<AuthResultDto> RegisterAsync(string firstName, string lastName, string email, string password)
        {
            var existingUser = await _userService.GetByEmailAsync(email);
            if (existingUser is not null)
                return new AuthResultDto(false, string.Empty, AuthFailureReason.EmailAlreadyExists);

            var user = new User(firstName, lastName, email);
            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, password);
            user.PasswordHash = hashedPassword;

            await _userService.CreateAsync(user);
            return new AuthResultDto(true, string.Empty, AuthFailureReason.None);
        }
    }
}
