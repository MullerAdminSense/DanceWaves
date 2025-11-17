using System.Collections.Generic;
using System.Threading.Tasks;
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
        return await _dbContext.EntryTypes.FindAsync(id);
    }

    public async Task<IEnumerable<EntryType>> GetAllAsync()
    {
        return _dbContext.EntryTypes;
    }

    public async Task<EntryType> CreateAsync(EntryType entryType)
    {
        _dbContext.EntryTypes.Add(entryType);
        await _dbContext.SaveChangesAsync();
        return entryType;
    }

    public async Task<EntryType> UpdateAsync(EntryType entryType)
    {
        _dbContext.EntryTypes.Update(entryType);
        await _dbContext.SaveChangesAsync();
        return entryType;
    }

    public async Task DeleteAsync(int id)
    {
        var entryType = await GetByIdAsync(id);
        if (entryType != null)
        {
            _dbContext.EntryTypes.Remove(entryType);
            await _dbContext.SaveChangesAsync();
        }
    }
}
