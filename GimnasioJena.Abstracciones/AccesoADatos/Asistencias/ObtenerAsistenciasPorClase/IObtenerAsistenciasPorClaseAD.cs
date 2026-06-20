using GimnasioJena.Abstracciones.Modelos.Asistencias;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.AccesoADatos.Asistencias.ObtenerAsistenciasPorClase
{
    public interface IObtenerAsistenciasPorClaseAD
    {
        List<AsistenciaClaseDto> ObtenerAsistenciasPorClase(int idClaseProgramada);
    }
}