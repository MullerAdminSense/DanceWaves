using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DanceWaves.Application.Ports;
using DanceWaves.Models;
using DanceWaves.Data;

namespace DanceWaves.Adapters.Persistence;

/// <summary>
/// Persistence adapter for EntryType entity
/// Implements IEntryTypePersistencePort using Entity Framework
/// </summary>
public class EntryTypePersistenceAdapter(ApplicationDbContext dbContext) : IEntryTypePersistencePort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<EntryType?> GetByIdAsync(int id)
    {
        return await _dbContext.EntryTypes.AsNoTracking().FirstOrDefaultAsync(et => et.Id == id);
    }

    public async Task<IEnumerable<EntryType>> GetAllAsync()
    {
        return await _dbContext.EntryTypes.AsNoTracking().ToListAsync();
    }

    public async Task<EntryType> CreateAsync(EntryType entryType)
    {
        _dbContext.EntryTypes.Add(entryType);
        await _dbContext.SaveChangesAsync();
        return entryType;
    }

    public async Task<EntryType> UpdateAsync(EntryType entryType)
    {
        var existingEntryType = await _dbContext.EntryTypes.FindAsync(entryType.Id);
        if (existingEntryType != null)
        {
            _dbContext.Entry(existingEntryType).CurrentValues.SetValues(entryType);
            await _dbContext.SaveChangesAsync();
            return existingEntryType;
        }
        else
        {
            _dbContext.EntryTypes.Update(entryType);
            await _dbContext.SaveChangesAsync();
            return entryType;
        }
    }

    public async Task DeleteAsync(int id)
    {
        var entryType = await _dbContext.EntryTypes.FindAsync(id);
        if (entryType != null)
        {
            _dbContext.EntryTypes.Remove(entryType);
            await _dbContext.SaveChangesAsync();
        }
    }
}
