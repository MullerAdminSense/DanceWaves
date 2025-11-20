using DanceWaves.Models;

namespace DanceWaves.Application.Dtos;

/// <summary>
/// DTO para EntryMember
/// </summary>
public class EntryMemberDto
{
    public int Id { get; set; }
    public int EntryId { get; set; }
    public int UserId { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
}
