using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Dtos;

namespace DanceWaves.Application.Ports
{
    public interface IStylePersistencePort
    {
        Task<StyleDto?> GetByIdAsync(int id);
        Task<IEnumerable<StyleDto>> GetAllAsync();
        Task<StyleDto> CreateAsync(StyleDto style);
        Task<StyleDto> UpdateAsync(StyleDto style);
        Task DeleteAsync(int id);
    }
}
