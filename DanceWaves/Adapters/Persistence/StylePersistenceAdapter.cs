using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Ports;
using DanceWaves.Models;
using DanceWaves.Data;

namespace DanceWaves.Adapters.Persistence;

/// <summary>
/// Persistence adapter for Style entity
/// Implements IStylePersistencePort using Entity Framework
/// </summary>
public class StylePersistenceAdapter(ApplicationDbContext dbContext) : IStylePersistencePort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Style?> GetByIdAsync(int id)
    {
        return await _dbContext.Styles.FindAsync(id);
    }

    public async Task<IEnumerable<Style>> GetAllAsync()
    {
        return _dbContext.Styles;
    }

    public async Task<Style> CreateAsync(Style style)
    {
        _dbContext.Styles.Add(style);
        await _dbContext.SaveChangesAsync();
        return style;
    }

    public async Task<Style> UpdateAsync(Style style)
    {
        _dbContext.Styles.Update(style);
        await _dbContext.SaveChangesAsync();
        return style;
    }

    public async Task DeleteAsync(int id)
    {
        var style = await GetByIdAsync(id);
        if (style != null)
        {
            _dbContext.Styles.Remove(style);
            await _dbContext.SaveChangesAsync();
        }
    }
}
