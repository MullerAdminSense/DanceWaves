using DanceWaves.Models;
using Microsoft.EntityFrameworkCore;

namespace DanceWaves.Data
{
    public static class UserRolePermissionSeeder
    {
        public static void SeedUserRolePermissions(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRolePermission>().HasData(
                new UserRolePermission
                {
                    Id = 1,
                    Name = "SuperAdmin",
                    Description = "Sees everything"
                },
                new UserRolePermission
                {
                    Id = 2,
                    Name = "FranchiseAdmin",
                    Description = "Manages all connected users, contests, results"
                },
                new UserRolePermission
                {
                    Id = 3,
                    Name = "User",
                    Description = "Sees his own data and joined contests"
                },
                new UserRolePermission
                {
                    Id = 4,
                    Name = "Jury",
                    Description = "Can put results in the system per connected contest"
                }
            );
        }
    }
}
