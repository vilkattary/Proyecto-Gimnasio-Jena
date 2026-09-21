using System.Collections.Generic;
using System.Threading.Tasks;
using GimnasioJena.AccesoADatos.Entidades.Progreso;
using System;

namespace GimnasioJena.AccesoADatos.Progreso
{
    public interface IProgresoAvanzadoAD
    {
        Task GuardarProgresoMultimodalAD(ProgresoClienteEntidad progreso);

        Task GuardarProgresoClienteDetalleAD(List<ProgresoClienteEntidad> lista);

        Task<List<ProgresoClienteEntidad>> ObtenerProgresoRangoAD(int idUsuario, DateTime fechaInicio, DateTime fechaFin);

        Task<Dictionary<string, int>> ObtenerMatrizAsistenciaAnualAD(int idUsuario, int anio);
    }
}
