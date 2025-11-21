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
        try
        {
            var model = await _dbContext.Scores.FindAsync(id);
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

    public async Task<IEnumerable<ScoreDto>> GetAllAsync()
    {
        try
        {
            var models = await _dbContext.Scores.ToListAsync();
            return models.Select(ModelToDtoMapper.ToDto);
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine($"DbContext concurrency error in GetAllAsync: {ex.Message}");
            return Enumerable.Empty<ScoreDto>();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error in GetAllAsync: {ex.Message}");
            return Enumerable.Empty<ScoreDto>();
        }
    }

    public async Task<ScoreDto> CreateAsync(ScoreDto dto)
    {
        try
        {
            var model = ModelToDtoMapper.ToModel(dto);
            _dbContext.Scores.Add(model);
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

    public async Task<ScoreDto> UpdateAsync(ScoreDto dto)
    {
        try
        {
            var model = ModelToDtoMapper.ToModel(dto);
            _dbContext.Scores.Update(model);
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
            var model = await _dbContext.Scores.FindAsync(id);
            if (model != null)
            {
                _dbContext.Scores.Remove(model);
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
