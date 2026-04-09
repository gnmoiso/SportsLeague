using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class SponsorService : ISponsorService
{
    private readonly ILogger<SponsorService> _logger;
    private readonly ISponsorRepository _sponsorRepository;

    public SponsorService(ILogger<SponsorService> logger, ISponsorRepository sponsorRepository)
    {
        _logger = logger;
        _sponsorRepository = sponsorRepository;
    }

    public async Task<Sponsor> CreateAsync(Sponsor sponsor)
    {
        try
        {
            _logger.LogInformation("Creando nuevo sponsor: {SponsorName}", sponsor.Name);

            // Validar que no exista un sponsor con el mismo nombre
            var existingSponsor = await _sponsorRepository.ExistByNameAsync(sponsor.Name);
            if (existingSponsor != null)
            {
                _logger.LogWarning("Sponsor con nombre {SponsorName} ya existe", sponsor.Name);
                throw new InvalidOperationException($"Ya existe un sponsor con el nombre '{sponsor.Name}'");
            }

            var createdSponsor = await _sponsorRepository.CreateAsync(sponsor);
            _logger.LogInformation("Sponsor creado exitosamente con ID: {SponsorId}", createdSponsor.Id);
            return createdSponsor;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear sponsor: {SponsorName}", sponsor.Name);
            throw;
        }
    }

    public async Task<IEnumerable<Sponsor>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Obteniendo todos los sponsors");
            return await _sponsorRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los sponsors");
            throw;
        }
    }

    public async Task<Sponsor?> GetByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Obteniendo sponsor con ID: {SponsorId}", id);
            return await _sponsorRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener sponsor con ID: {SponsorId}", id);
            throw;
        }
    }

    public async Task UpdateAsync(Sponsor entity)
    {
        try
        {
            _logger.LogInformation("Actualizando sponsor con ID: {SponsorId}", entity.Id);

            // Validar que el sponsor existe
            var existingSponsor = await _sponsorRepository.GetByIdAsync(entity.Id);
            if (existingSponsor == null)
            {
                _logger.LogWarning("Sponsor con ID {SponsorId} no encontrado", entity.Id);
                throw new InvalidOperationException($"Sponsor con ID '{entity.Id}' no existe");
            }

            await _sponsorRepository.UpdateAsync(entity);
            _logger.LogInformation("Sponsor con ID {SponsorId} actualizado exitosamente", entity.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar sponsor con ID: {SponsorId}", entity.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            _logger.LogInformation("Eliminando sponsor con ID: {SponsorId}", id);

            // Validar que el sponsor existe
            var sponsor = await _sponsorRepository.GetByIdAsync(id);
            if (sponsor == null)
            {
                _logger.LogWarning("Sponsor con ID {SponsorId} no encontrado", id);
                throw new InvalidOperationException($"Sponsor con ID '{id}' no existe");
            }

            await _sponsorRepository.DeleteAsync(id);
            _logger.LogInformation("Sponsor con ID {SponsorId} eliminado exitosamente", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar sponsor con ID: {SponsorId}", id);
            throw;
        }
    }

    public async Task<Sponsor?> ExistByNameAsync(string name)
    {
        try
        {
            _logger.LogInformation("Buscando sponsor por nombre: {SponsorName}", name);
            return await _sponsorRepository.ExistByNameAsync(name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar sponsor por nombre: {SponsorName}", name);
            throw;
        }
    }

    public async Task<IEnumerable<Sponsor>> ExistByNameAsync(IEnumerable<string> names)
    {
        try
        {
            _logger.LogInformation("Buscando sponsors por nombres");
            return await _sponsorRepository.ExistByNameAsync(names);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar sponsors por nombres");
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        try
        {
            _logger.LogInformation("Verificando existencia de sponsor con ID: {SponsorId}", id);
            var sponsor = await _sponsorRepository.GetByIdAsync(id);
            return sponsor != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia de sponsor con ID: {SponsorId}", id);
            throw;
        }
    }

    public async Task DeleteAsync()
    {
        _logger.LogWarning("Intento de eliminar todos los sponsors");
        throw new NotImplementedException("No se permite eliminar todos los sponsors");
    }

    public async Task<IEnumerable<Sponsor>> UpdateAsync()
    {
        _logger.LogWarning("Intento de actualizar sin especificar sponsor");
        throw new NotImplementedException("Debe especificar el sponsor a actualizar");
    }
}
