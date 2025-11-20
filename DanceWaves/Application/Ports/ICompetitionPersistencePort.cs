using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Dtos;
using DanceWaves.Models;

namespace DanceWaves.Application.Ports
{
    public interface ICompetitionPersistencePort
    {
        Task<CompetitionDto?> GetByIdAsync(int id);
        Task<IEnumerable<CompetitionDto>> GetAllAsync();
        Task<IEnumerable<CompetitionDto>> GetByStatusAsync(CompetitionStatus status);
        Task<CompetitionDto> CreateAsync(CompetitionDto competition);
        Task<CompetitionDto> UpdateAsync(CompetitionDto competition);
        Task DeleteAsync(int id);
    }
}
