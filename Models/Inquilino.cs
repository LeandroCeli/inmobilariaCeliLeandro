using System.ComponentModel.DataAnnotations;

namespace inmobilariaCeli.Models;

public class Inquilino
{
    public int Id { get; set; }

    [Required, StringLength(15)]
    public string DNI { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Apellido { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [EmailAddress, StringLength(100)]
    public string? Email { get; set; }

    [StringLength(25)]
    public string? Telefono { get; set; }

    [StringLength(150)]
    public string? Direccion { get; set; }

    public DateTime FechaAlta { get; set; } = DateTime.UtcNow;
    // Propiedad calculada
    public string NombreCompleto => $"{Nombre} {Apellido}".Trim();

    public bool PuedeEliminar { get; set; }

}
