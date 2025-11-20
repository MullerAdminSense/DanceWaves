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
    
    Task<List<DanceSchoolDto>> GetAllDanceSchoolsAsync();

    
    Task<List<FranchiseDto>> GetFranchisesByDanceSchoolAsync(int danceSchoolId);

    
    Task<List<FranchiseDto>> GetAllFranchisesAsync();

    
    Task<List<UserRolePermissionDto>> GetAllRolePermissionsAsync();

    Task<AuthenticationResponse> CreateRolePermissionAsync(UserRolePermissionDto rolePermission);

    Task<AuthenticationResponse> UpdateRolePermissionAsync(UserRolePermissionDto rolePermission);

    Task<AuthenticationResponse> DeleteRolePermissionAsync(int rolePermissionId);

    
    Task<List<AgeGroupDto>> GetAllAgeGroupsAsync();

    Task<List<StyleDto>> GetAllStylesAsync();

    Task<AuthenticationResponse> CreateStyleAsync(StyleDto style);

    Task<AuthenticationResponse> UpdateStyleAsync(StyleDto style);

    Task<AuthenticationResponse> DeleteStyleAsync(int styleId);

    Task<List<EntryTypeDto>> GetAllEntryTypesAsync();

    Task<AuthenticationResponse> CreateEntryTypeAsync(EntryTypeDto entryType);

    Task<AuthenticationResponse> UpdateEntryTypeAsync(EntryTypeDto entryType);

    Task<AuthenticationResponse> DeleteEntryTypeAsync(int entryTypeId);

    Task<AuthenticationResponse> CreateAgeGroupAsync(AgeGroupDto ageGroup);

    Task<AuthenticationResponse> UpdateAgeGroupAsync(AgeGroupDto ageGroup);

    Task<AuthenticationResponse> DeleteAgeGroupAsync(int ageGroupId);

    
    Task<List<UserDto>> GetAllUsersAsync();

    
    Task<AuthenticationResponse> DeleteUserAsync(int userId);
}
