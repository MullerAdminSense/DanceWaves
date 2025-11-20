using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Dtos;

namespace DanceWaves.Application.Ports
{
    public interface ILevelPersistencePort
    {
        Task<LevelDto?> GetByIdAsync(int id);
        Task<IEnumerable<LevelDto>> GetAllAsync();
        Task<LevelDto> CreateAsync(LevelDto level);
        Task<LevelDto> UpdateAsync(LevelDto level);
        Task DeleteAsync(int id);
    }
}
