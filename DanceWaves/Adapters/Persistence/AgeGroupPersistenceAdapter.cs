using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Ports;
using DanceWaves.Models;
using DanceWaves.Data;

namespace DanceWaves.Adapters.Persistence;

public class AgeGroupPersistenceAdapter(ApplicationDbContext dbContext) : IAgeGroupPersistencePort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<AgeGroup?> GetByIdAsync(int id)
    {
        return await _dbContext.AgeGroups.FindAsync(id);
    }

    public async Task<IEnumerable<AgeGroup>> GetAllAsync()
    {
        return _dbContext.AgeGroups;
    }

    public async Task<AgeGroup> CreateAsync(AgeGroup ageGroup)
    {
        _dbContext.AgeGroups.Add(ageGroup);
        await _dbContext.SaveChangesAsync();
        return ageGroup;
    }

    public async Task<AgeGroup> UpdateAsync(AgeGroup ageGroup)
    {
        _dbContext.AgeGroups.Update(ageGroup);
        await _dbContext.SaveChangesAsync();
        return ageGroup;
    }

    public async Task DeleteAsync(int id)
    {
        var ageGroup = await GetByIdAsync(id);
        if (ageGroup != null)
        {
            _dbContext.AgeGroups.Remove(ageGroup);
            await _dbContext.SaveChangesAsync();
        }
    }
}
