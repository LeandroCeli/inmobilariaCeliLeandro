using System.ComponentModel.DataAnnotations;

namespace inmobilariaCeli.Models;

public class Contrato
{
    public int Id { get; set; }

    [Required]
    public int IdPropiedad { get; set; }

    [Required]
    public int IdInquilino { get; set; }

    [DataType(DataType.Date)]
    public DateTime FechaInicio { get; set; }

    [DataType(DataType.Date)]
    public DateTime FechaFin { get; set; }

    [Range(0, double.MaxValue)]
    public decimal MontoMensual { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Deposito { get; set; }
}
