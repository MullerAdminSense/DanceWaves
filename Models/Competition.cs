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
        public string Name { get; set; }

        public string Location { get; set; }
        public string Venue { get; set; }

        
        [MaxLength(100)]
        public string Status { get; set; }

        
        public string GeoPoints { get; set; }

        public int MaxContestants { get; set; }

        public DateTime? RegistrationsOpenForMembers { get; set; }
        public DateTime? RegistrationsOpenForEveryone { get; set; }
        public DateTime? CheckInUntil { get; set; }

        
        public ICollection<CompetitionCategory> Categories { get; set; }
    }
}