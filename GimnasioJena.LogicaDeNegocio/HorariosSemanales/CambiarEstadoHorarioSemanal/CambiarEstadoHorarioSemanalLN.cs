using GimnasioJena.Abstracciones.AccesoADatos
    .HorariosSemanales.CambiarEstadoHorarioSemanal;
using GimnasioJena.Abstracciones.LogicaDeNegocio
    .HorariosSemanales.CambiarEstadoHorarioSemanal;
using GimnasioJena.AccesoADatos.HorariosSemanales
    .CambiarEstadoHorarioSemanal;

namespace GimnasioJena.LogicaDeNegocio.HorariosSemanales
    .CambiarEstadoHorarioSemanal
{
    public class CambiarEstadoHorarioSemanalLN
        : ICambiarEstadoHorarioSemanalLN
    {
        private readonly ICambiarEstadoHorarioSemanalAD
            _cambiarEstadoHorarioSemanalAD;

        public CambiarEstadoHorarioSemanalLN()
        {
            _cambiarEstadoHorarioSemanalAD =
                new CambiarEstadoHorarioSemanalAD();
        }

        public bool CambiarEstadoHorarioSemanal(
            int idHorario
        )
        {
            return _cambiarEstadoHorarioSemanalAD
                .CambiarEstadoHorarioSemanal(idHorario);
        }
    }
}