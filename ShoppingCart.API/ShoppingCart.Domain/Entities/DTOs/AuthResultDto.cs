using ShoppingCart.API.Entities.Enums;

namespace ShoppingCart.API.Entities.DTOs
{
    public class AuthResultDto
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; } = string.Empty;
        public AuthFailureReason FailureReason { get; set; }

        public AuthResultDto(bool isSuccess, string token, AuthFailureReason failureReason)
        {
            IsSuccess = isSuccess;
            Token = token;
            FailureReason = failureReason;
        }
    }
}
