using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Ports;
using DanceWaves.Models;
using DanceWaves.Data;

namespace DanceWaves.Adapters.Persistence;

/// <summary>
/// Persistence adapter for Score entity
/// Implements IScorePersistencePort using Entity Framework
/// </summary>
public class ScorePersistenceAdapter(ApplicationDbContext dbContext) : IScorePersistencePort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Score?> GetByIdAsync(int id)
    {
        return await _dbContext.Scores.FindAsync(id);
    }

    public async Task<IEnumerable<Score>> GetAllAsync()
    {
        return _dbContext.Scores;
    }

    public async Task<Score> CreateAsync(Score score)
    {
        _dbContext.Scores.Add(score);
        await _dbContext.SaveChangesAsync();
        return score;
    }

    public async Task<Score> UpdateAsync(Score score)
    {
        _dbContext.Scores.Update(score);
        await _dbContext.SaveChangesAsync();
        return score;
    }

    public async Task DeleteAsync(int id)
    {
        var score = await GetByIdAsync(id);
        if (score != null)
        {
            _dbContext.Scores.Remove(score);
            await _dbContext.SaveChangesAsync();
        }
    }
}
