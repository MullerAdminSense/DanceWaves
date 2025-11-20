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
/// Persistence adapter for Level entity
/// Implements ILevelPersistencePort using Entity Framework
/// </summary>
public class LevelPersistenceAdapter(ApplicationDbContext dbContext) : ILevelPersistencePort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<LevelDto?> GetByIdAsync(int id)
    {
        var model = await _dbContext.Levels.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
        return model != null ? ModelToDtoMapper.ToDto(model) : null;
    }

    public async Task<IEnumerable<LevelDto>> GetAllAsync()
    {
        var models = await _dbContext.Levels.AsNoTracking().ToListAsync();
        return models.Select(ModelToDtoMapper.ToDto);
    }

    public async Task<LevelDto> CreateAsync(LevelDto dto)
    {
        var model = ModelToDtoMapper.ToModel(dto);
        _dbContext.Levels.Add(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task<LevelDto> UpdateAsync(LevelDto dto)
    {
        var existingModel = await _dbContext.Levels.FindAsync(dto.Id);
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
            _dbContext.Levels.Update(model);
            await _dbContext.SaveChangesAsync();
            return ModelToDtoMapper.ToDto(model);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var model = await _dbContext.Levels.FindAsync(id);
        if (model != null)
        {
            _dbContext.Levels.Remove(model);
            await _dbContext.SaveChangesAsync();
        }
    }
}
