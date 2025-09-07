using System.ComponentModel.DataAnnotations;

namespace inmobilariaCeli.Models;

public class Propiedad
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Direccion { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Tipo { get; set; } = string.Empty;  // Casa, Departamento...

    [Required, StringLength(50)]
    public string Uso { get; set; } = string.Empty;   // Comercial, Residencial...

    [Range(1, 20)]
    public int Ambientes { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Precio { get; set; }

    [Required]
    public int IdPropietario { get; set; }
}
