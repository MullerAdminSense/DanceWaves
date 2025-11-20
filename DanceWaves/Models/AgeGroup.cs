using System.ComponentModel.DataAnnotations;

namespace DanceWaves.Models
{
    public class AgeGroup
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Code { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }

        // Min and Max ages in years
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
    }
}
