using GimnasioJena.Abstracciones.AccesoADatos
    .HorariosSemanales.CambiarEstadoHorarioSemanal;
using GimnasioJena.Abstracciones.General.Fechas;
using GimnasioJena.Abstracciones.LogicaDeNegocio
    .HorariosSemanales.CambiarEstadoHorarioSemanal;
using GimnasioJena.AccesoADatos.HorariosSemanales
    .CambiarEstadoHorarioSemanal;
using GimnasioJena.LogicaDeNegocio.General.Fechas;
using System;

namespace GimnasioJena.LogicaDeNegocio.HorariosSemanales
    .CambiarEstadoHorarioSemanal
{
    public class CambiarEstadoHorarioSemanalLN
        : ICambiarEstadoHorarioSemanalLN
    {
        private readonly ICambiarEstadoHorarioSemanalAD
            _cambiarEstadoHorarioSemanalAD;
        private readonly IFechasLN _fechasLN;

        public CambiarEstadoHorarioSemanalLN()
        {
            _cambiarEstadoHorarioSemanalAD =
                new CambiarEstadoHorarioSemanalAD();
            _fechasLN = new FechasLN();
        }

        public bool CambiarEstadoHorarioSemanal(int idHorario)
        {
            DateTime fechaActual = _fechasLN.ObtenerFechaActual();

            return _cambiarEstadoHorarioSemanalAD
                .CambiarEstadoHorarioSemanal(
                    idHorario,
                    fechaActual
                );
        }
    }
}