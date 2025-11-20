namespace DanceWaves.Application.Dtos;

/// <summary>
/// DTO para DanceSchool com navegação opcional
/// </summary>
public class DanceSchoolDto
{
    public int Id { get; set; }
    public string LegalName { get; set; } = null!;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Zip { get; set; }
    public string? Province { get; set; }
    public string? VatNumber { get; set; }
    public bool IsPartOfEU { get; set; }
    public string? Email { get; set; }
    public int? DefaultFranchiseId { get; set; }
    public int CountryId { get; set; }
    
    // Navigation properties (apenas quando necessário)
    public FranchiseDto? DefaultFranchise { get; set; }
}
