using System.Collections.Generic;
using System.Threading.Tasks;
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
        return await _dbContext.Levels.FindAsync(id);
    }

    public async Task<IEnumerable<Level>> GetAllAsync()
    {
        return _dbContext.Levels;
    }

    public async Task<Level> CreateAsync(Level level)
    {
        _dbContext.Levels.Add(level);
        await _dbContext.SaveChangesAsync();
        return level;
    }

    public async Task<Level> UpdateAsync(Level level)
    {
        _dbContext.Levels.Update(level);
        await _dbContext.SaveChangesAsync();
        return level;
    }

    public async Task DeleteAsync(int id)
    {
        var level = await GetByIdAsync(id);
        if (level != null)
        {
            _dbContext.Levels.Remove(level);
            await _dbContext.SaveChangesAsync();
        }
    }
}
