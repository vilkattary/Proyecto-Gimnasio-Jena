using System;

namespace GimnasioJena.Abstracciones.Modelos.Reportes
{
    public class ReporteClienteDto
    {
        public int idUsuario { get; set; }
        public string nombreCompleto { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string estadoUsuario { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}