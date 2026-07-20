using System;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.Modelos.Clases
{
    public class ClaseCrearDto
    {
        public int    idTipoClase           { get; set; }
        public string nombreTipoClase       { get; set; }
        public int    idEstadoClase         { get; set; }
        public int    cupoMaximo            { get; set; }
        public string observaciones         { get; set; }
        public DateTime fechaCreacion       { get; set; }

        // campos heredados usados por EditarClase (backward compat)
        public int      idUsuarioEntrenador { get; set; }
        public DateTime fechaClase          { get; set; }
        public TimeSpan horaInicio          { get; set; }
        public TimeSpan horaFin             { get; set; }
        public string   ubicacion           { get; set; }

        // lista de horarios para creación múltiple
        public List<HorarioItemDto> Horarios { get; set; }
    }
}