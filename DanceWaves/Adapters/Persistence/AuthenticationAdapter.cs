using DanceWaves.Application.Dtos;
using DanceWaves.Application.Ports;
using DanceWaves.Data;
using DanceWaves.Models;
using DanceWaves.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace DanceWaves.Adapters.Persistence;

/// <summary>
/// Adapter for authentication operations using Entity Framework Core
/// </summary>
public class AuthenticationAdapter : IAuthenticationPort
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<AuthenticationAdapter> _logger;

    public AuthenticationAdapter(
        ApplicationDbContext dbContext,
        ILogger<AuthenticationAdapter> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<AuthenticationResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var authUser = await _dbContext.AuthenticationUsers
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Provider == "local");

            if (authUser == null || !authUser.IsActive)
            {
                _logger.LogWarning($"Login attempt failed for email: {request.Email}");
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                };
            }

            // Verify password
            if (!PasswordHasher.VerifyPassword(request.Password, authUser.PasswordHash!))
            {
                _logger.LogWarning($"Login password verification failed for email: {request.Email}");
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                };
            }

            // Update last login
            authUser.LastLoginAt = DateTime.UtcNow;
            _dbContext.AuthenticationUsers.Update(authUser);
            await _dbContext.SaveChangesAsync();

            // Generate tokens
            var accessToken = GenerateAccessToken(authUser.Id);
            var refreshToken = GenerateRefreshToken();

            authUser.RefreshToken = refreshToken;
            authUser.TokenExpiryAt = DateTime.UtcNow.AddDays(7);
            _dbContext.AuthenticationUsers.Update(authUser);
            await _dbContext.SaveChangesAsync();

            var userDto = MapToUserDto(authUser);

            _logger.LogInformation($"User logged in successfully: {request.Email}");

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Login successful",
                User = userDto,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "An error occurred during login"
            };
        }
    }

    public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            // Check if email already exists
            var existingUser = await _dbContext.AuthenticationUsers
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Email already registered"
                };
            }

            // Create new authentication user
            var authUser = new AuthenticationUser
            {
                Provider = "local",
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                IsEmailVerified = false,
                CreatedAt = DateTime.UtcNow
            };

            // Hash password
            authUser.PasswordHash = PasswordHasher.HashPassword(request.Password);

            _dbContext.AuthenticationUsers.Add(authUser);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"User registered successfully: {request.Email}");

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Registration successful. Please verify your email.",
                User = MapToUserDto(authUser)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "An error occurred during registration"
            };
        }
    }

    public async Task<AuthenticationResponse> FederatedLoginAsync(FederatedLoginRequest request)
    {
        try
        {
            // Check if federated user exists
            var authUser = await _dbContext.AuthenticationUsers
                .FirstOrDefaultAsync(u =>
                    u.Provider == request.Provider &&
                    u.ExternalUserId == request.ExternalUserId);

            if (authUser != null && authUser.IsActive)
            {
                // Update last login
                authUser.LastLoginAt = DateTime.UtcNow;
                _dbContext.AuthenticationUsers.Update(authUser);
                await _dbContext.SaveChangesAsync();

                var accessToken = GenerateAccessToken(authUser.Id);
                var refreshToken = GenerateRefreshToken();

                authUser.RefreshToken = refreshToken;
                authUser.TokenExpiryAt = DateTime.UtcNow.AddDays(7);
                _dbContext.AuthenticationUsers.Update(authUser);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Federated user logged in: {request.Provider} - {request.Email}");

                return new AuthenticationResponse
                {
                    IsSuccess = true,
                    Message = "Federated login successful",
                    User = MapToUserDto(authUser),
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddHours(1)
                };
            }

            // Create new federated user
            var newAuthUser = new AuthenticationUser
            {
                Provider = request.Provider,
                ExternalUserId = request.ExternalUserId,
                Email = request.Email,
                FirstName = request.FirstName ?? "User",
                LastName = request.LastName ?? string.Empty,
                ProfilePictureUrl = request.ProfilePictureUrl,
                IsEmailVerified = true, // External providers verify email
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow
            };

            _dbContext.AuthenticationUsers.Add(newAuthUser);
            await _dbContext.SaveChangesAsync();

            var newAccessToken = GenerateAccessToken(newAuthUser.Id);
            var newRefreshToken = GenerateRefreshToken();

            newAuthUser.RefreshToken = newRefreshToken;
            newAuthUser.TokenExpiryAt = DateTime.UtcNow.AddDays(7);
            _dbContext.AuthenticationUsers.Update(newAuthUser);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"New federated user created and logged in: {request.Provider} - {request.Email}");

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Federated account created and linked successfully",
                User = MapToUserDto(newAuthUser),
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during federated login");
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "An error occurred during federated login"
            };
        }
    }

    public async Task<UserDto?> GetCurrentUserAsync(string userId)
    {
        try
        {
            if (!int.TryParse(userId, out var id))
                return null;

            var authUser = await _dbContext.AuthenticationUsers.FindAsync(id);
            return authUser != null ? MapToUserDto(authUser) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current user");
            return null;
        }
    }

    public async Task<AuthenticationResponse> UpdateProfileAsync(string userId, UserDto updatedProfile)
    {
        try
        {
            if (!int.TryParse(userId, out var id))
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Invalid user ID"
                };
            }

            var authUser = await _dbContext.AuthenticationUsers.FindAsync(id);
            if (authUser == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            authUser.FirstName = updatedProfile.FirstName;
            authUser.LastName = updatedProfile.LastName;
            authUser.PhoneNumber = updatedProfile.PhoneNumber;

            _dbContext.AuthenticationUsers.Update(authUser);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"User profile updated: {userId}");

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Profile updated successfully",
                User = MapToUserDto(authUser)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating profile");
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "An error occurred updating profile"
            };
        }
    }

    public async Task<AuthenticationResponse> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        try
        {
            if (!int.TryParse(userId, out var id))
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Invalid user ID"
                };
            }

            var authUser = await _dbContext.AuthenticationUsers.FindAsync(id);
            if (authUser == null || authUser.Provider != "local")
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "User not found or is not a local account"
                };
            }

            // Verify current password
            if (!PasswordHasher.VerifyPassword(request.CurrentPassword, authUser.PasswordHash!))
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Current password is incorrect"
                };
            }

            // Hash new password
            authUser.PasswordHash = PasswordHasher.HashPassword(request.NewPassword);

            _dbContext.AuthenticationUsers.Update(authUser);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Password changed for user: {userId}");

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Password changed successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password");
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "An error occurred changing password"
            };
        }
    }

    public async Task<AuthenticationResponse> RequestPasswordResetAsync(ResetPasswordRequest request)
    {
        try
        {
            var authUser = await _dbContext.AuthenticationUsers
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Provider == "local");

            if (authUser == null)
            {
                // Don't reveal if email exists for security
                return new AuthenticationResponse
                {
                    IsSuccess = true,
                    Message = "If email exists, a reset link will be sent"
                };
            }

            // Generate reset token
            var resetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            _logger.LogInformation($"Password reset requested for: {request.Email}");

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Password reset link sent to email"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting password reset");
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "An error occurred requesting password reset"
            };
        }
    }

    public async Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordConfirmRequest request)
    {
        try
        {
            var authUser = await _dbContext.AuthenticationUsers
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Provider == "local");

            if (authUser == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }


            // Hash new password
            authUser.PasswordHash = PasswordHasher.HashPassword(request.NewPassword);

            _dbContext.AuthenticationUsers.Update(authUser);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Password reset completed for: {request.Email}");

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Password reset successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting password");
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "An error occurred resetting password"
            };
        }
    }

    public async Task<AuthenticationResponse> VerifyEmailAsync(VerifyEmailRequest request)
    {
        try
        {
            var authUser = await _dbContext.AuthenticationUsers
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (authUser == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            authUser.IsEmailVerified = true;
            _dbContext.AuthenticationUsers.Update(authUser);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Email verified for: {request.Email}");

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Email verified successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying email");
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "An error occurred verifying email"
            };
        }
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _dbContext.AuthenticationUsers
            .AnyAsync(u => u.Email == email);
    }

    public async Task<AuthenticationUser?> GetAuthUserByExternalIdAsync(string provider, string externalUserId)
    {
        return await _dbContext.AuthenticationUsers
            .FirstOrDefaultAsync(u =>
                u.Provider == provider &&
                u.ExternalUserId == externalUserId);
    }

    public async Task<AuthenticationUser?> GetAuthUserByEmailAsync(string email)
    {
        return await _dbContext.AuthenticationUsers
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task LogoutAsync(string userId)
    {
        try
        {
            if (!int.TryParse(userId, out var id))
                return;

            var authUser = await _dbContext.AuthenticationUsers.FindAsync(id);
            if (authUser != null)
            {
                authUser.RefreshToken = null;
                authUser.TokenExpiryAt = null;
                _dbContext.AuthenticationUsers.Update(authUser);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"User logged out: {userId}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
        }
    }

    public async Task<AuthenticationResponse> RefreshTokenAsync(string refreshToken)
    {
        try
        {
            var authUser = await _dbContext.AuthenticationUsers
                .FirstOrDefaultAsync(u =>
                    u.RefreshToken == refreshToken &&
                    u.TokenExpiryAt > DateTime.UtcNow);

            if (authUser == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Invalid or expired refresh token"
                };
            }

            var newAccessToken = GenerateAccessToken(authUser.Id);
            var newRefreshToken = GenerateRefreshToken();

            authUser.RefreshToken = newRefreshToken;
            authUser.TokenExpiryAt = DateTime.UtcNow.AddDays(7);
            _dbContext.AuthenticationUsers.Update(authUser);
            await _dbContext.SaveChangesAsync();

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Token refreshed successfully",
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing token");
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "An error occurred refreshing token"
            };
        }
    }

    // Helper methods
    private string GenerateAccessToken(int userId)
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    private string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    private UserDto MapToUserDto(AuthenticationUser authUser)
    {
        return new UserDto
        {
            Id = authUser.Id,
            Email = authUser.Email,
            FirstName = authUser.FirstName,
            LastName = authUser.LastName,
            PhoneNumber = authUser.PhoneNumber,
            ProfilePictureUrl = authUser.ProfilePictureUrl,
            Provider = authUser.Provider,
            LastLoginAt = authUser.LastLoginAt ?? DateTime.UtcNow,
            IsEmailVerified = authUser.IsEmailVerified
        };
    }
}
