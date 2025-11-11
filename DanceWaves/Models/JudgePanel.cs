using System.ComponentModel.DataAnnotations;

namespace DanceWaves.Models
{
    public class JudgePanel
    {
        public int Id { get; set; }

        public int UserId { get; set; }
    public User? User { get; set; }

        public int CompetitionCategoryId { get; set; }
    public CompetitionCategory? CompetitionCategory { get; set; }
    }
}
