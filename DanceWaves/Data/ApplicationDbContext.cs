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
        public DbSet<UserRolePermission> UserRolePermissions { get; set; }
    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.SeedUserRolePermissions();

            
            modelBuilder.Entity<Franchise>(b =>
            {
                b.HasKey(f => f.Id);
                b.Property(f => f.Id).ValueGeneratedOnAdd();
                b.Property(f => f.LegalName).HasMaxLength(100).IsRequired();
                b.HasIndex(f => f.VatNumber).IsUnique(false);
                
            });

            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id);
                b.Property(u => u.Id).ValueGeneratedOnAdd();
                b.Property(u => u.Email).HasMaxLength(200).IsRequired();
                b.HasOne(u => u.DanceSchool)
                    .WithMany(s => s.Users)
                    .HasForeignKey(u => u.DanceSchoolId)
                    .OnDelete(DeleteBehavior.SetNull);
                b.HasOne(u => u.DefaultFranchise)
                    .WithMany(f => f.Users)
                    .HasForeignKey(u => u.DefaultFranchiseId)
                    .OnDelete(DeleteBehavior.SetNull);
                b.HasOne(u => u.RolePermission)
                    .WithMany()
                    .HasForeignKey(u => u.RolePermissionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<DanceSchool>(b =>
            {
                b.HasKey(s => s.Id);
                b.Property(s => s.Id).ValueGeneratedOnAdd();
                b.Property(s => s.LegalName).HasMaxLength(200).IsRequired();
                
            });

            modelBuilder.Entity<Competition>(b =>
            {
                b.HasKey(c => c.Id);
                b.Property(c => c.Id).ValueGeneratedOnAdd();
                b.Property(c => c.Name).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<CompetitionCategory>(b =>
            {
                b.HasKey(cc => cc.Id);
                b.Property(cc => cc.Id).ValueGeneratedOnAdd();
                b.HasOne(cc => cc.Competition).WithMany(c => c.Categories).HasForeignKey(cc => cc.CompetitionId);
                b.HasOne(cc => cc.Style).WithMany().HasForeignKey(cc => cc.StyleId);
                b.HasOne(cc => cc.AgeGroup).WithMany().HasForeignKey(cc => cc.AgeGroupId);
                b.HasOne(cc => cc.Level).WithMany().HasForeignKey(cc => cc.LevelId);
            });

            modelBuilder.Entity<JudgePanel>(b =>
            {
                b.HasKey(j => j.Id);
                b.Property(j => j.Id).ValueGeneratedOnAdd();
                b.HasOne(j => j.User).WithMany().HasForeignKey(j => j.UserId);
                b.HasOne(j => j.CompetitionCategory).WithMany(cc => cc.JudgePanels).HasForeignKey(j => j.CompetitionCategoryId);
            });

            modelBuilder.Entity<Entry>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.Id).ValueGeneratedOnAdd();
                b.HasOne(e => e.CompetitionCategory).WithMany(cc => cc.Entries).HasForeignKey(e => e.CompetitionCategoryId);
                b.HasOne(e => e.School).WithMany(s => s.Entries).HasForeignKey(e => e.SchoolId).OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<EntryMember>(b =>
            {
                b.HasKey(em => em.Id);
                b.Property(em => em.Id).ValueGeneratedOnAdd();
                b.HasOne(em => em.Entry).WithMany(e => e.Members).HasForeignKey(em => em.EntryId);
                b.HasOne(em => em.User).WithMany().HasForeignKey(em => em.UserId);
            });

            modelBuilder.Entity<Score>(b =>
            {
                b.HasKey(s => s.Id);
                b.Property(s => s.Id).ValueGeneratedOnAdd();
                b.HasOne(s => s.Judge).WithMany().HasForeignKey(s => s.JudgeUserId);
                b.HasOne(s => s.Entry).WithMany(e => e.Scores).HasForeignKey(s => s.EntryId);
            });

            modelBuilder.Entity<UserRolePermission>(b =>
            {
                b.HasKey(r => r.Id);
                b.Property(r => r.Id).ValueGeneratedOnAdd();
                b.Property(r => r.Name).HasMaxLength(50).IsRequired();
                b.Property(r => r.Description).HasMaxLength(500);
                b.HasIndex(r => r.Name).IsUnique();
            });

            modelBuilder.Entity<Style>(b =>
            {
                b.HasKey(s => s.Id);
                b.Property(s => s.Id).ValueGeneratedOnAdd();
                b.Property(s => s.Code).HasMaxLength(50).IsRequired();
                b.Property(s => s.Name).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<AgeGroup>(b =>
            {
                b.HasKey(ag => ag.Id);
                b.Property(ag => ag.Id).ValueGeneratedOnAdd();
                b.Property(ag => ag.Code).HasMaxLength(50).IsRequired();
                b.Property(ag => ag.Name).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<Level>(b =>
            {
                b.HasKey(l => l.Id);
                b.Property(l => l.Id).ValueGeneratedOnAdd();
                b.Property(l => l.Code).HasMaxLength(50).IsRequired();
                b.Property(l => l.Name).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<EntryType>(b =>
            {
                b.HasKey(et => et.Id);
                b.Property(et => et.Id).ValueGeneratedOnAdd();
                b.Property(et => et.Name).HasMaxLength(100).IsRequired();
            });

            
        }
    }
}
