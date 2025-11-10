using System.ComponentModel.DataAnnotations;

namespace DanceWaves.Models
{
    public class Level
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
    }
}
