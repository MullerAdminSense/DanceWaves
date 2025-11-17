using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Ports;
using DanceWaves.Models;

namespace DanceWaves.Application.UseCases
{
    public class ListEntriesUseCase(IEntryPersistencePort entryPersistencePort)
    {
        private readonly IEntryPersistencePort _entryPersistencePort = entryPersistencePort;

        public async Task<IEnumerable<Entry>> ExecuteAsync()
        {
            return await _entryPersistencePort.GetAllAsync();
        }
    }
}
