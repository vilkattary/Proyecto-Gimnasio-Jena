using System.ComponentModel.DataAnnotations;
using System.Web;

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

        [Display(Name = "Dirección")]
        [StringLength(300, ErrorMessage = "La dirección no puede superar los 300 caracteres.")]
        public string direccion { get; set; }

        public string fotoPerfil { get; set; }

        [Display(Name = "Foto de perfil")]
        public HttpPostedFileBase archivoFotoPerfil { get; set; }

        public string correo { get; set; }
        public string identificacion { get; set; }
    }
}