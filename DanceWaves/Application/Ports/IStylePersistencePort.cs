using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Models;

namespace DanceWaves.Application.Ports
{
    public interface IStylePersistencePort
    {
        Task<Style?> GetByIdAsync(int id);
        Task<IEnumerable<Style>> GetAllAsync();
        Task<Style> CreateAsync(Style style);
        Task<Style> UpdateAsync(Style style);
        Task DeleteAsync(int id);
    }
}
