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
    }
}
