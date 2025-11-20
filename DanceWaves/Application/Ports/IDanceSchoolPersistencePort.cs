using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Dtos;

namespace DanceWaves.Application.Ports
{
    public interface IDanceSchoolPersistencePort
    {
        Task<DanceSchoolDto?> GetByIdAsync(int id);
        Task<IEnumerable<DanceSchoolDto>> GetAllAsync();
        Task<DanceSchoolDto> CreateAsync(DanceSchoolDto school);
        Task<DanceSchoolDto> UpdateAsync(DanceSchoolDto school);
        Task DeleteAsync(int id);
    }
}
