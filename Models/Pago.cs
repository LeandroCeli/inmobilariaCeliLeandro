using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobilariaCeli.Models
{
    public class Pago
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Contrato")]
        public int IdContrato { get; set; }

        [Display(Name = "Número de Pago")]
        [Required(ErrorMessage = "Debe indicar el número de pago.")]
        public int NumeroPago { get; set; }

        [Required(ErrorMessage = "Debe indicar la fecha de pago.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Pago")]
        public DateTime FechaPago { get; set; }

        [StringLength(200)]
        public string? Detalle { get; set; }

        [Required(ErrorMessage = "Debe ingresar el importe.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El importe debe ser mayor que 0.")]
        [Display(Name = "Importe")]
        public decimal Importe { get; set; }

        [Display(Name = "Pagado")]
        public bool Pagado { get; set; } = false;

        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Display(Name = "Usuario que registró")]
        public string? UsuarioRegistro { get; set; }

        // 🔗 Relación con Contrato
        [ForeignKey("IdContrato")]
        public Contrato? Contrato { get; set; }

        // Propiedades auxiliares (para mostrar en vista)
        [NotMapped]
        [Display(Name = "Inquilino")]
        public string? InquilinoNombre { get; set; }

        [NotMapped]
        [Display(Name = "Dirección del Inmueble")]
        public string? PropiedadDireccion { get; set; }



    }
}
