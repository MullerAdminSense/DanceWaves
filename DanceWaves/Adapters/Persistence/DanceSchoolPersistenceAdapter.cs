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
/// Persistence adapter for DanceSchool entity
/// Implements IDanceSchoolPersistencePort using Entity Framework
/// </summary>
public class DanceSchoolPersistenceAdapter(ApplicationDbContext dbContext) : IDanceSchoolPersistencePort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<DanceSchoolDto?> GetByIdAsync(int id)
    {
        try
        {
            var model = await _dbContext.DanceSchools.FindAsync(id);
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

    public async Task<IEnumerable<DanceSchoolDto>> GetAllAsync()
    {
        try
        {
            var models = await _dbContext.DanceSchools.ToListAsync();
            return models.Select(ModelToDtoMapper.ToDto);
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine($"DbContext concurrency error in GetAllAsync: {ex.Message}");
            return Enumerable.Empty<DanceSchoolDto>();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error in GetAllAsync: {ex.Message}");
            return Enumerable.Empty<DanceSchoolDto>();
        }
    }

    public async Task<DanceSchoolDto> CreateAsync(DanceSchoolDto dto)
    {
        try
        {
            var model = ModelToDtoMapper.ToModel(dto);
            _dbContext.DanceSchools.Add(model);
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

    public async Task<DanceSchoolDto> UpdateAsync(DanceSchoolDto dto)
    {
        try
        {
            var model = ModelToDtoMapper.ToModel(dto);
            _dbContext.DanceSchools.Update(model);
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
            var model = await _dbContext.DanceSchools.FindAsync(id);
            if (model != null)
            {
                _dbContext.DanceSchools.Remove(model);
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
