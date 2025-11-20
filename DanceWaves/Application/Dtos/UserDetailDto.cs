namespace DanceWaves.Application.Dtos;

/// <summary>
/// DTO para User com navegação opcional
/// </summary>
public class UserDetailDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Zip { get; set; }
    public string? Province { get; set; }
    public int CountryId { get; set; }
    public string Email { get; set; } = null!;
    public int? DanceSchoolId { get; set; }
    public string? Phone { get; set; }
    public int? DefaultFranchiseId { get; set; }
    public int? AgeGroupId { get; set; }
    public int RolePermissionId { get; set; }
    
    // Navigation properties (apenas quando necessário via Include)
    public DanceSchoolDto? DanceSchool { get; set; }
    public FranchiseDto? DefaultFranchise { get; set; }
    public AgeGroupDto? AgeGroup { get; set; }
    public UserRolePermissionDto? RolePermission { get; set; }
}
