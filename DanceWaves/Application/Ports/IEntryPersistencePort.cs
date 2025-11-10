using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Models;

namespace DanceWaves.Application.Ports
{
    /// <summary>
    /// Porta para persistência de Entries
    /// Interface que define as operações de entrada do domínio
    /// </summary>
    public interface IEntryPersistencePort
    {
        Task<Entry> GetByIdAsync(int id);
        Task<IEnumerable<Entry>> GetAllAsync();
        Task<Entry> CreateAsync(Entry entry);
        Task<Entry> UpdateAsync(Entry entry);
        Task DeleteAsync(int id);
    }
}
