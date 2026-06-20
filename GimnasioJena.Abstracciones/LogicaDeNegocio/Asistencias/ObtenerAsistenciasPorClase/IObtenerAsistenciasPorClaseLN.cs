using GimnasioJena.Abstracciones.Modelos.Asistencias;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Asistencias.ObtenerAsistenciasPorClase
{
    public interface IObtenerAsistenciasPorClaseLN
    {
        List<AsistenciaClaseDto> ObtenerAsistenciasPorClase(int idClaseProgramada);
    }
}