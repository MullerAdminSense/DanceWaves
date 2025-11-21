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
        try
        {
            var model = await _dbContext.Levels.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
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

    public async Task<IEnumerable<LevelDto>> GetAllAsync()
    {
        try
        {
            var models = await _dbContext.Levels.AsNoTracking().ToListAsync();
            return models.Select(ModelToDtoMapper.ToDto);
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine($"DbContext concurrency error in GetAllAsync: {ex.Message}");
            return Enumerable.Empty<LevelDto>();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error in GetAllAsync: {ex.Message}");
            return Enumerable.Empty<LevelDto>();
        }
    }

    public async Task<LevelDto> CreateAsync(LevelDto dto)
    {
        try
        {
            var model = ModelToDtoMapper.ToModel(dto);
            _dbContext.Levels.Add(model);
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

    public async Task<LevelDto> UpdateAsync(LevelDto dto)
    {
        try
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
            var model = await _dbContext.Levels.FindAsync(id);
            if (model != null)
            {
                _dbContext.Levels.Remove(model);
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
