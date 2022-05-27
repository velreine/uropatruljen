using System.Text;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Identity;

namespace CloudApi.Logic;

/**
 * Beware this is not a cryptographically secure "hasher" this is actually just used for mocking.
 */
public class MockPasswordHasher : IPasswordHasher<Person>
{
    public string HashPassword(Person user, string password)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
    }

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