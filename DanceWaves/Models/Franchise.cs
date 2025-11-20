using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DanceWaves.Models
{
    public class Franchise
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string LegalName { get; set; } = null!;

        [MaxLength(300)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(10)]
        public string? Zip { get; set; }

        [MaxLength(100)]
        public string? Province { get; set; }

        [Display(Name = "Tax registration Number")]
        [MaxLength(100)]
        public string? VatNumber { get; set; }

        public bool IsPartOfEU { get; set; }

        [EmailAddress]
        [MaxLength(200)]
        public string? ContactEmail { get; set; }

        [EmailAddress]
        [MaxLength(200)]
        public string? SystemEmail { get; set; }

        [Required]
        public int CountryId { get; set; }
    }
}
