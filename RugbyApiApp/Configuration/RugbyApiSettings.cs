namespace RugbyApiApp.Configuration
{
    public class RugbyApiSettings
    {
        public const string ApiBaseUrl = "https://v1.rugby-api.com";
        public const string CdnHostName = "bl-media-api-sports.b-cdn.net";
        public const string ApiKeyEnvironmentVariable = "RUGBY_API_KEY";

        /// <summary>
        /// Get API key from environment variables
        /// </summary>
        public static string? GetApiKey()
        {
            return Environment.GetEnvironmentVariable(ApiKeyEnvironmentVariable);
        }

        /// <summary>
        /// Get database connection string
        /// </summary>
        public static string GetConnectionString()
        {
            return "Data Source=rugby.db";
        }

        /// <summary>
        /// Get CDN URL for a resource path
        /// </summary>
        public static string GetCdnUrl(string? resourcePath)
        {
            if (string.IsNullOrEmpty(resourcePath))
                return string.Empty;

            return $"https://{CdnHostName}{resourcePath}";
        }
    }
}
