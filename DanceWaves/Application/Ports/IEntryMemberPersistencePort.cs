using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Dtos;

namespace DanceWaves.Application.Ports
{
    public interface IEntryMemberPersistencePort
    {
        Task<EntryMemberDto?> GetByIdAsync(int id);
        Task<IEnumerable<EntryMemberDto>> GetAllAsync();
        Task<EntryMemberDto> CreateAsync(EntryMemberDto member);
        Task<EntryMemberDto> UpdateAsync(EntryMemberDto member);
        Task DeleteAsync(int id);
    }
}
