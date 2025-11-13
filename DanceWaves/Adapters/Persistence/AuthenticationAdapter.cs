using DanceWaves.Application.Dtos;
using DanceWaves.Application.Ports;
using DanceWaves.Data;
using DanceWaves.Models;
using DanceWaves.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Serilog;

namespace DanceWaves.Adapters.Persistence
{
	public class AuthenticationAdapter : IAuthenticationPort
	{
		private readonly ApplicationDbContext _dbContext;

		public AuthenticationAdapter(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<DanceSchool>> GetAllDanceSchoolsAsync()
		{
			return await _dbContext.DanceSchools.ToListAsync();
		}

		public async Task<List<Franchise>> GetFranchisesByDanceSchoolAsync(int danceSchoolId)
		{
            
			return await _dbContext.Franchises
				.Where(f => _dbContext.DanceSchools.Any(ds => ds.Id == danceSchoolId && ds.DefaultFranchiseId == f.Id))
				.ToListAsync();
		}

		public async Task<List<UserRolePermission>> GetAllRolePermissionsAsync()
		{
			return await _dbContext.UserRolePermissions.ToListAsync();
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
				RolePermissionId = user.RolePermissionId
			};
		}

		public async Task<AuthenticationResponse> UpdateProfileAsync(string userId, UserDto updatedProfile) => throw new NotImplementedException();
		public async Task<AuthenticationResponse> ChangePasswordAsync(string userId, ChangePasswordRequest request) => throw new NotImplementedException();
		public async Task<AuthenticationResponse> RequestPasswordResetAsync(ResetPasswordRequest request) => throw new NotImplementedException();
		public async Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordConfirmRequest request) => throw new NotImplementedException();
		public async Task<AuthenticationResponse> VerifyEmailAsync(VerifyEmailRequest request) => throw new NotImplementedException();
		public async Task<bool> EmailExistsAsync(string email) => throw new NotImplementedException();

		public async Task LogoutAsync(string userId)
		{
			if (string.IsNullOrWhiteSpace(userId) || !int.TryParse(userId, out var id))
				return;

			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
			if (user != null)
			{
				Log.Information("User {UserId} ({Email}) logged out at {Time}", user.Id, user.Email, DateTime.UtcNow);
			}

			Log.Information("Logout process completed for user {UserId}", userId);
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
}
