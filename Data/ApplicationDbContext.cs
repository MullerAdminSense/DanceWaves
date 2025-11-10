using Microsoft.EntityFrameworkCore;
using DanceWaves.Models;

namespace DanceWaves.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<Franchise> Franchises { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<DanceSchool> DanceSchools { get; set; }
		public DbSet<Competition> Competitions { get; set; }
		public DbSet<CompetitionCategory> CompetitionCategories { get; set; }
		public DbSet<JudgePanel> JudgePanels { get; set; }
		public DbSet<Score> Scores { get; set; }
		public DbSet<Entry> Entries { get; set; }
		public DbSet<EntryMember> EntryMembers { get; set; }
		public DbSet<Style> Styles { get; set; }
		public DbSet<AgeGroup> AgeGroups { get; set; }
		public DbSet<Level> Levels { get; set; }
		public DbSet<EntryType> EntryTypes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Basic configuration
			modelBuilder.Entity<Franchise>(b =>
			{
				b.HasKey(f => f.Id);
				b.Property(f => f.LegalName).HasMaxLength(200).IsRequired();
				b.HasIndex(f => f.VatNumber).IsUnique(false);
			});

			modelBuilder.Entity<User>(b =>
			{
				b.HasKey(u => u.Id);
				b.Property(u => u.Email).HasMaxLength(200).IsRequired();
				b.HasOne(u => u.DanceSchool)
					.WithMany(s => s.Users)
					.HasForeignKey(u => u.DanceSchoolId)
					.OnDelete(DeleteBehavior.SetNull);
				b.HasOne(u => u.DefaultFranchise)
					.WithMany(f => f.Users)
					.HasForeignKey(u => u.DefaultFranchiseId)
					.OnDelete(DeleteBehavior.SetNull);
			});

			modelBuilder.Entity<DanceSchool>(b =>
			{
				b.HasKey(s => s.Id);
				b.Property(s => s.LegalName).HasMaxLength(200).IsRequired();
			});

			modelBuilder.Entity<Competition>(b =>
			{
				b.HasKey(c => c.Id);
				b.Property(c => c.Name).HasMaxLength(300).IsRequired();
			});

			modelBuilder.Entity<CompetitionCategory>(b =>
			{
				b.HasKey(cc => cc.Id);
				b.HasOne(cc => cc.Competition).WithMany(c => c.Categories).HasForeignKey(cc => cc.CompetitionId);
				b.HasOne(cc => cc.Style).WithMany().HasForeignKey(cc => cc.StyleId);
				b.HasOne(cc => cc.AgeGroup).WithMany().HasForeignKey(cc => cc.AgeGroupId);
				b.HasOne(cc => cc.Level).WithMany().HasForeignKey(cc => cc.LevelId);
			});

			modelBuilder.Entity<JudgePanel>(b =>
			{
				b.HasKey(j => j.Id);
				b.HasOne(j => j.User).WithMany().HasForeignKey(j => j.UserId);
				b.HasOne(j => j.CompetitionCategory).WithMany(cc => cc.JudgePanels).HasForeignKey(j => j.CompetitionCategoryId);
			});

			modelBuilder.Entity<Entry>(b =>
			{
				b.HasKey(e => e.Id);
				b.HasOne(e => e.CompetitionCategory).WithMany(cc => cc.Entries).HasForeignKey(e => e.CompetitionCategoryId);
				b.HasOne(e => e.School).WithMany(s => s.Entries).HasForeignKey(e => e.SchoolId).OnDelete(DeleteBehavior.SetNull);
			});

			modelBuilder.Entity<EntryMember>(b =>
			{
				b.HasKey(em => em.Id);
				b.HasOne(em => em.Entry).WithMany(e => e.Members).HasForeignKey(em => em.EntryId);
				b.HasOne(em => em.User).WithMany().HasForeignKey(em => em.UserId);
			});

			modelBuilder.Entity<Score>(b =>
			{
				b.HasKey(s => s.Id);
				b.HasOne(s => s.Judge).WithMany().HasForeignKey(s => s.JudgeUserId);
				b.HasOne(s => s.Entry).WithMany(e => e.Scores).HasForeignKey(s => s.EntryId);
			});
		}
	}
}
