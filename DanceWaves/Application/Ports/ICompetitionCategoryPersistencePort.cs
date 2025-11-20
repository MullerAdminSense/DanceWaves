using System.Collections.Generic;
using System.Threading.Tasks;
using DanceWaves.Application.Dtos;

namespace DanceWaves.Application.Ports
{
    public interface ICompetitionCategoryPersistencePort
    {
        Task<CompetitionCategoryDto?> GetByIdAsync(int id);
        Task<IEnumerable<CompetitionCategoryDto>> GetAllAsync();
        Task<IEnumerable<CompetitionCategoryDto>> GetByCompetitionIdAsync(int competitionId);
        Task<CompetitionCategoryDto> CreateAsync(CompetitionCategoryDto category);
        Task<CompetitionCategoryDto> UpdateAsync(CompetitionCategoryDto category);
        Task DeleteAsync(int id);
    }
}
