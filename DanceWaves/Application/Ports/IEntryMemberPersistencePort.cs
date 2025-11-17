using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Models;

namespace DanceWaves.Application.Ports
{
    public interface IEntryMemberPersistencePort
    {
        Task<EntryMember?> GetByIdAsync(int id);
        Task<IEnumerable<EntryMember>> GetAllAsync();
        Task<EntryMember> CreateAsync(EntryMember member);
        Task<EntryMember> UpdateAsync(EntryMember member);
        Task DeleteAsync(int id);
    }
}
