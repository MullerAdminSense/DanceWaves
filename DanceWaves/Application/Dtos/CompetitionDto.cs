using DanceWaves.Models;

namespace DanceWaves.Application.Dtos;

/// <summary>
/// DTO para Competition
/// </summary>
public class CompetitionDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    public string? Venue { get; set; }
    public CompetitionStatus Status { get; set; }
    public string? GeoPoints { get; set; }
    public int MaxContestants { get; set; }
    public DateTime? RegistrationsOpenForMembers { get; set; }
    public DateTime? RegistrationsOpenForEveryone { get; set; }
    public DateTime? CheckInUntil { get; set; }
}
