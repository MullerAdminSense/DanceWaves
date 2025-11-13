using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DanceWaves.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        // Navigation
        public ICollection<Franchise> Franchises { get; set; } = new List<Franchise>();
        public ICollection<DanceSchool> DanceSchools { get; set; } = new List<DanceSchool>();
    }
}
