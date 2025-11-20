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
/// Persistence adapter for Competition entity
/// Implements ICompetitionPersistencePort using Entity Framework
/// </summary>
public class CompetitionPersistenceAdapter(ApplicationDbContext dbContext) : ICompetitionPersistencePort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<CompetitionDto?> GetByIdAsync(int id)
    {
        var model = await _dbContext.Competitions.FindAsync(id);
        return model != null ? ModelToDtoMapper.ToDto(model) : null;
    }

    public async Task<IEnumerable<CompetitionDto>> GetAllAsync()
    {
        var models = await _dbContext.Competitions.ToListAsync();
        return models.Select(ModelToDtoMapper.ToDto);
    }

    public async Task<IEnumerable<CompetitionDto>> GetByStatusAsync(CompetitionStatus status)
    {
        var models = await _dbContext.Competitions
            .Where(c => c.Status == status)
            .ToListAsync();
        return models.Select(ModelToDtoMapper.ToDto);
    }

    public async Task<CompetitionDto> CreateAsync(CompetitionDto dto)
    {
        var model = ModelToDtoMapper.ToModel(dto);
        _dbContext.Competitions.Add(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task<CompetitionDto> UpdateAsync(CompetitionDto dto)
    {
        var existingModel = await _dbContext.Competitions.FindAsync(dto.Id);
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
            _dbContext.Competitions.Update(model);
            await _dbContext.SaveChangesAsync();
            return ModelToDtoMapper.ToDto(model);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var model = await _dbContext.Competitions.FindAsync(id);
        if (model != null)
        {
            _dbContext.Competitions.Remove(model);
            await _dbContext.SaveChangesAsync();
        }
    }
}
