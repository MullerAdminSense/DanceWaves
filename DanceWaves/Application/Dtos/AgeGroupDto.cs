namespace DanceWaves.Application.Dtos;

/// <summary>
/// DTO para AgeGroup
/// </summary>
public class AgeGroupDto
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
}
