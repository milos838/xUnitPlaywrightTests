using Microsoft.Extensions.Configuration;

namespace PlaywrightTests.Utilities
{
    /// <summary>
    /// Helper class to retrieve credentials from user secrets or environment variables.
    /// Secrets are stored securely and not committed to source control.
    /// </summary>
    public static class CredentialsHelper
    {
        private static IConfiguration? _config;

        /// <summary>
        /// Gets the configuration builder with user secrets and environment variables.
        /// </summary>
        private static IConfiguration GetConfiguration()
        {
            if (_config != null)
                return _config;

            var configBuilder = new ConfigurationBuilder()
                .AddUserSecrets(typeof(CredentialsHelper).Assembly, optional: true)
                .AddEnvironmentVariables("TEST_");

            _config = configBuilder.Build();
            return _config;
        }

        /// <summary>
        /// Gets the username from user secrets or environment variable (TEST_USERNAME).
        /// </summary>
        public static string GetUsername()
        {
            var config = GetConfiguration();
            var username = config["Credentials:Username"] 
                ?? Environment.GetEnvironmentVariable("TEST_USERNAME");
            
            if (string.IsNullOrEmpty(username))
                throw new InvalidOperationException(
                    "Username not found. Set using: dotnet user-secrets set \"Credentials:Username\" \"<value>\" " +
                    "or set environment variable TEST_USERNAME");
            
            return username;
        }

        /// <summary>
        /// Gets the password from user secrets or environment variable (TEST_PASSWORD).
        /// </summary>
        public static string GetPassword()
        {
            var config = GetConfiguration();
            var password = config["Credentials:Password"] 
                ?? Environment.GetEnvironmentVariable("TEST_PASSWORD");
            
            if (string.IsNullOrEmpty(password))
                throw new InvalidOperationException(
                    "Password not found. Set using: dotnet user-secrets set \"Credentials:Password\" \"<value>\" " +
                    "or set environment variable TEST_PASSWORD");
            
            return password;
        }
    }
}
