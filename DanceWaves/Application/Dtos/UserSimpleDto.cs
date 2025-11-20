namespace DanceWaves.Application.Dtos;

/// <summary>
/// DTO simplificado para User (sem navegação)
/// </summary>
public class UserSimpleDto
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
}
