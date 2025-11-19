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

    
    Task<List<Franchise>> GetAllFranchisesAsync();

    
    Task<List<UserRolePermission>> GetAllRolePermissionsAsync();

    Task<AuthenticationResponse> CreateRolePermissionAsync(UserRolePermission rolePermission);

    Task<AuthenticationResponse> UpdateRolePermissionAsync(UserRolePermission rolePermission);

    Task<AuthenticationResponse> DeleteRolePermissionAsync(int rolePermissionId);

    
    Task<List<AgeGroup>> GetAllAgeGroupsAsync();

    Task<List<Style>> GetAllStylesAsync();

    Task<AuthenticationResponse> CreateStyleAsync(Style style);

    Task<AuthenticationResponse> UpdateStyleAsync(Style style);

    Task<AuthenticationResponse> DeleteStyleAsync(int styleId);

    Task<List<EntryType>> GetAllEntryTypesAsync();

    Task<AuthenticationResponse> CreateEntryTypeAsync(EntryType entryType);

    Task<AuthenticationResponse> UpdateEntryTypeAsync(EntryType entryType);

    Task<AuthenticationResponse> DeleteEntryTypeAsync(int entryTypeId);

    Task<AuthenticationResponse> CreateAgeGroupAsync(AgeGroup ageGroup);

    Task<AuthenticationResponse> UpdateAgeGroupAsync(AgeGroup ageGroup);

    Task<AuthenticationResponse> DeleteAgeGroupAsync(int ageGroupId);

    
    Task<List<UserDto>> GetAllUsersAsync();

    
    Task<AuthenticationResponse> DeleteUserAsync(int userId);
}
