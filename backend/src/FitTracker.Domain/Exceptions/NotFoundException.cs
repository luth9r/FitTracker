namespace FitTracker.Domain.Exceptions
{
    /// <summary>
    /// Represents an application-specific exception that is thrown when a requested resource cannot be found.
    /// </summary>
    public class NotFoundException : Exception
    {
        public string ErrorCode { get; }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, string errorCode)
        : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
