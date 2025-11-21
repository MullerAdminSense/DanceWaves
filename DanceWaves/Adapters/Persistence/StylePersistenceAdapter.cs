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
        var model = await _dbContext.Styles.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        return model != null ? ModelToDtoMapper.ToDto(model) : null;
    }

    public async Task<IEnumerable<StyleDto>> GetAllAsync()
    {
        var models = await _dbContext.Styles.AsNoTracking().ToListAsync();
        return models.Select(ModelToDtoMapper.ToDto);
    }

    public async Task<StyleDto> CreateAsync(StyleDto dto)
    {
        var model = ModelToDtoMapper.ToModel(dto);
        _dbContext.Styles.Add(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task<StyleDto> UpdateAsync(StyleDto dto)
    {
        var existingModel = await _dbContext.Styles.FindAsync(dto.Id);
        if (existingModel != null)
        {
            var updatedModel = ModelToDtoMapper.ToModel(dto);
            _dbContext.Entry(existingModel).CurrentValues.SetValues(updatedModel);
            await _dbContext.SaveChangesAsync();
            return ModelToDtoMapper.ToDto(existingModel);
        }

        var model = ModelToDtoMapper.ToModel(dto);
        _dbContext.Styles.Update(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task DeleteAsync(int id)
    {
        var model = await _dbContext.Styles.FindAsync(id);
        if (model != null)
        {
            _dbContext.Styles.Remove(model);
            await _dbContext.SaveChangesAsync();
        }
    }
}
