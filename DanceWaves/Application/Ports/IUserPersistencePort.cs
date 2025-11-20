using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Dtos;

namespace DanceWaves.Application.Ports
{
    public interface IUserPersistencePort
    {
        Task<UserSimpleDto?> GetByIdAsync(int id);
        Task<UserSimpleDto?> GetByEmailAsync(string email);
        Task<IEnumerable<UserSimpleDto>> GetAllAsync();
        Task<UserSimpleDto> CreateAsync(UserSimpleDto user);
        Task<UserSimpleDto> UpdateAsync(UserSimpleDto user);
        Task DeleteAsync(int id);
    }
}
