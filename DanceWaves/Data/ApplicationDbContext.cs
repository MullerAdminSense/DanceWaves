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
                // Configuração de relacionamentos usando apenas FKs (sem navigation properties)
                b.HasOne<DanceSchool>()
                    .WithMany()
                    .HasForeignKey(u => u.DanceSchoolId)
                    .OnDelete(DeleteBehavior.SetNull);
                b.HasOne<Franchise>()
                    .WithMany()
                    .HasForeignKey(u => u.DefaultFranchiseId)
                    .OnDelete(DeleteBehavior.SetNull);
                b.HasOne<UserRolePermission>()
                    .WithMany()
                    .HasForeignKey(u => u.RolePermissionId)
                    .OnDelete(DeleteBehavior.Restrict);
                b.HasOne<AgeGroup>()
                    .WithMany()
                    .HasForeignKey(u => u.AgeGroupId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<DanceSchool>(b =>
            {
                b.HasKey(s => s.Id);
                b.Property(s => s.Id).ValueGeneratedOnAdd();
                b.Property(s => s.LegalName).HasMaxLength(200).IsRequired();
                // Configuração de relacionamento usando apenas FK
                b.HasOne<Franchise>()
                    .WithMany()
                    .HasForeignKey(s => s.DefaultFranchiseId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Competition>(b =>
            {
                b.HasKey(c => c.Id);
                b.Property(c => c.Id).ValueGeneratedOnAdd();
                b.Property(c => c.Name).HasMaxLength(200).IsRequired();
                b.Property(c => c.Status)
                    .HasConversion<int>()
                    .IsRequired();
            });

            modelBuilder.Entity<CompetitionCategory>(b =>
            {
                b.HasKey(cc => cc.Id);
                b.Property(cc => cc.Id).ValueGeneratedOnAdd();
                // Configuração de relacionamentos usando apenas FKs
                b.HasOne<Competition>()
                    .WithMany()
                    .HasForeignKey(cc => cc.CompetitionId)
                    .OnDelete(DeleteBehavior.Cascade);
                b.HasOne<Style>()
                    .WithMany()
                    .HasForeignKey(cc => cc.StyleId)
                    .OnDelete(DeleteBehavior.Restrict);
                b.HasOne<AgeGroup>()
                    .WithMany()
                    .HasForeignKey(cc => cc.AgeGroupId)
                    .OnDelete(DeleteBehavior.Restrict);
                b.HasOne<Level>()
                    .WithMany()
                    .HasForeignKey(cc => cc.LevelId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<JudgePanel>(b =>
            {
                b.HasKey(j => j.Id);
                b.Property(j => j.Id).ValueGeneratedOnAdd();
                // Configuração de relacionamentos usando apenas FKs
                b.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(j => j.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                b.HasOne<CompetitionCategory>()
                    .WithMany()
                    .HasForeignKey(j => j.CompetitionCategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Entry>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.Id).ValueGeneratedOnAdd();
                // Configuração de relacionamentos usando apenas FKs
                b.HasOne<CompetitionCategory>()
                    .WithMany()
                    .HasForeignKey(e => e.CompetitionCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
                b.HasOne<DanceSchool>()
                    .WithMany()
                    .HasForeignKey(e => e.SchoolId)
                    .OnDelete(DeleteBehavior.SetNull);
                b.Property(e => e.Status)
                    .HasConversion<int>();
                b.Property(e => e.PaymentStatus)
                    .HasConversion<int>();
            });

            modelBuilder.Entity<EntryMember>(b =>
            {
                b.HasKey(em => em.Id);
                b.Property(em => em.Id).ValueGeneratedOnAdd();
                // Configuração de relacionamentos usando apenas FKs
                b.HasOne<Entry>()
                    .WithMany()
                    .HasForeignKey(em => em.EntryId)
                    .OnDelete(DeleteBehavior.Cascade);
                b.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(em => em.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Score>(b =>
            {
                b.HasKey(s => s.Id);
                b.Property(s => s.Id).ValueGeneratedOnAdd();
                // Configuração de relacionamentos usando apenas FKs
                b.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(s => s.JudgeUserId)
                    .OnDelete(DeleteBehavior.Restrict);
                b.HasOne<Entry>()
                    .WithMany()
                    .HasForeignKey(s => s.EntryId)
                    .OnDelete(DeleteBehavior.Cascade);
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
