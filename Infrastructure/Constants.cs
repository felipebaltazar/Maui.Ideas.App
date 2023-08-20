namespace Maui.Ideas.App.Infrastructure
{
    public static class Constants
    {
        public static class Policy
        {
            public const int MAX_REFRESH_TOKEN_ATTEMPTS = 2;

            public const int MAX_RETRY_ATTEMPTS = 3;
        }

        public static class Api
        {
            public const string BASE_URL = "https://raw.githubusercontent.com";
        }
    }
}
