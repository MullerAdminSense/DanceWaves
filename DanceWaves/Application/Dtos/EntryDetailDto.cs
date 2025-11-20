using DanceWaves.Models;

namespace DanceWaves.Application.Dtos;

/// <summary>
/// Entry DTO with optional navigation properties
/// </summary>
public class EntryDetailDto
{
    public int Id { get; set; }
    public int CompetitionCategoryId { get; set; }
    public int StartNumber { get; set; }
    public DateTime? StartTime { get; set; }
    public int? SchoolId { get; set; }
    public EntryStatus? Status { get; set; }
    public PaymentStatus? PaymentStatus { get; set; }
    public string? Song { get; set; }
    public int? DurationSeconds { get; set; }
    
    // Navigation properties (only when needed via Include)
    public CompetitionCategoryDto? CompetitionCategory { get; set; }
    public DanceSchoolDto? School { get; set; }
    public ICollection<EntryMemberDto>? Members { get; set; }
    public ICollection<ScoreDto>? Scores { get; set; }
}
