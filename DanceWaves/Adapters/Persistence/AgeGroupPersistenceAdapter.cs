using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DanceWaves.Application.Ports;
using DanceWaves.Models;
using DanceWaves.Data;

namespace DanceWaves.Adapters.Persistence;

public class AgeGroupPersistenceAdapter(ApplicationDbContext dbContext) : IAgeGroupPersistencePort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<AgeGroup?> GetByIdAsync(int id)
    {
        return await _dbContext.AgeGroups.AsNoTracking().FirstOrDefaultAsync(ag => ag.Id == id);
    }

    public async Task<IEnumerable<AgeGroup>> GetAllAsync()
    {
        return await _dbContext.AgeGroups.AsNoTracking().ToListAsync();
    }

    public async Task<AgeGroup> CreateAsync(AgeGroup ageGroup)
    {
        _dbContext.AgeGroups.Add(ageGroup);
        await _dbContext.SaveChangesAsync();
        return ageGroup;
    }

    public async Task<AgeGroup> UpdateAsync(AgeGroup ageGroup)
    {
        var existingAgeGroup = await _dbContext.AgeGroups.FindAsync(ageGroup.Id);
        if (existingAgeGroup != null)
        {
            _dbContext.Entry(existingAgeGroup).CurrentValues.SetValues(ageGroup);
            await _dbContext.SaveChangesAsync();
            return existingAgeGroup;
        }
        else
        {
            _dbContext.AgeGroups.Update(ageGroup);
            await _dbContext.SaveChangesAsync();
            return ageGroup;
        }
    }

    public async Task DeleteAsync(int id)
    {
        var ageGroup = await _dbContext.AgeGroups.FindAsync(id);
        if (ageGroup != null)
        {
            _dbContext.AgeGroups.Remove(ageGroup);
            await _dbContext.SaveChangesAsync();
        }
    }
}
