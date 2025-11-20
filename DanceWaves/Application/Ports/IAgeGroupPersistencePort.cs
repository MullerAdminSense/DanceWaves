using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Dtos;

namespace DanceWaves.Application.Ports
{
    public interface IAgeGroupPersistencePort
    {
        Task<AgeGroupDto?> GetByIdAsync(int id);
        Task<IEnumerable<AgeGroupDto>> GetAllAsync();
        Task<AgeGroupDto> CreateAsync(AgeGroupDto ageGroup);
        Task<AgeGroupDto> UpdateAsync(AgeGroupDto ageGroup);
        Task DeleteAsync(int id);
    }
}
