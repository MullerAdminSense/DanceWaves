using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DanceWaves.Models
{
	public class DanceSchool
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(200)]
		public string LegalName { get; set; }

		[MaxLength(300)]
		public string Address { get; set; }

		[MaxLength(100)]
		public string City { get; set; }

		[MaxLength(30)]
		public string Zip { get; set; }

		[MaxLength(100)]
		public string Province { get; set; }

		[MaxLength(100)]
		public string Country { get; set; }

		[MaxLength(100)]
		public string VatNumber { get; set; }

		public bool IsPartOfEU { get; set; }

		[EmailAddress]
		[MaxLength(200)]
		public string Email { get; set; }

		public int? DefaultFranchiseId { get; set; }
		public Franchise DefaultFranchise { get; set; }

        
		public ICollection<User> Users { get; set; }
		public ICollection<Entry> Entries { get; set; }
	}
}
