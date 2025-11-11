using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DanceWaves.Models
{
    public class Competition
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
    public string? Name { get; set; }

    public string? Location { get; set; }
    public string? Venue { get; set; }

        public CompetitionStatus Status { get; set; }

        // Geo points for map display (could be GeoJSON or lat/lon)
    public string? GeoPoints { get; set; }

        public int MaxContestants { get; set; }

        public DateTime? RegistrationsOpenForMembers { get; set; }
        public DateTime? RegistrationsOpenForEveryone { get; set; }
        public DateTime? CheckInUntil { get; set; }

        // Navigation
    public ICollection<CompetitionCategory>? Categories { get; set; }
    }
}
