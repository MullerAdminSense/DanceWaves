using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DanceWaves.Application.Ports;
using DanceWaves.Models;

namespace DanceWaves.Adapters.Persistence;

public class UserPersistenceAdapter(Data.ApplicationDbContext dbContext) : IUserPersistencePort
{
    private readonly Data.ApplicationDbContext _dbContext = dbContext;

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbContext.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User> CreateAsync(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        var existingUser = await _dbContext.Users.FindAsync(user.Id);
        if (existingUser != null)
        {
            _dbContext.Entry(existingUser).CurrentValues.SetValues(user);
            await _dbContext.SaveChangesAsync();
            return existingUser;
        }
        else
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user != null)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
