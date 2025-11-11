using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Ports;
using DanceWaves.Models;

namespace DanceWaves.Adapters.Persistence
{
    /// <summary>
    /// Adaptador de persistência para Usuários
    /// Implementa a porta IUserPersistencePort usando Entity Framework
    /// </summary>
    public class UserPersistenceAdapter : IUserPersistencePort
    {
        private readonly Data.ApplicationDbContext _dbContext;

        public UserPersistenceAdapter(Data.ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return _dbContext.Users;
        }

        public async Task<User> CreateAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
