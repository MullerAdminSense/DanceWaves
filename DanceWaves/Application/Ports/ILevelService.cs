using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Models;

namespace DanceWaves.Application.Ports
{
    public interface ILevelService
    {
        Task<List<Level>> GetAllLevelsAsync();
        Task CreateLevelAsync(Level level);
        Task UpdateLevelAsync(Level level);
        Task DeleteLevelAsync(int levelId);
    }
}