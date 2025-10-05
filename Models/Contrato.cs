using System.ComponentModel.DataAnnotations;

namespace inmobilariaCeli.Models
{
    public class Contrato
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La propiedad es obligatoria.")]
        [Display(Name = "Propiedad")]
        public int IdPropiedad { get; set; }

        [Required(ErrorMessage = "El inquilino es obligatorio.")]
        [Display(Name = "Inquilino")]
        public int IdInquilino { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de fin")]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "El tipo de ocupación es obligatorio.")]
        [Display(Name = "Tipo de ocupación")]
        public TipoOcupacion TipoOcupacion { get; set; }

        [Required(ErrorMessage = "El monto mensual es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto debe ser positivo.")]
        [Display(Name = "Monto mensual")]
        public decimal MontoMensual { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El depósito debe ser positivo.")]
        [Display(Name = "Depósito")]
        public decimal Deposito { get; set; }

        // Propiedades auxiliares para mostrar en vistas
        public string PropiedadDireccion { get; set; } = "Sin dirección";
        public string InquilinoNombre { get; set; } = "Sin nombre";

        // Propiedad calculada para saber si el contrato está vigente
        public bool Vigente => FechaFin >= DateTime.Today;
    }
}