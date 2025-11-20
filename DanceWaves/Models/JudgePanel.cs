using System.ComponentModel.DataAnnotations;

namespace DanceWaves.Models
{
    public class JudgePanel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CompetitionCategoryId { get; set; }
    }
}
