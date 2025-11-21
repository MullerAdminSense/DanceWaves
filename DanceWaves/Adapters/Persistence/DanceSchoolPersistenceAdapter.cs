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
        var model = await _dbContext.DanceSchools.FindAsync(id);
        return model != null ? ModelToDtoMapper.ToDto(model) : null;
    }

    public async Task<IEnumerable<DanceSchoolDto>> GetAllAsync()
    {
        var models = await _dbContext.DanceSchools.ToListAsync();
        return models.Select(ModelToDtoMapper.ToDto);
    }

    public async Task<DanceSchoolDto> CreateAsync(DanceSchoolDto dto)
    {
        var model = ModelToDtoMapper.ToModel(dto);
        _dbContext.DanceSchools.Add(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task<DanceSchoolDto> UpdateAsync(DanceSchoolDto dto)
    {
        var model = ModelToDtoMapper.ToModel(dto);
        _dbContext.DanceSchools.Update(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task DeleteAsync(int id)
    {
        var model = await _dbContext.DanceSchools.FindAsync(id);
        if (model != null)
        {
            _dbContext.DanceSchools.Remove(model);
            await _dbContext.SaveChangesAsync();
        }
    }
}
