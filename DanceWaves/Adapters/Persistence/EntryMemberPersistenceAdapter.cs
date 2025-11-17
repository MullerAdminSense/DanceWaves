using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Ports;
using DanceWaves.Models;
using DanceWaves.Data;

namespace DanceWaves.Adapters.Persistence;

/// <summary>
/// Persistence adapter for EntryMember entity
/// Implements IEntryMemberPersistencePort using Entity Framework
/// </summary>
public class EntryMemberPersistenceAdapter(ApplicationDbContext dbContext) : IEntryMemberPersistencePort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<EntryMember?> GetByIdAsync(int id)
    {
        return await _dbContext.EntryMembers.FindAsync(id);
    }

    public async Task<IEnumerable<EntryMember>> GetAllAsync()
    {
        return _dbContext.EntryMembers;
    }

    public async Task<EntryMember> CreateAsync(EntryMember member)
    {
        _dbContext.EntryMembers.Add(member);
        await _dbContext.SaveChangesAsync();
        return member;
    }

    public async Task<EntryMember> UpdateAsync(EntryMember member)
    {
        _dbContext.EntryMembers.Update(member);
        await _dbContext.SaveChangesAsync();
        return member;
    }

    public async Task DeleteAsync(int id)
    {
        var member = await GetByIdAsync(id);
        if (member != null)
        {
            _dbContext.EntryMembers.Remove(member);
            await _dbContext.SaveChangesAsync();
        }
    }
}
