using System;

namespace GimnasioJena.Abstracciones.Modelos.Clases
{
    public class ClaseCrearDto
    {

        public int idClaseProgramada { get; set; }
        public string nombreClase { get; set; }
        public string tipoClase { get; set; }
        public string nombreEntrenador { get; set; }
        public bool estadoClase { get; set; }
        public string estadoTexto => estadoClase ? "Activo" : "Inactivo";
        public DateTime fechaClase { get; set; }
        public TimeSpan horaInicio { get; set; }
        public TimeSpan horaFin { get; set; }
        public int cupoMaximo { get; set; }
        public string ubicacion { get; set; }
        public string observaciones { get; set; }
        public DateTime fechaCreacion { get; set; }
    }
}