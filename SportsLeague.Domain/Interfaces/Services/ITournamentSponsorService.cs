using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services;

public interface ITournamentSponsorService
{
    Task<TournamentSponsor> LinkSponsorToTournamentAsync(int tournamentId, int sponsorId, decimal contractAmount);
    Task UnlinkSponsorFromTournamentAsync(int tournamentId, int sponsorId);
    Task<TournamentSponsor?> GetByIdAsync(int id);
    Task<IEnumerable<TournamentSponsor>> GetAllAsync();
    Task<IEnumerable<TournamentSponsor>> GetByTournamentIdAsync(int tournamentId);
    Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId);
    Task UpdateAsync(TournamentSponsor tournamentSponsor);
}
