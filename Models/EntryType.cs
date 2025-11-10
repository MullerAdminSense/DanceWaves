using System.ComponentModel.DataAnnotations;

namespace DanceWaves.Models
{
    public class EntryType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        // e.g. SOLO, DUO, TEAM - number of dancers expected
        public int NumberOfDancers { get; set; }
    }
}