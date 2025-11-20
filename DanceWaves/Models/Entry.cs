using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DanceWaves.Models
{
    public enum EntryStatus
    {
        Pending = 1,
        Accepted = 2,
        NotAccepted = 3
    }

    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2,
        Failed = 3
    }

    public class Entry
    {
        public int Id { get; set; }

        public int CompetitionCategoryId { get; set; }

        public int StartNumber { get; set; }

        public DateTime? StartTime { get; set; }

        public int? SchoolId { get; set; }

        public EntryStatus? Status { get; set; }

        public PaymentStatus? PaymentStatus { get; set; }

        [MaxLength(500)]
        public string? Song { get; set; }

        // Duration in seconds for simplicity
        public int? DurationSeconds { get; set; }
    }
}
