namespace DanceWaves.Application.Dtos;

/// <summary>
/// DTO para Level
/// </summary>
public class LevelDto
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
}
