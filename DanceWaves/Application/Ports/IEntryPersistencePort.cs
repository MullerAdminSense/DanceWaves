using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Dtos;

namespace DanceWaves.Application.Ports
{
    /// <summary>
    /// Porta para persistência de Entries
    /// Interface que define as operações de entrada do domínio
    /// </summary>
    public interface IEntryPersistencePort
    {
        Task<EntrySimpleDto?> GetByIdAsync(int id);
        Task<IEnumerable<EntrySimpleDto>> GetAllAsync();
        Task<EntrySimpleDto> CreateAsync(EntrySimpleDto entry);
        Task<EntrySimpleDto> UpdateAsync(EntrySimpleDto entry);
        Task DeleteAsync(int id);
    }
}
