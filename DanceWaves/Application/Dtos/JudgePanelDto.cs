namespace DanceWaves.Application.Dtos;

/// <summary>
/// DTO para JudgePanel
/// </summary>
public class JudgePanelDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CompetitionCategoryId { get; set; }
    
    // Navigation opcional
    public UserSimpleDto? User { get; set; }
    public CompetitionCategoryDto? CompetitionCategory { get; set; }
}
