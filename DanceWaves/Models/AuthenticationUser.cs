namespace DanceWaves.Models;

/// <summary>
/// Authentication-specific user model for login/registration
/// </summary>
public class AuthenticationUser
{
    public int Id { get; set; }

    /// <summary>
    /// External identity provider (e.g., "microsoft", "google", "apple", or "local")
    /// </summary>
    public required string Provider { get; set; }

    /// <summary>
    /// External provider's unique identifier
    /// </summary>
    public string? ExternalUserId { get; set; }

    /// <summary>
    /// Email address (primary key for local accounts)
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Hashed password (null for federated accounts)
    /// </summary>
    public string? PasswordHash { get; set; }

    /// <summary>
    /// First name
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// Phone number (optional)
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Profile picture URL
    /// </summary>
    public string? ProfilePictureUrl { get; set; }

    /// <summary>
    /// Indicates if email is verified
    /// </summary>
    public bool IsEmailVerified { get; set; }

    /// <summary>
    /// Account creation date
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last login date
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Is account active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Navigation property to User
    /// </summary>
    public int? UserId { get; set; }
    public User? User { get; set; }

    /// <summary>
    /// Refresh tokens for federated accounts
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Token expiration date
    /// </summary>
    public DateTime? TokenExpiryAt { get; set; }
}
