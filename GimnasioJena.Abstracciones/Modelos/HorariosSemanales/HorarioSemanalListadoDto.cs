using System;

namespace GimnasioJena.Abstracciones.Modelos.HorariosSemanales
{
    public class HorarioSemanalListadoDto
    {
        public int idHorario { get; set; }

        public int idTipoClase { get; set; }

        public int idUsuarioEntrenador { get; set; }

        public byte diaSemana { get; set; }

        public string nombreDia { get; set; }

        public string nombreClase { get; set; }

        public string nombreEntrenador { get; set; }

        public TimeSpan horaInicio { get; set; }

        public TimeSpan horaFin { get; set; }

        public int cupoMaximo { get; set; }

        public string ubicacion { get; set; }

        public bool estado { get; set; }

        public string nombreEstado
        {
            get
            {
                return estado ? "Activo" : "Inactivo";
            }
        }

        public DateTime fechaCreacion { get; set; }

        public DateTime? fechaModificacion { get; set; }
    }
}