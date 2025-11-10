using System;
using System.ComponentModel.DataAnnotations;

namespace DanceWaves.Models
{
	public class Score
	{
		public int Id { get; set; }

		public int JudgeUserId { get; set; }
		public User Judge { get; set; }

		public int EntryId { get; set; }
		public Entry Entry { get; set; }

		public decimal RawScore { get; set; }

		public decimal Penalty { get; set; }

		[MaxLength(1000)]
		public string Note { get; set; }

		public DateTime SubmittedDate { get; set; }
	}
}
