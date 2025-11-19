using DanceWaves.Application.Dtos;
using DanceWaves.Application.Ports;
using DanceWaves.Data;
using DanceWaves.Models;
using DanceWaves.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq;
using System.Security.Cryptography;

namespace DanceWaves.Adapters.Persistence;

/// <summary>
/// Adaptador de autenticação
/// Implementa a porta IAuthenticationPort
/// </summary>
public class AuthenticationAdapter(ApplicationDbContext dbContext) : IAuthenticationPort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<DanceSchool>> GetAllDanceSchoolsAsync()
    {
        // Sempre busca dados atualizados do banco sem cache
        return await _dbContext.DanceSchools
            .AsNoTracking()
            .OrderBy(ds => ds.LegalName)
            .ToListAsync();
    }

    public async Task<List<Franchise>> GetAllFranchisesAsync()
    {
        // Sempre busca dados atualizados do banco sem cache
        return await _dbContext.Franchises
            .AsNoTracking()
            .OrderBy(f => f.LegalName)
            .ToListAsync();
    }

    public async Task<List<Franchise>> GetFranchisesByDanceSchoolAsync(int danceSchoolId)
    {
        // Sempre busca dados atualizados do banco sem cache
        // Busca a DanceSchool primeiro para obter informações relacionadas
        var danceSchool = await _dbContext.DanceSchools
            .AsNoTracking()
            .FirstOrDefaultAsync(ds => ds.Id == danceSchoolId);

        if (danceSchool == null)
        {
            return new List<Franchise>();
        }

        // Se a DanceSchool tem uma Franchise padrão, inclui essa Franchise na lista
        var franchises = new List<Franchise>();
        
        if (danceSchool.DefaultFranchiseId.HasValue)
        {
            var defaultFranchise = await _dbContext.Franchises
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == danceSchool.DefaultFranchiseId.Value);

            if (defaultFranchise != null)
            {
                franchises.Add(defaultFranchise);
            }
        }

        // Também busca todas as outras Franchises do mesmo país
        // para dar mais opções ao usuário
        var countryFranchises = await _dbContext.Franchises
            .AsNoTracking()
            .Where(f => f.CountryId == danceSchool.CountryId)
            .OrderBy(f => f.LegalName)
            .ToListAsync();

        // Remove duplicatas mantendo a ordem (DefaultFranchise primeiro, se houver)
        var result = franchises.Union(countryFranchises)
            .GroupBy(f => f.Id)
            .Select(g => g.First())
            .OrderByDescending(f => danceSchool.DefaultFranchiseId == f.Id) // DefaultFranchise primeiro
            .ThenBy(f => f.LegalName)
            .ToList();

        return result;
    }

    public async Task<List<UserRolePermission>> GetAllRolePermissionsAsync()
    {
        return await _dbContext.UserRolePermissions.ToListAsync();
    }

    public async Task<AuthenticationResponse> CreateRolePermissionAsync(UserRolePermission rolePermission)
    {
        try
        {
            if (rolePermission == null || string.IsNullOrWhiteSpace(rolePermission.Name))
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Role permission name is required."
                };
            }

            var exists = await _dbContext.UserRolePermissions.AnyAsync(rp => rp.Name == rolePermission.Name);
            if (exists)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "A role permission with the same name already exists."
                };
            }

            var entity = new UserRolePermission
            {
                Name = rolePermission.Name,
                Description = rolePermission.Description
            };

            _dbContext.UserRolePermissions.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Role permission created successfully."
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating role permission: {RoleName}", rolePermission?.Name);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"An error occurred while creating the role permission: {ex.Message}"
            };
        }
    }

    public async Task<AuthenticationResponse> UpdateRolePermissionAsync(UserRolePermission rolePermission)
    {
        try
        {
            if (rolePermission == null || rolePermission.Id <= 0)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Invalid role permission."
                };
            }

            var entity = await _dbContext.UserRolePermissions.FindAsync(rolePermission.Id);
            if (entity == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Role permission not found."
                };
            }

            var nameExists = await _dbContext.UserRolePermissions.AnyAsync(rp => rp.Name == rolePermission.Name && rp.Id != rolePermission.Id);
            if (nameExists)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "A role permission with this name already exists."
                };
            }

            entity.Name = rolePermission.Name;
            entity.Description = rolePermission.Description;

            await _dbContext.SaveChangesAsync();

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Role permission updated successfully."
            };
        }
        catch (DbUpdateException dbEx)
        {
            var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
            Log.Error(dbEx, "Database error updating role permission: {RoleId}", rolePermission?.Id);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error updating role permission: {innerMessage}"
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error updating role permission: {RoleId}", rolePermission?.Id);
            var innerMessage = ex.InnerException?.Message ?? ex.Message;
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error updating role permission: {innerMessage}"
            };
        }
    }

    public async Task<AuthenticationResponse> DeleteRolePermissionAsync(int rolePermissionId)
    {
        try
        {
            var entity = await _dbContext.UserRolePermissions.FindAsync(rolePermissionId);
            if (entity == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Role permission not found."
                };
            }

            var usersWithRole = await _dbContext.Users.AnyAsync(u => u.RolePermissionId == rolePermissionId);
            if (usersWithRole)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Cannot delete this role permission because it is assigned to one or more users. Please reassign or remove those users first."
                };
            }

            _dbContext.UserRolePermissions.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Role permission deleted successfully."
            };
        }
        catch (DbUpdateException dbEx)
        {
            var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
            Log.Error(dbEx, "Database error deleting role permission: {RoleId}", rolePermissionId);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error deleting role permission: {innerMessage}"
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting role permission: {RoleId}", rolePermissionId);
            var innerMessage = ex.InnerException?.Message ?? ex.Message;
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error deleting role permission: {innerMessage}"
            };
        }
    }

    public async Task<List<AgeGroup>> GetAllAgeGroupsAsync()
    {
        return await _dbContext.AgeGroups
            .AsNoTracking()
            .OrderBy(ag => ag.Name)
            .ToListAsync();
    }

    public async Task<List<Style>> GetAllStylesAsync()
    {
        return await _dbContext.Styles
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<AuthenticationResponse> CreateStyleAsync(Style style)
    {
        try
        {
            if (style == null || string.IsNullOrWhiteSpace(style.Code) || string.IsNullOrWhiteSpace(style.Name))
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Style code and name are required."
                };
            }

            var exists = await _dbContext.Styles.AnyAsync(s => s.Code == style.Code);
            if (exists)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "A style with this code already exists."
                };
            }

            _dbContext.Styles.Add(style);
            await _dbContext.SaveChangesAsync();

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Style created successfully."
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating style: {StyleCode}", style?.Code);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error creating style: {ex.Message}"
            };
        }
    }

    public async Task<AuthenticationResponse> UpdateStyleAsync(Style style)
    {
        try
        {
            var entity = await _dbContext.Styles.FindAsync(style.Id);
            if (entity == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Style not found."
                };
            }

            if (string.IsNullOrWhiteSpace(style.Code) || string.IsNullOrWhiteSpace(style.Name))
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Style code and name are required."
                };
            }

            var codeExists = await _dbContext.Styles.AnyAsync(s => s.Code == style.Code && s.Id != style.Id);
            if (codeExists)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "A style with this code already exists."
                };
            }

            entity.Code = style.Code;
            entity.Name = style.Name;

            await _dbContext.SaveChangesAsync();

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Style updated successfully."
            };
        }
        catch (DbUpdateException dbEx)
        {
            var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
            Log.Error(dbEx, "Database error updating style: {StyleId}", style.Id);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error updating style: {innerMessage}"
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error updating style: {StyleId}", style.Id);
            var innerMessage = ex.InnerException?.Message ?? ex.Message;
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error updating style: {innerMessage}"
            };
        }
    }

    public async Task<AuthenticationResponse> DeleteStyleAsync(int styleId)
    {
        try
        {
            var entity = await _dbContext.Styles.FindAsync(styleId);
            if (entity == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Style not found."
                };
            }

            var categoriesUsingStyle = await _dbContext.CompetitionCategories.AnyAsync(cc => cc.StyleId == styleId);
            if (categoriesUsingStyle)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Cannot delete this style because it is used in one or more competition categories. Please remove or update those categories first."
                };
            }

            _dbContext.Styles.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Style deleted successfully."
            };
        }
        catch (DbUpdateException dbEx)
        {
            var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
            Log.Error(dbEx, "Database error deleting style: {StyleId}", styleId);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error deleting style: {innerMessage}"
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting style: {StyleId}", styleId);
            var innerMessage = ex.InnerException?.Message ?? ex.Message;
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error deleting style: {innerMessage}"
            };
        }
    }

    public async Task<List<EntryType>> GetAllEntryTypesAsync()
    {
        return await _dbContext.EntryTypes
            .AsNoTracking()
            .OrderBy(et => et.Name)
            .ToListAsync();
    }

    public async Task<AuthenticationResponse> CreateEntryTypeAsync(EntryType entryType)
    {
        try
        {
            if (entryType == null || string.IsNullOrWhiteSpace(entryType.Name))
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Entry type name is required."
                };
            }

            var exists = await _dbContext.EntryTypes.AnyAsync(et => et.Name == entryType.Name);
            if (exists)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "An entry type with this name already exists."
                };
            }

            _dbContext.EntryTypes.Add(entryType);
            await _dbContext.SaveChangesAsync();

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Entry type created successfully."
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating entry type: {EntryTypeName}", entryType?.Name);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error creating entry type: {ex.Message}"
            };
        }
    }

    public async Task<AuthenticationResponse> UpdateEntryTypeAsync(EntryType entryType)
    {
        try
        {
            var entity = await _dbContext.EntryTypes.FindAsync(entryType.Id);
            if (entity == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Entry type not found."
                };
            }

            if (string.IsNullOrWhiteSpace(entryType.Name))
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Entry type name is required."
                };
            }

            var nameExists = await _dbContext.EntryTypes.AnyAsync(et => et.Name == entryType.Name && et.Id != entryType.Id);
            if (nameExists)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "An entry type with this name already exists."
                };
            }

            entity.Name = entryType.Name;
            entity.NumberOfDancers = entryType.NumberOfDancers;

            await _dbContext.SaveChangesAsync();

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Entry type updated successfully."
            };
        }
        catch (DbUpdateException dbEx)
        {
            var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
            Log.Error(dbEx, "Database error updating entry type: {EntryTypeId}", entryType.Id);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error updating entry type: {innerMessage}"
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error updating entry type: {EntryTypeId}", entryType.Id);
            var innerMessage = ex.InnerException?.Message ?? ex.Message;
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error updating entry type: {innerMessage}"
            };
        }
    }

    public async Task<AuthenticationResponse> DeleteEntryTypeAsync(int entryTypeId)
    {
        try
        {
            var entity = await _dbContext.EntryTypes.FindAsync(entryTypeId);
            if (entity == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Entry type not found."
                };
            }

            _dbContext.EntryTypes.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Entry type deleted successfully."
            };
        }
        catch (DbUpdateException dbEx)
        {
            var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
            Log.Error(dbEx, "Database error deleting entry type: {EntryTypeId}", entryTypeId);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error deleting entry type: {innerMessage}"
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting entry type: {EntryTypeId}", entryTypeId);
            var innerMessage = ex.InnerException?.Message ?? ex.Message;
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error deleting entry type: {innerMessage}"
            };
        }
    }

    public async Task<AuthenticationResponse> CreateAgeGroupAsync(AgeGroup ageGroup)
    {
        try
        {
            if (ageGroup == null || string.IsNullOrWhiteSpace(ageGroup.Name) || string.IsNullOrWhiteSpace(ageGroup.Code))
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Age group code and name are required."
                };
            }

            if (ageGroup.MinAge < 0 || ageGroup.MaxAge < ageGroup.MinAge)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Age group min and max ages must be valid."
                };
            }

            var exists = await _dbContext.AgeGroups.AnyAsync(ag => ag.Code == ageGroup.Code);
            if (exists)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "An age group with this code already exists."
                };
            }

            _dbContext.AgeGroups.Add(ageGroup);
            await _dbContext.SaveChangesAsync();

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Age group created successfully."
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating age group: {AgeGroupCode}", ageGroup?.Code);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error creating age group: {ex.Message}"
            };
        }
    }

    public async Task<AuthenticationResponse> UpdateAgeGroupAsync(AgeGroup ageGroup)
    {
        try
        {
            var entity = await _dbContext.AgeGroups.FindAsync(ageGroup.Id);
            if (entity == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Age group not found."
                };
            }

            if (string.IsNullOrWhiteSpace(ageGroup.Name) || string.IsNullOrWhiteSpace(ageGroup.Code))
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Age group code and name are required."
                };
            }

            if (ageGroup.MinAge < 0 || ageGroup.MaxAge < ageGroup.MinAge)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Age group min and max ages must be valid."
                };
            }

            var codeExists = await _dbContext.AgeGroups.AnyAsync(ag => ag.Code == ageGroup.Code && ag.Id != ageGroup.Id);
            if (codeExists)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "An age group with this code already exists."
                };
            }

            entity.Code = ageGroup.Code;
            entity.Name = ageGroup.Name;
            entity.MinAge = ageGroup.MinAge;
            entity.MaxAge = ageGroup.MaxAge;

            await _dbContext.SaveChangesAsync();

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Age group updated successfully."
            };
        }
        catch (DbUpdateException dbEx)
        {
            var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
            Log.Error(dbEx, "Database error updating age group: {AgeGroupId}", ageGroup.Id);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error updating age group: {innerMessage}"
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error updating age group: {AgeGroupId}", ageGroup.Id);
            var innerMessage = ex.InnerException?.Message ?? ex.Message;
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error updating age group: {innerMessage}"
            };
        }
    }

    public async Task<AuthenticationResponse> DeleteAgeGroupAsync(int ageGroupId)
    {
        try
        {
            var entity = await _dbContext.AgeGroups.FindAsync(ageGroupId);
            if (entity == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Age group not found."
                };
            }

            var categoriesUsingAgeGroup = await _dbContext.CompetitionCategories.AnyAsync(cc => cc.AgeGroupId == ageGroupId);
            if (categoriesUsingAgeGroup)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Cannot delete this age group because it is used in one or more competition categories. Please remove or update those categories first."
                };
            }

            var usersUsingAgeGroup = await _dbContext.Users.AnyAsync(u => u.AgeGroupId == ageGroupId);
            if (usersUsingAgeGroup)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Cannot delete this age group because it is assigned to one or more users. Please reassign or remove those users first."
                };
            }

            _dbContext.AgeGroups.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Age group deleted successfully."
            };
        }
        catch (DbUpdateException dbEx)
        {
            var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
            Log.Error(dbEx, "Database error deleting age group: {AgeGroupId}", ageGroupId);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error deleting age group: {innerMessage}"
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting age group: {AgeGroupId}", ageGroupId);
            var innerMessage = ex.InnerException?.Message ?? ex.Message;
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"Error deleting age group: {innerMessage}"
            };
        }
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        // Sempre busca dados atualizados do banco sem cache
        var users = await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.RolePermission)
            .Include(u => u.DanceSchool)
            .Include(u => u.DefaultFranchise)
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .ToListAsync();

        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Email = u.Email,
            FirstName = u.FirstName ?? string.Empty,
            LastName = u.LastName ?? string.Empty,
            PhoneNumber = u.Phone,
            Phone = u.Phone,
            Provider = "local",
            Address = u.Address,
            City = u.City,
            Zip = u.Zip,
            Province = u.Province,
            CountryId = u.CountryId,
            DanceSchoolId = u.DanceSchoolId,
            DefaultFranchiseId = u.DefaultFranchiseId,
            AgeGroupId = u.AgeGroupId,
            RolePermissionId = u.RolePermissionId
        }).ToList();
    }

    public async Task<AuthenticationResponse> DeleteUserAsync(int userId)
    {
        try
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "User not found."
                };
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            Log.Information("User deleted: {Email} (ID: {UserId})", user.Email, user.Id);

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "User deleted successfully."
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting user: {UserId}", userId);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"An error occurred while deleting the user: {ex.Message}"
            };
        }
    }

    public async Task<AuthenticationResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _dbContext.Users.Include(u => u.RolePermission).FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                };
            }
            if (string.IsNullOrEmpty(user.Password) || !PasswordHasher.VerifyPassword(request.Password, user.Password))
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                };
            }
            var role = user.RolePermission?.Name ?? "User";
            var token = GenerateJwtToken(user, role);
            Log.Information("User logged in: {Email} (ID: {UserId})", user.Email, user.Id);
            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Login successful",
                AccessToken = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName ?? string.Empty,
                    LastName = user.LastName ?? string.Empty,
                    RolePermissionId = user.RolePermissionId
                }
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Login error for email: {Email}", request.Email);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"An error occurred during login: {ex.Message}"
            };
        }
    }

    public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "All fields are required."
                };
            }
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Email already registered"
                };
            }
            var rolePermissionId = 3;
            var rolePermission = await _dbContext.UserRolePermissions.FindAsync(rolePermissionId);
            if (rolePermission == null)
            {
                return new AuthenticationResponse
                {
                    IsSuccess = false,
                    Message = "Role permission not allowed."
                };
            }
            var newUser = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                RolePermissionId = rolePermissionId,
                Password = PasswordHasher.HashPassword(request.Password)
            };
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();
            Log.Information("User registered: {Email} (ID: {UserId})", newUser.Email, newUser.Id);
            return new AuthenticationResponse
            {
                IsSuccess = true,
                Message = "Registration successful"
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Registration error for email: {Email}", request.Email);
            return new AuthenticationResponse
            {
                IsSuccess = false,
                Message = $"An error occurred during registration: {ex.Message}"
            };
        }
    }

    public async Task<AuthenticationResponse> FederatedLoginAsync(FederatedLoginRequest request)
    {
        Log.Information("Federated login attempted for provider: {Provider}", request.Provider);
        throw new NotImplementedException();
    }

    public async Task<UserDto?> GetCurrentUserAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return null;
        if (!int.TryParse(userId, out var id))
            return null;
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            Log.Warning("GetCurrentUserAsync: User not found for ID {UserId}", userId);
            return null;
        }
        Log.Information("GetCurrentUserAsync: User found {Email} (ID: {UserId})", user.Email, user.Id);
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            PhoneNumber = user.Phone,
            Provider = "local",
            Address = user.Address,
            City = user.City,
            Zip = user.Zip,
            Province = user.Province,
            CountryId = user.CountryId,
            DanceSchoolId = user.DanceSchoolId,
            Phone = user.Phone,
            DefaultFranchiseId = user.DefaultFranchiseId,
            AgeGroupId = user.AgeGroupId,
            RolePermissionId = user.RolePermissionId
        };
    }

    public async Task<AuthenticationResponse> UpdateProfileAsync(string userId, UserDto updatedProfile)
    {
        if (string.IsNullOrWhiteSpace(userId) || !int.TryParse(userId, out var id))
            return new AuthenticationResponse { IsSuccess = false, Message = "Invalid user id." };

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            return new AuthenticationResponse { IsSuccess = false, Message = "User not found." };

        user.FirstName = updatedProfile.FirstName;
        user.LastName = updatedProfile.LastName;
        user.Address = updatedProfile.Address;
        user.City = updatedProfile.City;
        user.Zip = updatedProfile.Zip;
        user.Province = updatedProfile.Province;
        user.CountryId = updatedProfile.CountryId ?? user.CountryId;
        user.Email = updatedProfile.Email;
        user.DanceSchoolId = updatedProfile.DanceSchoolId;
        user.Phone = updatedProfile.Phone;
        user.DefaultFranchiseId = updatedProfile.DefaultFranchiseId;
        user.AgeGroupId = updatedProfile.AgeGroupId;
        user.RolePermissionId = updatedProfile.RolePermissionId;

        await _dbContext.SaveChangesAsync();

        return new AuthenticationResponse { IsSuccess = true, Message = "Profile updated successfully." };
    }
    public async Task<AuthenticationResponse> ChangePasswordAsync(string userId, ChangePasswordRequest request) => throw new NotImplementedException();
    public async Task<AuthenticationResponse> RequestPasswordResetAsync(ResetPasswordRequest request) => throw new NotImplementedException();
    public async Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordConfirmRequest request) => throw new NotImplementedException();
    public async Task<AuthenticationResponse> VerifyEmailAsync(VerifyEmailRequest request) => throw new NotImplementedException();
    public async Task<bool> EmailExistsAsync(string email) => throw new NotImplementedException();

    public async Task LogoutAsync(string userId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId) || !int.TryParse(userId, out var id))
            {
                Log.Warning("LogoutAsync called with invalid userId: {UserId}", userId);
                return;
            }

            // Usa AsNoTracking para evitar problemas de concorrência e não precisa salvar mudanças
            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
                
            if (user != null)
            {
                Log.Information("User {UserId} ({Email}) logged out at {Time}", user.Id, user.Email, DateTime.UtcNow);
            }
            else
            {
                Log.Warning("LogoutAsync: User not found with ID {UserId}", id);
            }

            Log.Information("Logout process completed for user {UserId}", userId);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in LogoutAsync for user {UserId}: {Error}", userId, ex.Message);
            // Não relança a exceção para não interromper o processo de logout
        }
    }

    public async Task<AuthenticationResponse> RefreshTokenAsync(string refreshToken) => throw new NotImplementedException();

    private string GenerateJwtToken(User user, string role)
    {
        var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("DanceWavesSuperSecretKey_2025_ChangeMe!@#1234567890"));
        var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);
        var firstName = user.FirstName ?? string.Empty;
        var lastName = user.LastName ?? string.Empty;
        var fullName = $"{firstName} {lastName}".Trim();
        if (string.IsNullOrWhiteSpace(fullName))
        {
            fullName = user.Email ?? "User";
        }
        var claims = new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.Email ?? "User"),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email ?? string.Empty),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role),
            new System.Security.Claims.Claim("FullName", fullName),
            new System.Security.Claims.Claim("FirstName", firstName),
            new System.Security.Claims.Claim("LastName", lastName)
        };
        var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
            issuer: "DanceWaves",
            audience: "DanceWavesClient",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );
        return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
    }
}
