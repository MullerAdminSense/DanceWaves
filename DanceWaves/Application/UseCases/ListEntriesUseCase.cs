using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Ports;
using DanceWaves.Models;

namespace DanceWaves.Application.UseCases
{
    /// <summary>
    /// Use Case para listar todas as Entries
    /// Implementa o núcleo de lógica de negócio para consulta de entradas
    /// </summary>
    public class ListEntriesUseCase
    {
        private readonly IEntryPersistencePort _entryPersistencePort;

        public ListEntriesUseCase(IEntryPersistencePort entryPersistencePort)
        {
            _entryPersistencePort = entryPersistencePort;
        }

        /// <summary>
        /// Executa o caso de uso para listar todas as entradas
        /// </summary>
        public async Task<IEnumerable<Entry>> ExecuteAsync()
        {
            return await _entryPersistencePort.GetAllAsync();
        }
    }
}
