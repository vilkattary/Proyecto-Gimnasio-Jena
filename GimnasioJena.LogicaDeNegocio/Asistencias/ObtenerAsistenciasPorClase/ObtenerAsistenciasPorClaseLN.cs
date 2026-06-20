using GimnasioJena.Abstracciones.AccesoADatos.Asistencias.ObtenerAsistenciasPorClase;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Asistencias.ObtenerAsistenciasPorClase;
using GimnasioJena.Abstracciones.Modelos.Asistencias;
using GimnasioJena.AccesoADatos.Asistencias.ObtenerAsistenciasPorClase;
using System.Collections.Generic;

namespace GimnasioJena.LogicaDeNegocio.Asistencias.ObtenerAsistenciasPorClase
{
    public class ObtenerAsistenciasPorClaseLN : IObtenerAsistenciasPorClaseLN
    {
        private readonly IObtenerAsistenciasPorClaseAD _obtenerAsistenciasPorClaseAD;

        public ObtenerAsistenciasPorClaseLN()
        {
            _obtenerAsistenciasPorClaseAD = new ObtenerAsistenciasPorClaseAD();
        }

        public List<AsistenciaClaseDto> ObtenerAsistenciasPorClase(int idClaseProgramada)
        {
            if (idClaseProgramada <= 0)
                return new List<AsistenciaClaseDto>();

            return _obtenerAsistenciasPorClaseAD.ObtenerAsistenciasPorClase(idClaseProgramada);
        }
    }
}