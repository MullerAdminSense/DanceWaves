using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Models;

namespace DanceWaves.Application.Ports
{
    public interface ILevelPersistencePort
    {
        Task<Level?> GetByIdAsync(int id);
        Task<IEnumerable<Level>> GetAllAsync();
        Task<Level> CreateAsync(Level level);
        Task<Level> UpdateAsync(Level level);
        Task DeleteAsync(int id);
    }
}
