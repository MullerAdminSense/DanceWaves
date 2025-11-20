namespace DanceWaves.Application.Dtos;

/// <summary>
/// DTO para Score
/// </summary>
public class ScoreDto
{
    public int Id { get; set; }
    public int JudgeUserId { get; set; }
    public int EntryId { get; set; }
    public int RawScore { get; set; }
    public int Penalty { get; set; }
    public int? Note { get; set; }
    public DateTime SubmittedDate { get; set; }
}
