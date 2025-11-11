using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DanceWaves.Models
{
    public class CompetitionCategory
    {
        public int Id { get; set; }

        public int CompetitionId { get; set; }
    public Competition? Competition { get; set; }

        public int StyleId { get; set; }
    public Style? Style { get; set; }

        public int AgeGroupId { get; set; }
    public AgeGroup? AgeGroup { get; set; }

        public int LevelId { get; set; }
    public Level? Level { get; set; }

        public int MinTeamSize { get; set; }
        public int MaxTeamSize { get; set; }

        public bool GenderMix { get; set; }

        // stored as seconds or formatted string; using TimeSpan for clarity
        public int MaxMusicLengthSeconds { get; set; }

        public decimal FeeAmount { get; set; }

        public int Capacity { get; set; }

        // Navigation
    public ICollection<JudgePanel>? JudgePanels { get; set; }
    public ICollection<Entry>? Entries { get; set; }
    }
}
