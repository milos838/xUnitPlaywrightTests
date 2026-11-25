using Microsoft.Extensions.Configuration;

namespace PlaywrightTests.Utilities
{
    /// <summary>
    /// Resolves a complete credential pair from environment variables or local user secrets.
    /// </summary>
    public static class CredentialsHelper
    {
        private static readonly Lazy<(string Username, string Password)> Credentials = new(LoadCredentials);

        /// <summary>
        /// Resolves and validates both values together on first use.
        /// </summary>
        private static (string Username, string Password) LoadCredentials()
        {
            var username = Environment.GetEnvironmentVariable("TEST_USERNAME");
            var password = Environment.GetEnvironmentVariable("TEST_PASSWORD");

            if (username is not null || password is not null)
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                    throw new InvalidOperationException(
                        "Environment credentials require both TEST_USERNAME and TEST_PASSWORD to be nonblank. " +
                        "User Secrets fallback is disabled when either variable is defined.");

                return (username, password);
            }

            if (IsCiEnvironment())
                throw new InvalidOperationException(
                    "CI authentication requires both TEST_USERNAME and TEST_PASSWORD. " +
                    "Local User Secrets fallback is disabled in CI.");

            var config = new ConfigurationBuilder()
                .AddUserSecrets(typeof(CredentialsHelper).Assembly, optional: true)
                .Build();
            using var configLifetime = config as IDisposable;

            username = config["Credentials:Username"];
            password = config["Credentials:Password"];

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException(
                    "Local authentication requires nonblank Credentials:Username and Credentials:Password " +
                    "in User Secrets, or a complete TEST_USERNAME and TEST_PASSWORD environment pair.");

            return (username, password);
        }

        private static bool IsCiEnvironment()
        {
            return string.Equals(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"),
                       "true", StringComparison.OrdinalIgnoreCase)
                || !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("JENKINS_URL"))
                || !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("JENKINS_HOME"))
                || Environment.GetEnvironmentVariable("BUILD_TAG")
                    ?.StartsWith("jenkins-", StringComparison.OrdinalIgnoreCase) == true;
        }

        /// <summary>
        /// Gets the username from the validated credential pair.
        /// </summary>
        public static string GetUsername()
        {
            return Credentials.Value.Username;
        }

        /// <summary>
        /// Gets the password from the validated credential pair.
        /// </summary>
        public static string GetPassword()
        {
            return Credentials.Value.Password;
        }
    }
}
