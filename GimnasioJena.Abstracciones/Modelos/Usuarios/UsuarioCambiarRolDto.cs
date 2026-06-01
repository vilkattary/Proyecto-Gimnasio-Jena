namespace GimnasioJena.Abstracciones.Modelos.Usuarios
{
    public class UsuarioCambiarRolDto
    {
        public int idUsuario { get; set; }
        public string identityUserId { get; set; }
        public string rolActual { get; set; }
        public string rolNuevo { get; set; }

        // Opcional para bitácora
        public int? idUsuarioAdministrador { get; set; }
    }
}