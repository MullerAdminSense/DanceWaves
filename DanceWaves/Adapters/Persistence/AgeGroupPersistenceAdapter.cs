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
        var model = await _dbContext.AgeGroups.AsNoTracking().FirstOrDefaultAsync(ag => ag.Id == id);
        return model != null ? ModelToDtoMapper.ToDto(model) : null;
    }

    public async Task<IEnumerable<AgeGroupDto>> GetAllAsync()
    {
        var models = await _dbContext.AgeGroups.AsNoTracking().ToListAsync();
        return models.Select(ModelToDtoMapper.ToDto);
    }

    public async Task<AgeGroupDto> CreateAsync(AgeGroupDto dto)
    {
        var model = ModelToDtoMapper.ToModel(dto);
        _dbContext.AgeGroups.Add(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task<AgeGroupDto> UpdateAsync(AgeGroupDto dto)
    {
        var existingModel = await _dbContext.AgeGroups.FindAsync(dto.Id);
        if (existingModel != null)
        {
            var updatedModel = ModelToDtoMapper.ToModel(dto);
            _dbContext.Entry(existingModel).CurrentValues.SetValues(updatedModel);
            await _dbContext.SaveChangesAsync();
            return ModelToDtoMapper.ToDto(existingModel);
        }

        var model = ModelToDtoMapper.ToModel(dto);
        _dbContext.AgeGroups.Update(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task DeleteAsync(int id)
    {
        var model = await _dbContext.AgeGroups.FindAsync(id);
        if (model != null)
        {
            _dbContext.AgeGroups.Remove(model);
            await _dbContext.SaveChangesAsync();
        }
    }
}
