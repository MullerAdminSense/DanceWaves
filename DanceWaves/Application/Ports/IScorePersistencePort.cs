using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Models;

namespace DanceWaves.Application.Ports
{
    public interface IScorePersistencePort
    {
        Task<Score?> GetByIdAsync(int id);
        Task<IEnumerable<Score>> GetAllAsync();
        Task<Score> CreateAsync(Score score);
        Task<Score> UpdateAsync(Score score);
        Task DeleteAsync(int id);
    }
}
