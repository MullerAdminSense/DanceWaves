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

                    // Seed countries
                    await SeedCountriesAsync(dbContext);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseInitializer");
                    logger.LogError(ex, "An error occurred while seeding the database");
                    throw;
                }
            }
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
                new UserRolePermission
                {
                    Name = "SuperAdmin",
                    Description = "Sees everything"
                },
                new UserRolePermission
                {
                    Name = "FranchiseAdmin",
                    Description = "Manages all connected users, contests, results"
                },
                new UserRolePermission
                {
                    Name = "User",
                    Description = "Sees his own data and joined contests"
                },
                new UserRolePermission
                {
                    Name = "Jury",
                    Description = "Can put results in the system per connected contest"
                }
            };

            await dbContext.UserRolePermissions.AddRangeAsync(rolePermissions);
            await dbContext.SaveChangesAsync();
        }
        private static async Task SeedCountriesAsync(ApplicationDbContext dbContext)
        {
            if (await dbContext.Countries.AnyAsync())
                return;

            var countries = new List<Country>
            {
                new Country { Name = "United States" },
                new Country { Name = "Canada" },
                new Country { Name = "Brazil" },
                new Country { Name = "United Kingdom" },
                new Country { Name = "France" },
                new Country { Name = "Germany" },
                new Country { Name = "Italy" },
                new Country { Name = "Spain" },
                new Country { Name = "Netherlands" },
                new Country { Name = "Belgium" },
                new Country { Name = "Portugal" },
                new Country { Name = "Argentina" },
                new Country { Name = "Mexico" },
                new Country { Name = "Japan" },
                new Country { Name = "China" },
                new Country { Name = "South Korea" },
                new Country { Name = "Australia" },
                new Country { Name = "South Africa" },
                new Country { Name = "Russia" },
                new Country { Name = "India" }
            };
            await dbContext.Countries.AddRangeAsync(countries);
            await dbContext.SaveChangesAsync();
        }
    }
}
