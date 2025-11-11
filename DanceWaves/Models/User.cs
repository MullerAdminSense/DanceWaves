using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceWaves.Models
{
    public class User
    {
        public int Id { get; set; }

    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    [MaxLength(300)]
    public string? Address { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(10)]
    public string? Zip { get; set; }

    [MaxLength(100)]
    public string? Province { get; set; }

    [MaxLength(100)]
    public string? Country { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = null!;

        public int? DanceSchoolId { get; set; }
    public DanceSchool? DanceSchool { get; set; }

    [Phone]
    [MaxLength(50)]
    public string? Phone { get; set; }

        public int? DefaultFranchiseId { get; set; }
    public Franchise? DefaultFranchise { get; set; }

        public int? AgeGroupId { get; set; }
    public AgeGroup? AgeGroup { get; set; }

        public int RolePermissionId { get; set; }
    public UserRolePermission? RolePermission { get; set; }
    [MaxLength(300)]
    public string? Password { get; set; }
    }
}
