using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DanceWaves.Application.Ports;
using DanceWaves.Application.Dtos;
using DanceWaves.Models;
using DanceWaves.Adapters.Persistence.Mappers;
using DanceWaves.Infrastructure.Security;

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
        if (!string.IsNullOrWhiteSpace(model.Password))
        {
            model.Password = PasswordHasher.HashPassword(model.Password);
        }
        _dbContext.Users.Add(model);
        await _dbContext.SaveChangesAsync();
        return ModelToDtoMapper.ToDto(model);
    }

    public async Task<UserSimpleDto> UpdateAsync(UserSimpleDto dto)
    {
        var existingModel = await _dbContext.Users.FindAsync(dto.Id);
        if (existingModel != null)
        {
            // Update only non-password fields
            existingModel.FirstName = dto.FirstName;
            existingModel.LastName = dto.LastName;
            existingModel.Email = dto.Email;
            existingModel.Phone = dto.Phone;
            existingModel.Address = dto.Address;
            existingModel.City = dto.City;
            existingModel.Zip = dto.Zip;
            existingModel.Province = dto.Province;
            existingModel.CountryId = dto.CountryId;
            existingModel.DanceSchoolId = dto.DanceSchoolId;
            existingModel.DefaultFranchiseId = dto.DefaultFranchiseId;
            existingModel.AgeGroupId = dto.AgeGroupId;
            existingModel.RolePermissionId = dto.RolePermissionId;
            
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                existingModel.Password = PasswordHasher.HashPassword(dto.Password);
            }
            
            await _dbContext.SaveChangesAsync();
            return ModelToDtoMapper.ToDto(existingModel);
        }
        else
        {
            var model = ModelToDtoMapper.ToModel(dto);
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                model.Password = PasswordHasher.HashPassword(model.Password);
            }
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
