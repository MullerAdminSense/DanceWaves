using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Dtos;

namespace DanceWaves.Application.Ports
{
    public interface IEntryTypePersistencePort
    {
        Task<EntryTypeDto?> GetByIdAsync(int id);
        Task<IEnumerable<EntryTypeDto>> GetAllAsync();
        Task<EntryTypeDto> CreateAsync(EntryTypeDto entryType);
        Task<EntryTypeDto> UpdateAsync(EntryTypeDto entryType);
        Task DeleteAsync(int id);
    }
}
