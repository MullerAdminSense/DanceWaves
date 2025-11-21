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

public class AgeGroupPersistenceAdapter(ApplicationDbContext dbContext) : IAgeGroupPersistencePort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<AgeGroupDto?> GetByIdAsync(int id)
    {
        try
        {
            var model = await _dbContext.AgeGroups.AsNoTracking().FirstOrDefaultAsync(ag => ag.Id == id);
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

    public async Task<IEnumerable<AgeGroupDto>> GetAllAsync()
    {
        try
        {
            var models = await _dbContext.AgeGroups.AsNoTracking().ToListAsync();
            return models.Select(ModelToDtoMapper.ToDto);
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine($"DbContext concurrency error in GetAllAsync: {ex.Message}");
            return Enumerable.Empty<AgeGroupDto>();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error in GetAllAsync: {ex.Message}");
            return Enumerable.Empty<AgeGroupDto>();
        }
    }

    public async Task<AgeGroupDto> CreateAsync(AgeGroupDto dto)
    {
        try
        {
            var model = ModelToDtoMapper.ToModel(dto);
            _dbContext.AgeGroups.Add(model);
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

    public async Task<AgeGroupDto> UpdateAsync(AgeGroupDto dto)
    {
        try
        {
            var existingModel = await _dbContext.AgeGroups.FindAsync(dto.Id);
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
                _dbContext.AgeGroups.Update(model);
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
            var model = await _dbContext.AgeGroups.FindAsync(id);
            if (model != null)
            {
                _dbContext.AgeGroups.Remove(model);
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
