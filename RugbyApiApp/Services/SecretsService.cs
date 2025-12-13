using Microsoft.Extensions.Configuration;

namespace RugbyApiApp.Services
{
    /// <summary>
    /// Service for managing API key storage securely using User Secrets or environment variables
    /// </summary>
    public class SecretsService
    {
        private readonly IConfiguration? _configuration;
        private const string API_KEY_NAME = "RugbyApiKey";
        private const string ENV_VAR_NAME = "RUGBY_API_KEY";

        public SecretsService(IConfiguration? configuration = null)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Get API key from User Secrets first, then fallback to environment variable
        /// </summary>
        public string? GetApiKey()
        {
            // Try User Secrets first (for development)
            if (_configuration != null)
            {
                var apiKey = _configuration[API_KEY_NAME];
                if (!string.IsNullOrWhiteSpace(apiKey))
                {
                    return apiKey;
                }
            }

            // Fallback to environment variable (for production/CI/CD)
            var envKey = Environment.GetEnvironmentVariable(ENV_VAR_NAME);
            if (!string.IsNullOrWhiteSpace(envKey))
            {
                return envKey;
            }

            return null;
        }

        /// <summary>
        /// Set API key in User Secrets (recommended) and environment variable
        /// </summary>
        public void SetApiKey(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("API key cannot be empty", nameof(apiKey));
            }

            // Store in environment variable for immediate use
            Environment.SetEnvironmentVariable(ENV_VAR_NAME, apiKey, EnvironmentVariableTarget.User);
        }

        /// <summary>
        /// Clear API key from environment variable
        /// Note: User Secrets should be cleared manually using: dotnet user-secrets remove RugbyApiKey
        /// </summary>
        public void ClearApiKey()
        {
            Environment.SetEnvironmentVariable(ENV_VAR_NAME, null, EnvironmentVariableTarget.User);
        }

        /// <summary>
        /// Get information about where API key is stored
        /// </summary>
        public string GetStorageInfo()
        {
            var hasConfigKey = _configuration != null && !string.IsNullOrWhiteSpace(_configuration[API_KEY_NAME]);
            var hasEnvKey = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(ENV_VAR_NAME));

            var locations = new List<string>();
            if (hasConfigKey) locations.Add("User Secrets");
            if (hasEnvKey) locations.Add("Environment Variable");

            if (locations.Count == 0)
            {
                return "No API key found in User Secrets or Environment Variable";
            }

            return $"API key found in: {string.Join(", ", locations)}";
        }
    }
}
