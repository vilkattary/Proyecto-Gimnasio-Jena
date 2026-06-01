using System.ComponentModel.DataAnnotations;

namespace GimnasioJena.UI.Models
{
    public class EditarPerfilViewModel
    {
        public int idUsuario { get; set; }
        public string identityUserId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [Display(Name = "Primer apellido")]
        public string apellido1 { get; set; }

        [Display(Name = "Segundo apellido")]
        public string apellido2 { get; set; }

        [Phone(ErrorMessage = "Ingrese un número de teléfono válido.")]
        [Display(Name = "Teléfono")]
        public string telefono { get; set; }

        public string correo { get; set; }
        public string identificacion { get; set; }
    }
}