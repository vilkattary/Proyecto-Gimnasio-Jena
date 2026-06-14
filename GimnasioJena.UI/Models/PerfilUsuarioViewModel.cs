using System;
using System.ComponentModel.DataAnnotations;

namespace GimnasioJena.UI.Models
{
    public class PerfilUsuarioViewModel
    {
        public int idUsuario { get; set; }
        public string identityUserId { get; set; }

        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Display(Name = "Primer apellido")]
        public string apellido1 { get; set; }

        [Display(Name = "Segundo apellido")]
        public string apellido2 { get; set; }

        public string nombreCompleto { get; set; }

        [Display(Name = "Identificación")]
        public string identificacion { get; set; }

        [Display(Name = "Correo electrónico")]
        public string correo { get; set; }

        [Display(Name = "Teléfono")]
        public string telefono { get; set; }

        [Display(Name = "Dirección")]
        public string direccion { get; set; }

        public string fotoPerfil { get; set; }

        public string rol { get; set; }
        public bool estado { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}