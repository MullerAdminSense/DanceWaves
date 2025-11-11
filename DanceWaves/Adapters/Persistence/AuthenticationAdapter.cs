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
            // Aqui você deve verificar a senha (implementar hash/senha no User)
            // Exemplo: if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash!)) { ... }
            // Por enquanto, vamos assumir que a senha está correta
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
            // Aqui você deve salvar o hash da senha (implementar campo PasswordHash em User se necessário)
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
        // Federated login não implementado para User. Retorne erro ou implemente conforme necessário.
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
        // Adapte outros campos conforme necessário
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
        // Implementar lógica de alteração de senha para User
        return new AuthenticationResponse
        {
            IsSuccess = false,
            Message = "Alteração de senha não implementada."
        };
    }

    public async Task<AuthenticationResponse> RequestPasswordResetAsync(ResetPasswordRequest request)
    {
        // Implementar lógica de reset de senha para User
        return new AuthenticationResponse
        {
            IsSuccess = false,
            Message = "Reset de senha não implementado."
        };
    }

    public async Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordConfirmRequest request)
    {
        // Implementar lógica de confirmação de reset de senha para User
        return new AuthenticationResponse
        {
            IsSuccess = false,
            Message = "Confirmação de reset de senha não implementada."
        };
    }

    public async Task<AuthenticationResponse> VerifyEmailAsync(VerifyEmailRequest request)
    {
        // Implementar lógica de verificação de email para User
        return new AuthenticationResponse
        {
            IsSuccess = false,
            Message = "Verificação de email não implementada."
        };
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email);
    }

    // Métodos GetAuthUserByExternalIdAsync e GetAuthUserByEmailAsync removidos

    public async Task LogoutAsync(string userId)
    {
        // Implementar lógica de logout para User se necessário
    }

    public async Task<AuthenticationResponse> RefreshTokenAsync(string refreshToken)
    {
        // Implementar lógica de refresh de token para User se necessário
        return new AuthenticationResponse
        {
            IsSuccess = false,
            Message = "Refresh de token não implementado."
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

    // Método MapToUserDto removido
}
