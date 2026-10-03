using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GimnasioJena.Abstracciones.Modelos.Progreso;

namespace GimnasioJena.Abstracciones.AccesoADatos.Progreso
{
    public interface IProgresoAD
    {
        Task<List<ProgresoDiaDto>> ObtenerProgresoPorRangoFechaAD(int idUsuario, DateTime fechaInicio, DateTime fechaFin);

        Task GuardarProgresoAD(ProgresoDiaDto progreso);
    }
}
