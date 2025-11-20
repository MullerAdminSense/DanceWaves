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
/// Persistence adapter for Score entity
/// Implements IScorePersistencePort using Entity Framework
/// </summary>
public class ScorePersistenceAdapter(ApplicationDbContext dbContext) : IScorePersistencePort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<ScoreDto?> GetByIdAsync(int id)
    {
        var model = await _dbContext.Scores.FindAsync(id);
        return model != null ? ModelToDtoMapper.ToDto(model) : null;
    }

    public async Task<IEnumerable<ScoreDto>> GetAllAsync()
    {
        var models = await _dbContext.Scores.ToListAsync();
        return models.Select(ModelToDtoMapper.ToDto);
    }

    public async Task<ScoreDto> CreateAsync(ScoreDto dto)
    {
        var model = ModelToDtoMapper.ToModel(dto);
        _dbContext.Scores.Add(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task<ScoreDto> UpdateAsync(ScoreDto dto)
    {
        var model = ModelToDtoMapper.ToModel(dto);
        _dbContext.Scores.Update(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task DeleteAsync(int id)
    {
        var model = await _dbContext.Scores.FindAsync(id);
        if (model != null)
        {
            _dbContext.Scores.Remove(model);
            await _dbContext.SaveChangesAsync();
        }
    }
}
