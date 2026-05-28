namespace GimnasioJena.Abstracciones.Modelos.Usuarios
{
    public class LoginDto
    {
        public string correo { get; set; }
        public string contrasena { get; set; }
        public bool recordarSesion { get; set; }
    }
}