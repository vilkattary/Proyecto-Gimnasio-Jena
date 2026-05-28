namespace GimnasioJena.Abstracciones.Modelos.Usuarios
{
    public class UsuarioEditarDto
    {
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public string identificacion { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public bool estado { get; set; }
    }
}