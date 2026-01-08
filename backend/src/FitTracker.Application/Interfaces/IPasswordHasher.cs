namespace FitTracker.Application.Interfaces;

/// <summary>
///     Service for password hashing and verification.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    ///     Hash plain text password.
    /// </summary>
    string HashPassword(string password);

    /// <summary>
    ///     Verify password against hash.
    /// </summary>
    bool VerifyPassword(string password, string hash);
}