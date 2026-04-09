using AutoMapper;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;

namespace SportsLeague.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Team mappings
        _ = CreateMap<TeamRequestDTO, Team>();
        _ = CreateMap<Team, TeamResponseDTO>();

        // Player mappings
        _ = CreateMap<PlayerRequestDTO, Player>();
        _ = CreateMap<Player, PlayerResponseDTO>()
            .ForMember(
                dest => dest.TeamName,
                opt => opt.MapFrom(src => src.Team.Name));

        // Referee mappings
        _ = CreateMap<RefereeRequestDTO, Referee>();
        _ = CreateMap<Referee, RefereeResponseDTO>();

        // Tournament mappings
        _ = CreateMap<TournamentRequestDTO, Tournament>();
        _ = CreateMap<Tournament, TournamentResponseDTO>()
            .ForMember(
                dest => dest.TeamsCount,
                opt => opt.MapFrom(src =>
                    src.TournamentTeams != null ? src.TournamentTeams.Count : 0));

        // Sponsor mappings
        _ = CreateMap<SponsorRequestDTO, Sponsor>();
        _ = CreateMap<Sponsor, SponsorResponseDTO>();

        // TournamentSponsor mappings
        _ = CreateMap<TournamentSponsorRequestDTO, TournamentSponsor>();
        _ = CreateMap<TournamentSponsor, TournamentSponsorResponseDTO>()
            .ForMember(
                dest => dest.SponsorName,
                opt => opt.MapFrom(src => src.Sponsor.Name))
            .ForMember(
                dest => dest.SponsorContactEmail,
                opt => opt.MapFrom(src => src.Sponsor.ContactEmail));

    }
}


