using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Models;

namespace DanceWaves.Application.Ports
{
    /// <summary>
    /// Porta para persistência de Usuários
    /// Interface que define as operações de usuários do domínio
    /// </summary>
    public interface IUserPersistencePort
    {
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}
