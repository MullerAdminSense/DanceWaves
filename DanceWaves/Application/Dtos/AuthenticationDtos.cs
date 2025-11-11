namespace DanceWaves.Application.Dtos;

/// <summary>
/// DTO for user login request
/// </summary>
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; }
}

/// <summary>
/// DTO for user registration request
/// </summary>
public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool AcceptTerms { get; set; }
}

/// <summary>
/// DTO for federated account linking
/// </summary>
public class FederatedLoginRequest
{
    public required string Provider { get; set; }
    public required string ExternalUserId { get; set; }
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfilePictureUrl { get; set; }
}

/// <summary>
/// DTO for authentication response
/// </summary>
public class AuthenticationResponse
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public UserDto? User { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

/// <summary>
/// DTO for user profile information
/// </summary>
public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}

/// <summary>
/// DTO for password change request
/// </summary>
public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// DTO for password reset request
/// </summary>
public class ResetPasswordRequest
{
    public required string Email { get; set; }
}

/// <summary>
/// DTO for password reset confirmation
/// </summary>
public class ResetPasswordConfirmRequest
{
    public required string Email { get; set; }
    public required string ResetToken { get; set; }
    public required string NewPassword { get; set; }
    public required string ConfirmPassword { get; set; }
}

/// <summary>
/// DTO for email verification
/// </summary>
public class VerifyEmailRequest
{
    public required string Email { get; set; }
    public required string VerificationCode { get; set; }
}
