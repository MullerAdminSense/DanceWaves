using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Models;

namespace DanceWaves.Application.Ports
{
    public interface IDanceSchoolPersistencePort
    {
        Task<DanceSchool?> GetByIdAsync(int id);
        Task<IEnumerable<DanceSchool>> GetAllAsync();
        Task<DanceSchool> CreateAsync(DanceSchool school);
        Task<DanceSchool> UpdateAsync(DanceSchool school);
        Task DeleteAsync(int id);
    }
}
