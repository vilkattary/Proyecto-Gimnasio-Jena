using System;

namespace GimnasioJena.Abstracciones.Modelos.Usuarios
{
    public class UsuarioListadoDto
    {
        public int idUsuario { get; set; }
        public string identityUserId { get; set; }
        public string nombreCompleto { get; set; }
        public string identificacion { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string rol { get; set; }
        public DateTime fechaRegistro { get; set; }
        public bool estado { get; set; }
    }
}