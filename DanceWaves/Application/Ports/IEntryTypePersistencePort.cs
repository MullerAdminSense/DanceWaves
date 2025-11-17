using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Models;

namespace DanceWaves.Application.Ports
{
    public interface IEntryTypePersistencePort
    {
        Task<EntryType?> GetByIdAsync(int id);
        Task<IEnumerable<EntryType>> GetAllAsync();
        Task<EntryType> CreateAsync(EntryType entryType);
        Task<EntryType> UpdateAsync(EntryType entryType);
        Task DeleteAsync(int id);
    }
}
