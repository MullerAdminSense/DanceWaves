using DanceWaves.Application.Dtos;
using DanceWaves.Models;

namespace DanceWaves.Application.Ports;

 
public interface IAuthenticationPort
{
    
    Task<AuthenticationResponse> LoginAsync(LoginRequest request);

    
    Task<AuthenticationResponse> RegisterAsync(RegisterRequest request);

    
    Task<AuthenticationResponse> FederatedLoginAsync(FederatedLoginRequest request);

    
    Task<UserDto?> GetCurrentUserAsync(string userId);

    
    Task<AuthenticationResponse> UpdateProfileAsync(string userId, UserDto updatedProfile);

    
    Task<AuthenticationResponse> ChangePasswordAsync(string userId, ChangePasswordRequest request);

    
    Task<AuthenticationResponse> RequestPasswordResetAsync(ResetPasswordRequest request);

    
    Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordConfirmRequest request);

    
    Task<AuthenticationResponse> VerifyEmailAsync(VerifyEmailRequest request);

    
    Task<bool> EmailExistsAsync(string email);

    

    
    Task LogoutAsync(string userId);

    
    Task<AuthenticationResponse> RefreshTokenAsync(string refreshToken);
    
    Task<List<DanceSchool>> GetAllDanceSchoolsAsync();

    
    Task<List<Franchise>> GetFranchisesByDanceSchoolAsync(int danceSchoolId);

    
    Task<List<UserRolePermission>> GetAllRolePermissionsAsync();
}
