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

public class StylePersistenceAdapter(ApplicationDbContext dbContext) : IStylePersistencePort
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<StyleDto?> GetByIdAsync(int id)
    {
        try
        {
            var model = await _dbContext.Styles.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
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

    public async Task<IEnumerable<StyleDto>> GetAllAsync()
    {
        try
        {
            var models = await _dbContext.Styles.AsNoTracking().ToListAsync();
            return models.Select(ModelToDtoMapper.ToDto);
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine($"DbContext concurrency error in GetAllAsync: {ex.Message}");
            return Enumerable.Empty<StyleDto>();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error in GetAllAsync: {ex.Message}");
            return Enumerable.Empty<StyleDto>();
        }
    }

    public async Task<StyleDto> CreateAsync(StyleDto dto)
    {
        try
        {
            var model = ModelToDtoMapper.ToModel(dto);
            _dbContext.Styles.Add(model);
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

    public async Task<StyleDto> UpdateAsync(StyleDto dto)
    {
        try
        {
            var existingModel = await _dbContext.Styles.FindAsync(dto.Id);
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
                _dbContext.Styles.Update(model);
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
            var model = await _dbContext.Styles.FindAsync(id);
            if (model != null)
            {
                _dbContext.Styles.Remove(model);
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
