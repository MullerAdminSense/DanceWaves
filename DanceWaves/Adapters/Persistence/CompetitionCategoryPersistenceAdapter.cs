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
/// Persistence adapter for CompetitionCategory entity
/// Implements ICompetitionCategoryPersistencePort using Entity Framework
/// </summary>
public class CompetitionCategoryPersistenceAdapter(ApplicationDbContext dbContext) : ICompetitionCategoryPersistencePort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<CompetitionCategoryDto?> GetByIdAsync(int id)
    {
        var model = await _dbContext.CompetitionCategories.FindAsync(id);
        return model != null ? ModelToDtoMapper.ToDto(model) : null;
    }

    public async Task<IEnumerable<CompetitionCategoryDto>> GetAllAsync()
    {
        var models = await _dbContext.CompetitionCategories.ToListAsync();
        return models.Select(ModelToDtoMapper.ToDto);
    }

    public async Task<IEnumerable<CompetitionCategoryDto>> GetByCompetitionIdAsync(int competitionId)
    {
        var models = await _dbContext.CompetitionCategories
            .Where(c => c.CompetitionId == competitionId)
            .ToListAsync();
        return models.Select(ModelToDtoMapper.ToDto);
    }

    public async Task<CompetitionCategoryDto> CreateAsync(CompetitionCategoryDto dto)
    {
        var model = ModelToDtoMapper.ToModel(dto);
        _dbContext.CompetitionCategories.Add(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task<CompetitionCategoryDto> UpdateAsync(CompetitionCategoryDto dto)
    {
        var model = ModelToDtoMapper.ToModel(dto);
        _dbContext.CompetitionCategories.Update(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task DeleteAsync(int id)
    {
        var model = await _dbContext.CompetitionCategories.FindAsync(id);
        if (model != null)
        {
            _dbContext.CompetitionCategories.Remove(model);
            await _dbContext.SaveChangesAsync();
        }
    }
}
