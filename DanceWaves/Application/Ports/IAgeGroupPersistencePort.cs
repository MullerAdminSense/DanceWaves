using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Models;

namespace DanceWaves.Application.Ports
{
    public interface IAgeGroupPersistencePort
    {
        Task<AgeGroup?> GetByIdAsync(int id);
        Task<IEnumerable<AgeGroup>> GetAllAsync();
        Task<AgeGroup> CreateAsync(AgeGroup ageGroup);
        Task<AgeGroup> UpdateAsync(AgeGroup ageGroup);
        Task DeleteAsync(int id);
    }
}
