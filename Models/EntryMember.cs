using System.ComponentModel.DataAnnotations;

namespace DanceWaves.Models
{
	public class EntryMember
	{
		public int Id { get; set; }

		public int EntryId { get; set; }
		public Entry Entry { get; set; }

		public int UserId { get; set; }
		public User User { get; set; }

		public PaymentStatus PaymentStatus { get; set; }
	}
}
