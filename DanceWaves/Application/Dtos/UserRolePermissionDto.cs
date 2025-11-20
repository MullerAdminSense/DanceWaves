namespace DanceWaves.Application.Dtos;

/// <summary>
/// DTO para UserRolePermission
/// </summary>
public class UserRolePermissionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
