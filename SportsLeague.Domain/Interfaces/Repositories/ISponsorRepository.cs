using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories;

public interface ISponsorRepository : IGenericRepository<Sponsor>
{
    Task<Sponsor?> ExistByNameAsync(string name);
    Task<IEnumerable<Sponsor>> ExistByNameAsync(IEnumerable<string> names);
}
