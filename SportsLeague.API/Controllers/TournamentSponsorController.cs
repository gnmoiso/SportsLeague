using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/tournaments/{tournamentId}/sponsors")]
public class TournamentSponsorController : ControllerBase
{
    private readonly ITournamentSponsorService _tournamentSponsorService;
    private readonly IMapper _mapper;

    public TournamentSponsorController(
        ITournamentSponsorService tournamentSponsorService,
        IMapper mapper)
    {
        _tournamentSponsorService = tournamentSponsorService;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todos los patrocinadores de un torneo
    /// </summary>
    /// <param name="tournamentId">ID del torneo</param>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TournamentSponsorResponseDTO>>> GetSponsorsByTournament(int tournamentId)
    {
        try
        {
            var sponsors = await _tournamentSponsorService.GetByTournamentIdAsync(tournamentId);
            var sponsorsDto = _mapper.Map<IEnumerable<TournamentSponsorResponseDTO>>(sponsors);
            return Ok(sponsorsDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene una vinculación específica entre torneo y sponsor
    /// </summary>
    /// <param name="tournamentId">ID del torneo</param>
    /// <param name="id">ID de la vinculación</param>
    [HttpGet("{id}")]
    public async Task<ActionResult<TournamentSponsorResponseDTO>> GetById(int tournamentId, int id)
    {
        try
        {
            var tournamentSponsor = await _tournamentSponsorService.GetByIdAsync(id);

            if (tournamentSponsor == null || tournamentSponsor.TournamentId != tournamentId)
                return NotFound(new { message = "Vinculación no encontrada" });

            var dto = _mapper.Map<TournamentSponsorResponseDTO>(tournamentSponsor);
            return Ok(dto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Vincula un patrocinador a un torneo
    /// </summary>
    /// <param name="tournamentId">ID del torneo</param>
    /// <param name="dto">Datos del patrocinador a vincular</param>
    [HttpPost]
    public async Task<ActionResult<TournamentSponsorResponseDTO>> LinkSponsor(
        int tournamentId, LinkSponsorRequestDTO dto)
    {
        try
        {
            var linkedSponsor = await _tournamentSponsorService
                .LinkSponsorToTournamentAsync(tournamentId, dto.SponsorId, dto.ContractAmount);

            var responseDto = _mapper.Map<TournamentSponsorResponseDTO>(linkedSponsor);

            return CreatedAtAction(
                nameof(GetById),
                new { tournamentId = tournamentId, id = responseDto.Id },
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
    /// Actualiza el monto del contrato de una vinculación
    /// </summary>
    /// <param name="tournamentId">ID del torneo</param>
    /// <param name="id">ID de la vinculación</param>
    /// <param name="dto">Nuevos datos de la vinculación</param>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateContractAmount(int tournamentId, int id, TournamentSponsorRequestDTO dto)
    {
        try
        {
            var tournamentSponsor = await _tournamentSponsorService.GetByIdAsync(id);

            if (tournamentSponsor == null || tournamentSponsor.TournamentId != tournamentId)
                return NotFound(new { message = "Vinculación no encontrada" });

            tournamentSponsor.ContractAmount = dto.ContractAmount;
            await _tournamentSponsorService.UpdateAsync(tournamentSponsor);

            return NoContent();
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
    /// Desvincula un patrocinador de un torneo
    /// </summary>
    /// <param name="tournamentId">ID del torneo</param>
    /// <param name="sponsorId">ID del patrocinador</param>
    [HttpDelete("{sponsorId}")]
    public async Task<ActionResult> UnlinkSponsor(int tournamentId, int sponsorId)
    {
        try
        {
            await _tournamentSponsorService.UnlinkSponsorFromTournamentAsync(tournamentId, sponsorId);
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
