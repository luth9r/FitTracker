namespace FitTracker.Application.Constants;

public static class ValidationKeys
{
    public static class User
    {
        public static class Email
        {
            public const string Required = "Validation.User.Email.Required";
            public const string InvalidFormat = "Validation.User.Email.InvalidFormat";
        }

        public static class Password
        {
            public const string Required = "Validation.User.Password.Required";
            public const string Length = "Validation.User.Password.Length";
            public const string LetterRequired = "Validation.User.Password.LetterRequired";
            public const string NumberRequired = "Validation.User.Password.NumberRequired";
        }

        public static class Username
        {
            public const string Required = "Validation.User.Username.Required";
            public const string Length = "Validation.User.Username.Length";
        }
    }

    public static class Google
    {
        public const string CodeRequired = "Validation.Google.Code.Required";
        public const string CodeVerifierRequired = "Validation.Google.CodeVerifier.Required";
    }
}
