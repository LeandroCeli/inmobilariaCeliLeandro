using System.ComponentModel.DataAnnotations;

namespace inmobilariaCeli.Models;

public class Inmueble
{

    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Direccion { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Tipo { get; set; } = string.Empty;

    [Required]
    public String Uso { get; set; }

    [Range(1, 20)]
    public int Ambientes { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Precio { get; set; }

    [Required]
    public int IdPropietario { get; set; }

    public string PropietarioNombre { get; set; } = string.Empty;
    
     public bool Disponible { get; set; }
}