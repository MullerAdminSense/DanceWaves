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
/// Persistence adapter for EntryType entity
/// Implements IEntryTypePersistencePort using Entity Framework
/// </summary>
public class EntryTypePersistenceAdapter(ApplicationDbContext dbContext) : IEntryTypePersistencePort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<EntryTypeDto?> GetByIdAsync(int id)
    {
        var model = await _dbContext.EntryTypes.AsNoTracking().FirstOrDefaultAsync(et => et.Id == id);
        return model != null ? ModelToDtoMapper.ToDto(model) : null;
    }

    public async Task<IEnumerable<EntryTypeDto>> GetAllAsync()
    {
        var models = await _dbContext.EntryTypes.AsNoTracking().ToListAsync();
        return models.Select(ModelToDtoMapper.ToDto);
    }

    public async Task<EntryTypeDto> CreateAsync(EntryTypeDto dto)
    {
        var model = ModelToDtoMapper.ToModel(dto);
        _dbContext.EntryTypes.Add(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task<EntryTypeDto> UpdateAsync(EntryTypeDto dto)
    {
        var existingModel = await _dbContext.EntryTypes.FindAsync(dto.Id);
        if (existingModel != null)
        {
            var updatedModel = ModelToDtoMapper.ToModel(dto);
            _dbContext.Entry(existingModel).CurrentValues.SetValues(updatedModel);
            await _dbContext.SaveChangesAsync();
            return ModelToDtoMapper.ToDto(existingModel);
        }
        else
        {
            var model = ModelToDtoMapper.ToModel(dto);
            _dbContext.EntryTypes.Update(model);
            await _dbContext.SaveChangesAsync();
            return ModelToDtoMapper.ToDto(model);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var model = await _dbContext.EntryTypes.FindAsync(id);
        if (model != null)
        {
            _dbContext.EntryTypes.Remove(model);
            await _dbContext.SaveChangesAsync();
        }
    }
}
