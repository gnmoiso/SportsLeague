namespace SportsLeague.Domain.Interfaces.Services;

public interface IStandingsService

{
    Task<object> GetStandingsAsync(int tournamentId); // obtener la tabla de posiciones del torneo

    Task<object> GetTopScorersAsync(int tournamentId); // obtener la lista de los máximos goleadores del torneo

    Task<object> GetCardStatsAsync(int tournamentId); // obtener las estadisticas de tarjetas para el torneo
}