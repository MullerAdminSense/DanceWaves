using System.ComponentModel.DataAnnotations;

namespace DanceWaves.Models
{
    public class UserRolePermission
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
