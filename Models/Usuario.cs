using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobilariaCeli.Models
{

    public enum RolUsuario
    {
        Administrador,
        Empleado
    }



    public class Usuario
    {
        public enum RolUsuario
        {
            Administrador,
            Empleado
        }

        public int Id { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato inválido.")]
        public string Email { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public RolUsuario Rol { get; set; }

        public string? FotoPerfil { get; set; }
    }
}