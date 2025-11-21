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
        try
        {
            var model = await _dbContext.CompetitionCategories.FindAsync(id);
            return model != null ? ModelToDtoMapper.ToDto(model) : null;
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine($"DbContext concurrency error in GetByIdAsync: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error in GetByIdAsync: {ex.Message}");
            return null;
        }
    }

    public async Task<IEnumerable<CompetitionCategoryDto>> GetAllAsync()
    {
        try
        {
            var models = await _dbContext.CompetitionCategories.ToListAsync();
            return models.Select(ModelToDtoMapper.ToDto);
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine($"DbContext concurrency error in GetAllAsync: {ex.Message}");
            return Enumerable.Empty<CompetitionCategoryDto>();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error in GetAllAsync: {ex.Message}");
            return Enumerable.Empty<CompetitionCategoryDto>();
        }
    }

    public async Task<IEnumerable<CompetitionCategoryDto>> GetByCompetitionIdAsync(int competitionId)
    {
        try
        {
            var models = await _dbContext.CompetitionCategories
                .Where(c => c.CompetitionId == competitionId)
                .ToListAsync();
            return models.Select(ModelToDtoMapper.ToDto);
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine($"DbContext concurrency error in GetByCompetitionIdAsync: {ex.Message}");
            return Enumerable.Empty<CompetitionCategoryDto>();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error in GetByCompetitionIdAsync: {ex.Message}");
            return Enumerable.Empty<CompetitionCategoryDto>();
        }
    }

    public async Task<CompetitionCategoryDto> CreateAsync(CompetitionCategoryDto dto)
    {
        try
        {
            var model = ModelToDtoMapper.ToModel(dto);
            _dbContext.CompetitionCategories.Add(model);
            await _dbContext.SaveChangesAsync();
            return ModelToDtoMapper.ToDto(model);
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine($"DbContext concurrency error in CreateAsync: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error in CreateAsync: {ex.Message}");
            return null;
        }
    }

    public async Task<CompetitionCategoryDto> UpdateAsync(CompetitionCategoryDto dto)
    {
        try
        {
            var model = ModelToDtoMapper.ToModel(dto);
            _dbContext.CompetitionCategories.Update(model);
            await _dbContext.SaveChangesAsync();
            return ModelToDtoMapper.ToDto(model);
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine($"DbContext concurrency error in UpdateAsync: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error in UpdateAsync: {ex.Message}");
            return null;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var model = await _dbContext.CompetitionCategories.FindAsync(id);
            if (model != null)
            {
                _dbContext.CompetitionCategories.Remove(model);
                await _dbContext.SaveChangesAsync();
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine($"DbContext concurrency error in DeleteAsync: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error in DeleteAsync: {ex.Message}");
        }
    }
}
