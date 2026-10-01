namespace schoolmanagement.Api.Auth
{
    public class AuthSettings
    {
        public bool EnableApiKey { get; set; }
        public bool EnableJwt { get; set; }
        public JwtSettings Jwt { get; set; } = new JwtSettings();

        public string SecretKey => Jwt?.Key ?? string.Empty;
        public string Issuer => Jwt?.Issuer ?? string.Empty;
        public string Audience => Jwt?.Audience ?? string.Empty;
        public int TokenExpiryMinutes => Jwt?.TokenExpirationMinutes ?? 60;

        public string HeaderKeyName { get; set; } = "X-Api-Key";
        public string HeaderKeyValue { get; set; } = string.Empty;
    }

    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int TokenExpirationMinutes { get; set; } = 60;
        public string Key { get; set; } = string.Empty;
    }
}
