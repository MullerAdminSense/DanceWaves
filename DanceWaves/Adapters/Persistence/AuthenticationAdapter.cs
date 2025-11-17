using DanceWaves.Application.Dtos;
using DanceWaves.Application.Ports;
using DanceWaves.Data;
using DanceWaves.Models;
using DanceWaves.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Serilog;

namespace DanceWaves.Adapters.Persistence;

/// <summary>
/// Adaptador de autenticação
/// Implementa a porta IAuthenticationPort
/// </summary>
public class AuthenticationAdapter(ApplicationDbContext dbContext) : IAuthenticationPort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<DanceSchool>> GetAllDanceSchoolsAsync()
    {
        // Sempre busca dados atualizados do banco sem cache
        return await _dbContext.DanceSchools
            .AsNoTracking()
            .OrderBy(ds => ds.LegalName)
            .ToListAsync();
    }

    public async Task<List<Franchise>> GetAllFranchisesAsync()
    {
        // Sempre busca dados atualizados do banco sem cache
        return await _dbContext.Franchises
            .AsNoTracking()
            .OrderBy(f => f.LegalName)
            .ToListAsync();
    }

    public async Task<List<Franchise>> GetFranchisesByDanceSchoolAsync(int danceSchoolId)
    {
        // Sempre busca dados atualizados do banco sem cache
        // Busca a DanceSchool primeiro para obter informações relacionadas
        var danceSchool = await _dbContext.DanceSchools
            .AsNoTracking()
            .FirstOrDefaultAsync(ds => ds.Id == danceSchoolId);

        if (danceSchool == null)
        {
            return new List<Franchise>();
        }

        // Se a DanceSchool tem uma Franchise padrão, inclui essa Franchise na lista
        var franchises = new List<Franchise>();
        
        if (danceSchool.DefaultFranchiseId.HasValue)
        {
            var defaultFranchise = await _dbContext.Franchises
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == danceSchool.DefaultFranchiseId.Value);

            if (defaultFranchise != null)
            {
                franchises.Add(defaultFranchise);
            }
        }

        // Também busca todas as outras Franchises do mesmo país
        // para dar mais opções ao usuário
        var countryFranchises = await _dbContext.Franchises
            .AsNoTracking()
            .Where(f => f.CountryId == danceSchool.CountryId)
            .OrderBy(f => f.LegalName)
            .ToListAsync();

        // Remove duplicatas mantendo a ordem (DefaultFranchise primeiro, se houver)
        var result = franchises.Union(countryFranchises)
            .GroupBy(f => f.Id)
            .Select(g => g.First())
            .OrderByDescending(f => danceSchool.DefaultFranchiseId == f.Id) // DefaultFranchise primeiro
            .ThenBy(f => f.LegalName)
            .ToList();

        return result;
    }

    public async Task<List<UserRolePermission>> GetAllRolePermissionsAsync()
    {
        return await _dbContext.UserRolePermissions.ToListAsync();
    }

    public async Task<List<AgeGroup>> GetAllAgeGroupsAsync()
    {
        // Sempre busca dados atualizados do banco sem cache
        return await _dbContext.AgeGroups
            .AsNoTracking()
            .OrderBy(ag => ag.Name)
            .ToListAsync();
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        // Sempre busca dados atualizados do banco sem cache
        var users = await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.RolePermission)
            .Include(u => u.DanceSchool)
            .Include(u => u.DefaultFranchise)
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .ToListAsync();

        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Email = u.Email,
            FirstName = u.FirstName ?? string.Empty,
            LastName = u.LastName ?? string.Empty,
            PhoneNumber = u.Phone,
            Phone = u.Phone,
            Provider = "local",
            Address = u.Address,
            City = u.City,
            Zip = u.Zip,
            Province = u.Province,
            CountryId = u.CountryId,
            DanceSchoolId = u.DanceSchoolId,
            DefaultFranchiseId = u.DefaultFranchiseId,
            AgeGroupId = u.AgeGroupId,
            RolePermissionId = u.RolePermissionId
        }).ToList();
    }

    public async Task<AuthenticationResponse> DeleteUserAsync(int userId)
    {
        try
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "User not found."
                };
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            Log.Information("User deleted: {Email} (ID: {UserId})", user.Email, user.Id);

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "User deleted successfully."
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting user: {UserId}", userId);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"An error occurred while deleting the user: {ex.Message}"
            };
        }
    }

    public async Task<AuthenticationResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _dbContext.Users.Include(u => u.RolePermission).FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                };
            }
            if (string.IsNullOrEmpty(user.Password) || !PasswordHasher.VerifyPassword(request.Password, user.Password))
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                };
            }
            var role = user.RolePermission?.Name ?? "User";
            var token = GenerateJwtToken(user, role);
            Log.Information("User logged in: {Email} (ID: {UserId})", user.Email, user.Id);
            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Login successful",
                AccessToken = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName ?? string.Empty,
                    LastName = user.LastName ?? string.Empty,
                    RolePermissionId = user.RolePermissionId
                }
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Login error for email: {Email}", request.Email);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"An error occurred during login: {ex.Message}"
            };
        }
    }

    public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "All fields are required."
                };
            }
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Email already registered"
                };
            }
            var rolePermissionId = 3;
            var rolePermission = await _dbContext.UserRolePermissions.FindAsync(rolePermissionId);
            if (rolePermission == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Role permission not allowed."
                };
            }
            var newUser = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                RolePermissionId = rolePermissionId,
                Password = PasswordHasher.HashPassword(request.Password)
            };
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();
            Log.Information("User registered: {Email} (ID: {UserId})", newUser.Email, newUser.Id);
            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Registration successful"
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Registration error for email: {Email}", request.Email);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"An error occurred during registration: {ex.Message}"
            };
        }
    }

    public async Task<AuthenticationResponse> FederatedLoginAsync(FederatedLoginRequest request)
    {
        Log.Information("Federated login attempted for provider: {Provider}", request.Provider);
        throw new NotImplementedException();
    }

    public async Task<UserDto?> GetCurrentUserAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return null;
        if (!int.TryParse(userId, out var id))
            return null;
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            Log.Warning("GetCurrentUserAsync: User not found for ID {UserId}", userId);
            return null;
        }
        Log.Information("GetCurrentUserAsync: User found {Email} (ID: {UserId})", user.Email, user.Id);
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            PhoneNumber = user.Phone,
            Provider = "local",
            Address = user.Address,
            City = user.City,
            Zip = user.Zip,
            Province = user.Province,
            CountryId = user.CountryId,
            DanceSchoolId = user.DanceSchoolId,
            Phone = user.Phone,
            DefaultFranchiseId = user.DefaultFranchiseId,
            AgeGroupId = user.AgeGroupId,
            RolePermissionId = user.RolePermissionId
        };
    }

    public async Task<AuthenticationResponse> UpdateProfileAsync(string userId, UserDto updatedProfile)
    {
        if (string.IsNullOrWhiteSpace(userId) || !int.TryParse(userId, out var id))
            return new AuthenticationResponse { IsSuccess = false, Message = "Invalid user id." };

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            return new AuthenticationResponse { IsSuccess = false, Message = "User not found." };

        user.FirstName = updatedProfile.FirstName;
        user.LastName = updatedProfile.LastName;
        user.Address = updatedProfile.Address;
        user.City = updatedProfile.City;
        user.Zip = updatedProfile.Zip;
        user.Province = updatedProfile.Province;
        user.CountryId = updatedProfile.CountryId ?? user.CountryId;
        user.Email = updatedProfile.Email;
        user.DanceSchoolId = updatedProfile.DanceSchoolId;
        user.Phone = updatedProfile.Phone;
        user.DefaultFranchiseId = updatedProfile.DefaultFranchiseId;
        user.AgeGroupId = updatedProfile.AgeGroupId;
        user.RolePermissionId = updatedProfile.RolePermissionId;

        await _dbContext.SaveChangesAsync();

        return new AuthenticationResponse { IsSuccess = true, Message = "Profile updated successfully." };
    }
    public async Task<AuthenticationResponse> ChangePasswordAsync(string userId, ChangePasswordRequest request) => throw new NotImplementedException();
    public async Task<AuthenticationResponse> RequestPasswordResetAsync(ResetPasswordRequest request) => throw new NotImplementedException();
    public async Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordConfirmRequest request) => throw new NotImplementedException();
    public async Task<AuthenticationResponse> VerifyEmailAsync(VerifyEmailRequest request) => throw new NotImplementedException();
    public async Task<bool> EmailExistsAsync(string email) => throw new NotImplementedException();

    public async Task LogoutAsync(string userId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId) || !int.TryParse(userId, out var id))
            {
                Log.Warning("LogoutAsync called with invalid userId: {UserId}", userId);
                return;
            }

            // Usa AsNoTracking para evitar problemas de concorrência e não precisa salvar mudanças
            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
                
            if (user != null)
            {
                Log.Information("User {UserId} ({Email}) logged out at {Time}", user.Id, user.Email, DateTime.UtcNow);
            }
            else
            {
                Log.Warning("LogoutAsync: User not found with ID {UserId}", id);
            }

            Log.Information("Logout process completed for user {UserId}", userId);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in LogoutAsync for user {UserId}: {Error}", userId, ex.Message);
            // Não relança a exceção para não interromper o processo de logout
        }
    }

    public async Task<AuthenticationResponse> RefreshTokenAsync(string refreshToken) => throw new NotImplementedException();

    private string GenerateJwtToken(User user, string role)
    {
        var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("DanceWavesSuperSecretKey_2025_ChangeMe!@#1234567890"));
        var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);
        var firstName = user.FirstName ?? string.Empty;
        var lastName = user.LastName ?? string.Empty;
        var fullName = $"{firstName} {lastName}".Trim();
        if (string.IsNullOrWhiteSpace(fullName))
        {
            fullName = user.Email ?? "User";
        }
        var claims = new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.Email ?? "User"),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email ?? string.Empty),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role),
            new System.Security.Claims.Claim("FullName", fullName),
            new System.Security.Claims.Claim("FirstName", firstName),
            new System.Security.Claims.Claim("LastName", lastName)
        };
        var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
            issuer: "DanceWaves",
            audience: "DanceWavesClient",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );
        return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
    }
}
