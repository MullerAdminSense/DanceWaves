using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DanceWaves.Application.Ports;
using DanceWaves.Application.Dtos;
using DanceWaves.Models;
using DanceWaves.Adapters.Persistence.Mappers;

namespace DanceWaves.Adapters.Persistence;

/// <summary>
/// Adaptador de persistência para Entries
/// Implementa a porta IEntryPersistencePort usando Entity Framework
/// </summary>
public class EntryPersistenceAdapter(Data.ApplicationDbContext dbContext) : IEntryPersistencePort
{
    private readonly Data.ApplicationDbContext _dbContext = dbContext;

    public async Task<EntrySimpleDto?> GetByIdAsync(int id)
    {
        var model = await _dbContext.Entries.FindAsync(id);
        return model != null ? ModelToDtoMapper.ToDto(model) : null;
    }

    public async Task<IEnumerable<EntrySimpleDto>> GetAllAsync()
    {
        var models = await _dbContext.Entries.ToListAsync();
        return models.Select(ModelToDtoMapper.ToDto);
    }

    public async Task<EntrySimpleDto> CreateAsync(EntrySimpleDto dto)
    {
        var model = ModelToDtoMapper.ToModel(dto);
        _dbContext.Entries.Add(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task<EntrySimpleDto> UpdateAsync(EntrySimpleDto dto)
    {
        var model = ModelToDtoMapper.ToModel(dto);
        _dbContext.Entries.Update(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task DeleteAsync(int id)
    {
        var model = await _dbContext.Entries.FindAsync(id);
        if (model != null)
        {
            _dbContext.Entries.Remove(model);
            await _dbContext.SaveChangesAsync();
        }
    }
}
