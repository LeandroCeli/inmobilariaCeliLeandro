using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Web.Models;

public class Inquilino
{
    public int Id { get; set; }

    [Required, StringLength(15)]
    public string DNI { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string NombreCompleto { get; set; } = string.Empty;

    [EmailAddress, StringLength(100)]
    public string? Email { get; set; }

    [StringLength(25)]
    public string? Telefono { get; set; }

    [StringLength(150)]
    public string? Direccion { get; set; }

    public DateTime FechaAlta { get; set; } = DateTime.UtcNow;
}
