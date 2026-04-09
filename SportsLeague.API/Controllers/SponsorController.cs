using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SponsorController : ControllerBase
{
    private readonly ISponsorService _sponsorService;
    private readonly ITournamentSponsorService _tournamentSponsorService;
    private readonly IMapper _mapper;

    public SponsorController(
        ISponsorService sponsorService,
        ITournamentSponsorService tournamentSponsorService,
        IMapper mapper)
    {
        _sponsorService = sponsorService;
        _tournamentSponsorService = tournamentSponsorService;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todos los patrocinadores
    /// </summary>
    /// <returns>Lista de patrocinadores</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SponsorResponseDTO>>> GetAll()
    {
        var sponsors = await _sponsorService.GetAllAsync();
        var sponsorsDto = _mapper.Map<IEnumerable<SponsorResponseDTO>>(sponsors);
        return Ok(sponsorsDto);
    }

    /// <summary>
    /// Obtiene un patrocinador por ID
    /// </summary>
    /// <param name="id">ID del patrocinador</param>
    /// <returns>Patrocinador encontrado</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<SponsorResponseDTO>> GetById(int id)
    {
        var sponsor = await _sponsorService.GetByIdAsync(id);

        if (sponsor == null)
            return NotFound(new { message = $"Patrocinador con ID {id} no encontrado" });

        var sponsorDto = _mapper.Map<SponsorResponseDTO>(sponsor);
        return Ok(sponsorDto);
    }

    /// <summary>
    /// Crea un nuevo patrocinador
    /// </summary>
    /// <param name="dto">Datos del patrocinador a crear</param>
    /// <returns>Patrocinador creado</returns>
    [HttpPost]
    public async Task<ActionResult<SponsorResponseDTO>> Create(SponsorRequestDTO dto)
    {
        try
        {
            var sponsor = _mapper.Map<Sponsor>(dto);
            var createdSponsor = await _sponsorService.CreateAsync(sponsor);
            var responseDto = _mapper.Map<SponsorResponseDTO>(createdSponsor);

            return CreatedAtAction(
                nameof(GetById),
                new { id = responseDto.Id },
                responseDto);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza un patrocinador existente
    /// </summary>
    /// <param name="id">ID del patrocinador a actualizar</param>
    /// <param name="dto">Nuevos datos del patrocinador</param>
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, SponsorRequestDTO dto)
    {
        try
        {
            var sponsor = _mapper.Map<Sponsor>(dto);
            sponsor.Id = id;
            await _sponsorService.UpdateAsync(sponsor);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un patrocinador
    /// </summary>
    /// <param name="id">ID del patrocinador a eliminar</param>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _sponsorService.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene todos los torneos donde patrocina un sponsor
    /// </summary>
    /// <param name="id">ID del patrocinador</param>
    /// <returns>Lista de torneos patrocinados</returns>
    [HttpGet("{id}/tournaments")]
    public async Task<ActionResult<IEnumerable<TournamentSponsorResponseDTO>>> GetTournamentsBySponsor(int id)
    {
        try
        {
            // Validar que el sponsor exista
            var sponsor = await _sponsorService.GetByIdAsync(id);
            if (sponsor == null)
                return NotFound(new { message = $"Patrocinador con ID {id} no encontrado" });

            var tournaments = await _tournamentSponsorService.GetBySponsorIdAsync(id);
            var tournamentsDto = _mapper.Map<IEnumerable<TournamentSponsorResponseDTO>>(tournaments);
            return Ok(tournamentsDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Vincula un sponsor a un torneo
    /// </summary>
    /// <param name="id">ID del patrocinador</param>
    /// <param name="dto">Datos del torneo y contrato</param>
    [HttpPost("{id}/tournaments")]
    public async Task<ActionResult<TournamentSponsorResponseDTO>> LinkToTournament(int id, LinkSponsorToTournamentDTO dto)
    {
        try
        {
            // Validar que el sponsor exista
            var sponsor = await _sponsorService.GetByIdAsync(id);
            if (sponsor == null)
                return NotFound(new { message = $"Patrocinador con ID {id} no encontrado" });

            var linkedTournament = await _tournamentSponsorService
                .LinkSponsorToTournamentAsync(dto.TournamentId, id, dto.ContractAmount);

            var responseDto = _mapper.Map<TournamentSponsorResponseDTO>(linkedTournament);

            return CreatedAtAction(
                nameof(GetTournamentsBySponsor),
                new { id = id },
                responseDto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Desvincula un sponsor de un torneo
    /// </summary>
    /// <param name="id">ID del patrocinador</param>
    /// <param name="tournamentId">ID del torneo</param>
    [HttpDelete("{id}/tournaments/{tournamentId}")]
    public async Task<ActionResult> UnlinkFromTournament(int id, int tournamentId)
    {
        try
        {
            // Validar que el sponsor exista
            var sponsor = await _sponsorService.GetByIdAsync(id);
            if (sponsor == null)
                return NotFound(new { message = $"Patrocinador con ID {id} no encontrado" });

            await _tournamentSponsorService.UnlinkSponsorFromTournamentAsync(tournamentId, id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
