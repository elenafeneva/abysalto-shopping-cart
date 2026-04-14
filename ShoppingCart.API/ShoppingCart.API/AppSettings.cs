using MediatR.NotificationPublishers;

namespace ShoppingCart.API
{
    public class AppSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DummyProductsUrl { get; set; } = string.Empty;
        public JwtSettings Jwt { get; set; } = new JwtSettings();

        public class JwtSettings
        {
            public string Secret { get; set; } = string.Empty;
            public string Issuer { get; set; } = string.Empty;
            public string Audience { get; set; } = string.Empty;
            public int ExpiresMinutes { get; set; }
        }
    }
}
