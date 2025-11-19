using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DanceWaves.Application.Ports;
using DanceWaves.Models;
using DanceWaves.Data;

namespace DanceWaves.Adapters.Persistence;

public class StylePersistenceAdapter(ApplicationDbContext dbContext) : IStylePersistencePort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Style?> GetByIdAsync(int id)
    {
        return await _dbContext.Styles.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Style>> GetAllAsync()
    {
        return await _dbContext.Styles.AsNoTracking().ToListAsync();
    }

    public async Task<Style> CreateAsync(Style style)
    {
        _dbContext.Styles.Add(style);
        await _dbContext.SaveChangesAsync();
        return style;
    }

    public async Task<Style> UpdateAsync(Style style)
    {
        var existingStyle = await _dbContext.Styles.FindAsync(style.Id);
        if (existingStyle != null)
        {
            _dbContext.Entry(existingStyle).CurrentValues.SetValues(style);
            await _dbContext.SaveChangesAsync();
            return existingStyle;
        }
        else
        {
            _dbContext.Styles.Update(style);
            await _dbContext.SaveChangesAsync();
            return style;
        }
    }

    public async Task DeleteAsync(int id)
    {
        var style = await _dbContext.Styles.FindAsync(id);
        if (style != null)
        {
            _dbContext.Styles.Remove(style);
            await _dbContext.SaveChangesAsync();
        }
    }
}
