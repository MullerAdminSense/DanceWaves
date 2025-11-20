using DanceWaves.Models;

namespace DanceWaves.Application.Dtos;

/// <summary>
/// Simplified DTO for Entry (no navigation props)
/// </summary>
public class EntrySimpleDto
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
}
