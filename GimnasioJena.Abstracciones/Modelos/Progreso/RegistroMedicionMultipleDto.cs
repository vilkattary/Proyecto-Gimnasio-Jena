using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.Modelos.Progreso
{
    public class RegistroMedicionMultipleDto
    {
        public string idUsuario { get; set; }
        public string NombreCliente { get; set; }
        public List<ValorMedicionDto> Valores { get; set; } = new List<ValorMedicionDto>();
    }
}
