namespace DanceWaves.Application.Dtos;

/// <summary>
/// DTO para Franchise
/// </summary>
public class FranchiseDto
{
    public int Id { get; set; }
    public string LegalName { get; set; } = null!;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Zip { get; set; }
    public string? Province { get; set; }
    public string? VatNumber { get; set; }
    public bool IsPartOfEU { get; set; }
    public string? ContactEmail { get; set; }
    public string? SystemEmail { get; set; }
    public int CountryId { get; set; }
}
