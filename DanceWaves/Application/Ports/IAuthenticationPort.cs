using DanceWaves.Application.Dtos;
using DanceWaves.Models;

namespace DanceWaves.Application.Ports;

/// <summary>
/// Port for authentication and user management operations
/// </summary>
public interface IAuthenticationPort
{
    /// <summary>
    /// Authenticate user with local account (email/password)
    /// </summary>
    Task<AuthenticationResponse> LoginAsync(LoginRequest request);

    /// <summary>
    /// Register a new local user account
    /// </summary>
    Task<AuthenticationResponse> RegisterAsync(RegisterRequest request);

    /// <summary>
    /// Authenticate or create user from federated provider (Microsoft, Google, Apple, etc.)
    /// </summary>
    Task<AuthenticationResponse> FederatedLoginAsync(FederatedLoginRequest request);

    /// <summary>
    /// Get current user profile
    /// </summary>
    Task<UserDto?> GetCurrentUserAsync(string userId);

    /// <summary>
    /// Update user profile
    /// </summary>
    Task<AuthenticationResponse> UpdateProfileAsync(string userId, UserDto updatedProfile);

    /// <summary>
    /// Change password for local accounts
    /// </summary>
    Task<AuthenticationResponse> ChangePasswordAsync(string userId, ChangePasswordRequest request);

    /// <summary>
    /// Initiate password reset
    /// </summary>
    Task<AuthenticationResponse> RequestPasswordResetAsync(ResetPasswordRequest request);

    /// <summary>
    /// Confirm password reset with token
    /// </summary>
    Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordConfirmRequest request);

    /// <summary>
    /// Verify email address
    /// </summary>
    Task<AuthenticationResponse> VerifyEmailAsync(VerifyEmailRequest request);

    /// <summary>
    /// Check if email exists
    /// </summary>
    Task<bool> EmailExistsAsync(string email);

    // Métodos removidos: GetAuthUserByExternalIdAsync, GetAuthUserByEmailAsync

    /// <summary>
    /// Logout user
    /// </summary>
    Task LogoutAsync(string userId);

    /// <summary>
    /// Refresh access token
    /// </summary>
    Task<AuthenticationResponse> RefreshTokenAsync(string refreshToken);
}
