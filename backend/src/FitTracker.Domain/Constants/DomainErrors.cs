namespace FitTracker.Domain.Constants;

public class DomainErrors
{
    public static class Google
    {
        public const string InvalidToken = "Google.Auth.InvalidToken";
        public const string NotFound = "Google.Auth.NotFound";
    }

    public static class Auth
    {
        public const string AccountAlreadyExists = "Auth.AccountAlreadyExists";
        public const string InvalidCredentials = "Auth.InvalidCredentials";
        public const string UsernameAlreadyExists = "Auth.UsernameAlreadyExists";
        public const string EmailAlreadyExists = "Auth.EmailAlreadyExists";
        public const string InvalidToken = "Auth.InvalidToken";
    }

    public static class User
    {
        public const string NotFound = "User.NotFound";
        public const string EmailAlreadyVerified = "User.EmailAlreadyVerified";
        public const string EmailNotVerified = "User.EmailNotVerified";
        public const string RateLimitExceeded = "User.RateLimitExceeded";
        public const string InvalidPassword = "User.InvalidPassword";
    }

    public static class Exercise
    {
        public const string AlreadyExists = "Exercise.AlreadyExists";
        public const string NotFound = "Exercise.NotFound";
    }
}
