using System.ComponentModel.DataAnnotations;

namespace GimnasioJena.Abstracciones.Modelos.Contacto
{
    public class ContactoFormDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [StringLength(100, ErrorMessage = "El correo no puede superar los 100 caracteres.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El asunto es obligatorio.")]
        [StringLength(150, ErrorMessage = "El asunto no puede superar los 150 caracteres.")]
        public string Asunto { get; set; }

        [Required(ErrorMessage = "El mensaje es obligatorio.")]
        [StringLength(1000, ErrorMessage = "El mensaje no puede superar los 1000 caracteres.")]
        public string Mensaje { get; set; }
    }
}
