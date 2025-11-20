using System;
using System.ComponentModel.DataAnnotations;

namespace DanceWaves.Models
{
    public class Score
    {
        public int Id { get; set; }

        public int JudgeUserId { get; set; }

        public int EntryId { get; set; }

        public int RawScore { get; set; }

        public int Penalty { get; set; }

        public int? Note { get; set; }

        public DateTime SubmittedDate { get; set; }
    }
}
