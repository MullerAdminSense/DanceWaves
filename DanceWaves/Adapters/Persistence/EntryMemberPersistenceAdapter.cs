using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DanceWaves.Application.Ports;
using DanceWaves.Application.Dtos;
using DanceWaves.Models;
using DanceWaves.Data;
using DanceWaves.Adapters.Persistence.Mappers;

namespace DanceWaves.Adapters.Persistence;

/// <summary>
/// Persistence adapter for EntryMember entity
/// Implements IEntryMemberPersistencePort using Entity Framework
/// </summary>
public class EntryMemberPersistenceAdapter(ApplicationDbContext dbContext) : IEntryMemberPersistencePort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<EntryMemberDto?> GetByIdAsync(int id)
    {
        var model = await _dbContext.EntryMembers.FindAsync(id);
        return model != null ? ModelToDtoMapper.ToDto(model) : null;
    }

    public async Task<IEnumerable<EntryMemberDto>> GetAllAsync()
    {
        var models = await _dbContext.EntryMembers.ToListAsync();
        return models.Select(ModelToDtoMapper.ToDto);
    }

    public async Task<EntryMemberDto> CreateAsync(EntryMemberDto dto)
    {
        var model = ModelToDtoMapper.ToModel(dto);
        _dbContext.EntryMembers.Add(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task<EntryMemberDto> UpdateAsync(EntryMemberDto dto)
    {
        var model = ModelToDtoMapper.ToModel(dto);
        _dbContext.EntryMembers.Update(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task DeleteAsync(int id)
    {
        var model = await _dbContext.EntryMembers.FindAsync(id);
        if (model != null)
        {
            _dbContext.EntryMembers.Remove(model);
            await _dbContext.SaveChangesAsync();
        }
    }
}
