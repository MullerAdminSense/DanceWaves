namespace DanceWaves.Application.Dtos;

/// <summary>
/// DTO para EntryType
/// </summary>
public class EntryTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int NumberOfDancers { get; set; }
}
