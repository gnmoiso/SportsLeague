using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.Domain.Interfaces.Services;

public interface ISponsorService : ISponsorRepository
{
    new Task<Sponsor> CreateAsync(Sponsor sponsor);
    new Task<IEnumerable<Sponsor>> GetAllAsync();
    Task<IEnumerable<Sponsor>> UpdateAsync();
    Task DeleteAsync();
}
