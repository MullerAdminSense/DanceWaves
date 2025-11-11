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
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                _logger.LogWarning($"Login attempt failed for email: {request.Email}");
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                };
            }
            // Here you should verify the password (implement hash/password in User)
            // Example: if (!PasswordHasher.VerifyPassword(request.Password, user.Password!)) { ... }
            // For now, let's assume the password is correct
            _logger.LogInformation($"User logged in successfully: {request.Email}");
            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Login successful",
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName ?? string.Empty,
                    LastName = user.LastName ?? string.Empty
                }
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
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Email already registered"
                };
            }
            var newUser = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                RolePermissionId = 4 // Jury
            };
            // Here you should save the password hash (implement Password field in User if necessary)
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"User registered successfully: {request.Email}");
            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Registration successful.",
                User = new UserDto
                {
                    Id = newUser.Id,
                    Email = newUser.Email,
                    FirstName = newUser.FirstName ?? string.Empty,
                    LastName = newUser.LastName ?? string.Empty
                }
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
    // Federated login not implemented for User. Return error or implement as needed.
        return new AuthenticationResponse
        {
            IsSuccess = false,
            Message = "Federated login não implementado."
        };
    }

    public async Task<UserDto?> GetCurrentUserAsync(string userId)
    {
        if (!int.TryParse(userId, out var id))
            return null;
        var user = await _dbContext.Users.FindAsync(id);
        return user != null ? new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty
        } : null;
    }

    public async Task<AuthenticationResponse> UpdateProfileAsync(string userId, UserDto updatedProfile)
    {
        if (!int.TryParse(userId, out var id))
        {
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "Invalid user ID"
            };
        }
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
        {
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = "User not found"
            };
        }
        user.FirstName = updatedProfile.FirstName;
        user.LastName = updatedProfile.LastName;
    // Adapt other fields as needed
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"User profile updated: {userId}");
        return new AuthenticationResponse
        {
            IsSuccess = true,
            Message = "Profile updated successfully",
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty
            }
        };
    }

    public async Task<AuthenticationResponse> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        // Implement password change logic for User
        return new AuthenticationResponse
        {
            IsSuccess = false,
            Message = "Password change not implemented."
        };
    }

    public async Task<AuthenticationResponse> RequestPasswordResetAsync(ResetPasswordRequest request)
    {
        // Implement password reset logic for User
        return new AuthenticationResponse
        {
            IsSuccess = false,
            Message = "Password reset not implemented."
        };
    }

    public async Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordConfirmRequest request)
    {
        try
        {
            var authUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email /* && u.Provider == "local" */);

            if (authUser == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }


            // Hash new password
            authUser.Password = PasswordHasher.HashPassword(request.NewPassword);

            _dbContext.Users.Update(authUser);
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
        // Implement email verification logic for User
        return new AuthenticationResponse
        {
            IsSuccess = false,
            Message = "Email verification not implemented."
        };
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email);
    }

    // Methods GetAuthUserByExternalIdAsync and GetAuthUserByEmailAsync removed

    public async Task LogoutAsync(string userId)
    {
    // Implement logout logic for User if needed
    }

    public async Task<AuthenticationResponse> RefreshTokenAsync(string refreshToken)
    {
        // Implement token refresh logic for User if needed
        return new AuthenticationResponse
        {
            IsSuccess = false,
            Message = "Token refresh not implemented."
        };
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

    // Method MapToUserDto removed
}
