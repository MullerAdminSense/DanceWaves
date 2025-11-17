using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Models;

namespace DanceWaves.Application.Ports
{
    public interface ICompetitionPersistencePort
    {
        Task<Competition> GetByIdAsync(int id);
        Task<IEnumerable<Competition>> GetAllAsync();
        Task<IEnumerable<Competition>> GetByStatusAsync(CompetitionStatus status);
        Task<Competition> CreateAsync(Competition competition);
        Task<Competition> UpdateAsync(Competition competition);
        Task DeleteAsync(int id);
    }
}
