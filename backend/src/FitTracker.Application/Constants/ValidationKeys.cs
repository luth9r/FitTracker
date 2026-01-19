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
            public const string NotSameAsOld = "Validation.User.Password.NotSameAsOld";
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

    public static class Exercise
    {
        public const string NameRequired = "Validation.Exercise.Name.Required";
        public const string NameLength = "Validation.Exercise.Name.Length";
        public const string EquipmentNotValid = "Validation.Exercise.Equipment.NotValid";
        public const string MuscleGroupNotValid = "Validation.Exercise.MuscleGroup.NotValid";
        public const string DescriptionLength = "Validation.Exercise.Description.Length";
        public const string FileSizeTooLarge = "Validation.Exercise.File.TooLarge";
        public const string InvalidImageType = "Validation.Exercise.InvalidImageType";

        public const string NameAlreadyExists = "Validation.Exercise.VideoUrl.NameAlreadyExists";
    }

    public static class Template
    {
        public const string NameRequired = "Validation.Template.Name.Required";
        public const string NameMaxLength = "Validation.Template.Name.MaxLength";
    }
}
