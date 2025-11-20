using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DanceWaves.Application.Ports;
using DanceWaves.Application.Dtos;
using DanceWaves.Models;
using DanceWaves.Adapters.Persistence.Mappers;

namespace DanceWaves.Adapters.Persistence;

public class UserPersistenceAdapter(Data.ApplicationDbContext dbContext) : IUserPersistencePort
{
    private readonly Data.ApplicationDbContext _dbContext = dbContext;

    public async Task<UserSimpleDto?> GetByIdAsync(int id)
    {
        var model = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        return model != null ? ModelToDtoMapper.ToDto(model) : null;
    }

    public async Task<UserSimpleDto?> GetByEmailAsync(string email)
    {
        var model = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        return model != null ? ModelToDtoMapper.ToDto(model) : null;
    }

    public async Task<IEnumerable<UserSimpleDto>> GetAllAsync()
    {
        var models = await _dbContext.Users.AsNoTracking().ToListAsync();
        return models.Select(ModelToDtoMapper.ToDto);
    }

    public async Task<UserSimpleDto> CreateAsync(UserSimpleDto dto)
    {
        var model = ModelToDtoMapper.ToModel(dto);
        _dbContext.Users.Add(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task<UserSimpleDto> UpdateAsync(UserSimpleDto dto)
    {
        var existingModel = await _dbContext.Users.FindAsync(dto.Id);
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
            _dbContext.Users.Update(model);
            await _dbContext.SaveChangesAsync();
            return ModelToDtoMapper.ToDto(model);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var model = await _dbContext.Users.FindAsync(id);
        if (model != null)
        {
            _dbContext.Users.Remove(model);
            await _dbContext.SaveChangesAsync();
        }
    }
}
