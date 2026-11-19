using System.ComponentModel.DataAnnotations;
using System.Web;

namespace GimnasioJena.UI.Models
{
    public class EditarPerfilViewModel
    {
        public int idUsuario { get; set; }
        public string identityUserId { get; set; }


        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+(?:[ -][A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+)*$",
            ErrorMessage = "El nombre solo puede contener letras, espacios y guiones.")]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+(?:[ -][A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+)*$",
            ErrorMessage = "El primer apellido solo puede contener letras, espacios y guiones.")]
        [Display(Name = "Primer apellido")]
        public string apellido1 { get; set; }

        [RegularExpression(@"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+(?:[ -][A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+)*$",
            ErrorMessage = "El segundo apellido solo puede contener letras, espacios y guiones.")]
        [Display(Name = "Segundo apellido")]
        public string apellido2 { get; set; }

        [RegularExpression(@"^[0-9]+$",
            ErrorMessage = "El teléfono debe contener únicamente números.")]
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