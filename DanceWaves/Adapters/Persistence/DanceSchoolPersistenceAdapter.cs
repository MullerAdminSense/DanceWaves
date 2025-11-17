using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Ports;
using DanceWaves.Models;
using DanceWaves.Data;

namespace DanceWaves.Adapters.Persistence;

/// <summary>
/// Persistence adapter for DanceSchool entity
/// Implements IDanceSchoolPersistencePort using Entity Framework
/// </summary>
public class DanceSchoolPersistenceAdapter(ApplicationDbContext dbContext) : IDanceSchoolPersistencePort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<DanceSchool?> GetByIdAsync(int id)
    {
        return await _dbContext.DanceSchools.FindAsync(id);
    }

    public async Task<IEnumerable<DanceSchool>> GetAllAsync()
    {
        return _dbContext.DanceSchools;
    }

    public async Task<DanceSchool> CreateAsync(DanceSchool school)
    {
        _dbContext.DanceSchools.Add(school);
        await _dbContext.SaveChangesAsync();
        return school;
    }

    public async Task<DanceSchool> UpdateAsync(DanceSchool school)
    {
        _dbContext.DanceSchools.Update(school);
        await _dbContext.SaveChangesAsync();
        return school;
    }

    public async Task DeleteAsync(int id)
    {
        var school = await GetByIdAsync(id);
        if (school != null)
        {
            _dbContext.DanceSchools.Remove(school);
            await _dbContext.SaveChangesAsync();
        }
    }
}
