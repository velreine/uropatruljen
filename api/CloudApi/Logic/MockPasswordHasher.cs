using System.Text;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Identity;

namespace CloudApi.Logic;

/// <summary>
/// Beware this is not a cryptographically secure "hasher" this is actually just used for mocking.
/// </summary>
public class MockPasswordHasher : IPasswordHasher<Person>
{
    /// <summary>
    /// *MOCK* hashes the password by base64 encoding it.
    /// </summary>
    /// <param name="user">The user object (UNUSED)</param>
    /// <param name="password">The plain-text password.</param>
    /// <returns></returns>
    public string HashPassword(Person user, string password)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
    }

    /// <summary>
    /// *MOCK* Verifies the password by base64 encoding the provided password,
    /// and then checking if it matches the "hashedPassword".
    /// </summary>
    /// <param name="user"></param>
    /// <param name="hashedPassword">The "hashed" (base64 encoded password, usually from Database)</param>
    /// <param name="providedPassword">The plain-text password to be matched against the hashed password.</param>
    /// <returns></returns>
    public PasswordVerificationResult VerifyHashedPassword(Person user, string hashedPassword, string providedPassword)
    {
        // Convert the provided plain-text password to Base64.
        var providedHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(providedPassword));

        // Check that the hashed password (the existing base 64'ed) matches the provided hashes password.
        return hashedPassword == providedHash
                ? PasswordVerificationResult.Success
                : PasswordVerificationResult.Failed
            ;
    }
}