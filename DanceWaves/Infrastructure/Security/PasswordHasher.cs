using System.Security.Cryptography;
using System.Text;

namespace DanceWaves.Infrastructure.Security;

/// <summary>
/// Utility class for password hashing and verification
/// </summary>
public static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 10000;

    /// <summary>
    /// Hash a password
    /// </summary>
    public static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var key = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
    }

    /// <summary>
    /// Verify a password against a hash
    /// </summary>
    public static bool VerifyPassword(string password, string hash)
    {
        try
        {
            var parts = hash.Split('.');
            if (parts.Length != 3)
            {
                return false;
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            var keyToCheck = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, HashSize);
            return keyToCheck.SequenceEqual(key);
        }
        catch
        {
            return false;
        }
    }
}
