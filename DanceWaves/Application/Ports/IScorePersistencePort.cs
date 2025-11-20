using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Dtos;

namespace DanceWaves.Application.Ports
{
    public interface IScorePersistencePort
    {
        Task<ScoreDto?> GetByIdAsync(int id);
        Task<IEnumerable<ScoreDto>> GetAllAsync();
        Task<ScoreDto> CreateAsync(ScoreDto score);
        Task<ScoreDto> UpdateAsync(ScoreDto score);
        Task DeleteAsync(int id);
    }
}
