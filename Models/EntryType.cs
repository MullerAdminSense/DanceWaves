using System.ComponentModel.DataAnnotations;

namespace DanceWaves.Models
{
    public class EntryType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        
        public int NumberOfDancers { get; set; }
    }
}