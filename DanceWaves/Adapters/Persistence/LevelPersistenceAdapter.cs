using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DanceWaves.Application.Ports;
using DanceWaves.Models;
using DanceWaves.Data;

namespace DanceWaves.Adapters.Persistence;

/// <summary>
/// Persistence adapter for Level entity
/// Implements ILevelPersistencePort using Entity Framework
/// </summary>
public class LevelPersistenceAdapter(ApplicationDbContext dbContext) : ILevelPersistencePort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Level?> GetByIdAsync(int id)
    {
        return await _dbContext.Levels.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<IEnumerable<Level>> GetAllAsync()
    {
        return await _dbContext.Levels.AsNoTracking().ToListAsync();
    }

    public async Task<Level> CreateAsync(Level level)
    {
        _dbContext.Levels.Add(level);
        await _dbContext.SaveChangesAsync();
        return level;
    }

    public async Task<Level> UpdateAsync(Level level)
    {
        var existingLevel = await _dbContext.Levels.FindAsync(level.Id);
        if (existingLevel != null)
        {
            _dbContext.Entry(existingLevel).CurrentValues.SetValues(level);
            await _dbContext.SaveChangesAsync();
            return existingLevel;
        }
        else
        {
            _dbContext.Levels.Update(level);
            await _dbContext.SaveChangesAsync();
            return level;
        }
    }

    public async Task DeleteAsync(int id)
    {
        var level = await _dbContext.Levels.FindAsync(id);
        if (level != null)
        {
            _dbContext.Levels.Remove(level);
            await _dbContext.SaveChangesAsync();
        }
    }
}
