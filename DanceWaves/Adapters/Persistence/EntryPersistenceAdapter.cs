using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Ports;
using DanceWaves.Models;

namespace DanceWaves.Adapters.Persistence
{
    /// <summary>
    /// Adaptador de persistência para Entries
    /// Implementa a porta IEntryPersistencePort usando Entity Framework
    /// </summary>
    public class EntryPersistenceAdapter : IEntryPersistencePort
    {
        private readonly Data.ApplicationDbContext _dbContext;

        public EntryPersistenceAdapter(Data.ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Entry> GetByIdAsync(int id)
        {
            return await _dbContext.Entries.FindAsync(id);
        }

        public async Task<IEnumerable<Entry>> GetAllAsync()
        {
            return _dbContext.Entries;
        }

        public async Task<Entry> CreateAsync(Entry entry)
        {
            _dbContext.Entries.Add(entry);
            await _dbContext.SaveChangesAsync();
            return entry;
        }

        public async Task<Entry> UpdateAsync(Entry entry)
        {
            _dbContext.Entries.Update(entry);
            await _dbContext.SaveChangesAsync();
            return entry;
        }

        public async Task DeleteAsync(int id)
        {
            var entry = await GetByIdAsync(id);
            if (entry != null)
            {
                _dbContext.Entries.Remove(entry);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
