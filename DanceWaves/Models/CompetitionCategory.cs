using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DanceWaves.Models
{
    public class CompetitionCategory
    {
        public int Id { get; set; }

        public int CompetitionId { get; set; }

        public int StyleId { get; set; }

        public int AgeGroupId { get; set; }

        public int LevelId { get; set; }

        public int MinTeamSize { get; set; }
        public int MaxTeamSize { get; set; }

        public bool GenderMix { get; set; }

        // stored as seconds or formatted string; using TimeSpan for clarity
        public int MaxMusicLengthSeconds { get; set; }

        public decimal FeeAmount { get; set; }

        public int Capacity { get; set; }
    }
}
