using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class TournamentSponsorService : ITournamentSponsorService
{
    private readonly ILogger<TournamentSponsorService> _logger;
    private readonly ITournamentSponsorRepository _tournamentSponsorRepository;
    private readonly ITournamentRepository _tournamentRepository;
    private readonly ISponsorRepository _sponsorRepository;

    public TournamentSponsorService(
        ILogger<TournamentSponsorService> logger,
        ITournamentSponsorRepository tournamentSponsorRepository,
        ITournamentRepository tournamentRepository,
        ISponsorRepository sponsorRepository)
    {
        _logger = logger;
        _tournamentSponsorRepository = tournamentSponsorRepository;
        _tournamentRepository = tournamentRepository;
        _sponsorRepository = sponsorRepository;
    }

    public async Task<TournamentSponsor> LinkSponsorToTournamentAsync(int tournamentId, int sponsorId, decimal contractAmount)
    {
        try
        {
            _logger.LogInformation("Vinculando sponsor {SponsorId} a torneo {TournamentId} con contrato {ContractAmount}", 
                sponsorId, tournamentId, contractAmount);

            // Validación 1: Validar que el monto del contrato sea mayor a 0
            if (contractAmount <= 0)
            {
                _logger.LogWarning("Monto de contrato inválido: {ContractAmount}", contractAmount);
                throw new InvalidOperationException("El monto del contrato debe ser mayor a 0");
            }

            // Validación 2: Validar que el torneo exista
            var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
            if (tournament == null)
            {
                _logger.LogWarning("Torneo con ID {TournamentId} no encontrado", tournamentId);
                throw new KeyNotFoundException($"Torneo con ID '{tournamentId}' no existe");
            }

            // Validación 3: Validar que el sponsor exista
            var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);
            if (sponsor == null)
            {
                _logger.LogWarning("Patrocinador con ID {SponsorId} no encontrado", sponsorId);
                throw new KeyNotFoundException($"Patrocinador con ID '{sponsorId}' no existe");
            }

            // Validación 4: Validar que no esté duplicada la vinculación
            var existingLink = await _tournamentSponsorRepository
                .GetByTournamentAndSponsorAsync(tournamentId, sponsorId);
            if (existingLink != null)
            {
                _logger.LogWarning("Sponsor {SponsorId} ya está vinculado al torneo {TournamentId}", sponsorId, tournamentId);
                throw new InvalidOperationException(
                    $"El patrocinador '{sponsor.Name}' ya está vinculado al torneo '{tournament.Name}'");
            }

            // Crear la vinculación
            var tournamentSponsor = new TournamentSponsor
            {
                TournamentId = tournamentId,
                SponsorId = sponsorId,
                ContractAmount = contractAmount
            };

            var createdLink = await _tournamentSponsorRepository.CreateAsync(tournamentSponsor);
            _logger.LogInformation("Sponsor {SponsorId} vinculado exitosamente al torneo {TournamentId}", 
                sponsorId, tournamentId);

            return createdLink;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al vincular sponsor {SponsorId} a torneo {TournamentId}", 
                sponsorId, tournamentId);
            throw;
        }
    }

    public async Task UnlinkSponsorFromTournamentAsync(int tournamentId, int sponsorId)
    {
        try
        {
            _logger.LogInformation("Desvinculando sponsor {SponsorId} del torneo {TournamentId}", 
                sponsorId, tournamentId);

            // Obtener la vinculación
            var tournamentSponsor = await _tournamentSponsorRepository
                .GetByTournamentAndSponsorAsync(tournamentId, sponsorId);

            if (tournamentSponsor == null)
            {
                _logger.LogWarning("Vinculación no encontrada entre sponsor {SponsorId} y torneo {TournamentId}", 
                    sponsorId, tournamentId);
                throw new KeyNotFoundException(
                    $"No existe una vinculación entre el patrocinador y el torneo especificados");
            }

            // Eliminar la vinculación
            await _tournamentSponsorRepository.DeleteAsync(tournamentSponsor.Id);
            _logger.LogInformation("Sponsor {SponsorId} desvinculado exitosamente del torneo {TournamentId}", 
                sponsorId, tournamentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al desvincular sponsor {SponsorId} del torneo {TournamentId}", 
                sponsorId, tournamentId);
            throw;
        }
    }

    public async Task<TournamentSponsor?> GetByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Obteniendo vinculación con ID: {Id}", id);
            return await _tournamentSponsorRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener vinculación con ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TournamentSponsor>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Obteniendo todas las vinculaciones de sponsors con torneos");
            return await _tournamentSponsorRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las vinculaciones");
            throw;
        }
    }

    public async Task<IEnumerable<TournamentSponsor>> GetByTournamentIdAsync(int tournamentId)
    {
        try
        {
            _logger.LogInformation("Obteniendo sponsors del torneo {TournamentId}", tournamentId);
            return await _tournamentSponsorRepository.GetByTournamentIdAsync(tournamentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener sponsors del torneo {TournamentId}", tournamentId);
            throw;
        }
    }

    public async Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId)
    {
        try
        {
            _logger.LogInformation("Obteniendo torneos del patrocinador {SponsorId}", sponsorId);
            return await _tournamentSponsorRepository.GetBySponsorIdAsync(sponsorId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener torneos del patrocinador {SponsorId}", sponsorId);
            throw;
        }
    }

    public async Task UpdateAsync(TournamentSponsor tournamentSponsor)
    {
        try
        {
            _logger.LogInformation("Actualizando vinculación ID: {Id}", tournamentSponsor.Id);

            // Validación: Validar que el monto sea mayor a 0
            if (tournamentSponsor.ContractAmount <= 0)
            {
                _logger.LogWarning("Monto de contrato inválido: {ContractAmount}", tournamentSponsor.ContractAmount);
                throw new InvalidOperationException("El monto del contrato debe ser mayor a 0");
            }

            await _tournamentSponsorRepository.UpdateAsync(tournamentSponsor);
            _logger.LogInformation("Vinculación ID: {Id} actualizada exitosamente", tournamentSponsor.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar vinculación ID: {Id}", tournamentSponsor.Id);
            throw;
        }
    }
}
