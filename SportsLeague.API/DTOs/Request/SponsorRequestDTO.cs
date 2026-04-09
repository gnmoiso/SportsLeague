using SportsLeague.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SportsLeague.API.DTOs.Request;

public class SponsorRequestDTO
{
    [Required(ErrorMessage = "El nombre del patrocinador es requerido")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email de contacto es requerido")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    public string ContactEmail { get; set; } = string.Empty;

    [Phone(ErrorMessage = "El teléfono no es válido")]
    public string? Phone { get; set; }

    [Url(ErrorMessage = "La URL del sitio web no es válida")]
    public string? WebsiteUrl { get; set; }

    [Required(ErrorMessage = "La categoría del patrocinador es requerida")]
    public SponsorCategory Category { get; set; } = SponsorCategory.Main;
}
