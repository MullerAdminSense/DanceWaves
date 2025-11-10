using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DanceWaves.Models
{
    public enum EntryStatus
    {
        Pending,
        Accepted,
        NotAccepted
    }

    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed
    }

    public class Entry
    {
        public int Id { get; set; }

        public int CompetitionCategoryId { get; set; }
        public CompetitionCategory CompetitionCategory { get; set; }

        public int StartNumber { get; set; }

        public DateTime? StartTime { get; set; }

        public int? SchoolId { get; set; }
        public DanceSchool School { get; set; }

        public EntryStatus? Status { get; set; }

        public PaymentStatus? PaymentStatus { get; set; }

        [MaxLength(500)]
        public string Song { get; set; }

        // Duration in seconds for simplicity
        public int? DurationSeconds { get; set; }

        // Navigation
        public ICollection<EntryMember> Members { get; set; }
        public ICollection<Score> Scores { get; set; }
    }
}
