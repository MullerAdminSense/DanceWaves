namespace DanceWaves.Application.Dtos;

/// <summary>
/// DTO para CompetitionCategory
/// </summary>
public class CompetitionCategoryDto
{
    public int Id { get; set; }
    public int CompetitionId { get; set; }
    public int StyleId { get; set; }
    public int AgeGroupId { get; set; }
    public int LevelId { get; set; }
    public int MinTeamSize { get; set; }
    public int MaxTeamSize { get; set; }
    public bool GenderMix { get; set; }
    public int MaxMusicLengthSeconds { get; set; }
    public decimal FeeAmount { get; set; }
    public int Capacity { get; set; }
    
    // Navigation opcional
    public CompetitionDto? Competition { get; set; }
    public StyleDto? Style { get; set; }
    public AgeGroupDto? AgeGroup { get; set; }
    public LevelDto? Level { get; set; }
}
