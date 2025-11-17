using DanceWaves.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceWaves.Data
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    // Ensure database is created
                    await dbContext.Database.EnsureCreatedAsync();
                    // Seed user role permissions
                    await SeedUserRolePermissionsAsync(dbContext);
                    // Seed test Franchises and DanceSchools
                    await SeedTestFranchisesAndDanceSchoolsAsync(dbContext);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseInitializer");
                    logger.LogError(ex, "An error occurred while seeding the database");
                    throw;
                }
            }
        }

        private static async Task SeedTestFranchisesAndDanceSchoolsAsync(ApplicationDbContext dbContext)
        {
            if (!await dbContext.Franchises.AnyAsync())
            {
                var franchises = new List<Franchise>
                {
                    new() { LegalName = "Franchise USA", CountryId = 1, IsPartOfEU = false, ContactEmail = "usa@franchise.com" },
                    new() { LegalName = "Franchise NL", CountryId = 2, IsPartOfEU = true, ContactEmail = "nl@franchise.com" }
                };
                await dbContext.Franchises.AddRangeAsync(franchises);
            }
            if (!await dbContext.DanceSchools.AnyAsync())
            {
                var danceSchools = new List<DanceSchool>
                {
                    new() { LegalName = "DanceSchool FR", CountryId = 3, Email = "fr@school.com" },
                    new() { LegalName = "DanceSchool DE", CountryId = 4, Email = "de@school.com" }
                };
                await dbContext.DanceSchools.AddRangeAsync(danceSchools);
            }
            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedUserRolePermissionsAsync(ApplicationDbContext dbContext)
        {
            // Check if data already exists
            if (await dbContext.UserRolePermissions.AnyAsync())
            {
                return;
            }

            var rolePermissions = new List<UserRolePermission>
            {
                        new() {
                            Name = "GlobalAdmin",
                            Description = "Sees everything"
                        },
                new() {
                    Name = "FranchiseAdmin",
                    Description = "Manages all connected users, contests, results"
                },
                new() {
                    Name = "User",
                    Description = "Sees his own data and joined contests"
                },
                new() {
                    Name = "Judge",
                    Description = "Can put results in the system per connected contest"
                },
                new() {
                    Name = "DanceSchool",
                    Description = "Manages dance school data and members"
                },
                new() {
                    Name = "Dancer",
                    Description = "Participates in contests and events"
                }
            };

            await dbContext.UserRolePermissions.AddRangeAsync(rolePermissions);
            await dbContext.SaveChangesAsync();
        }
    }
}
