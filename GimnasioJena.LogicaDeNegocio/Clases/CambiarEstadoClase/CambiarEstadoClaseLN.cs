using GimnasioJena.Abstracciones.AccesoADatos.Clases.CambiarEstadoClase;
using GimnasioJena.Abstracciones.General.Fechas;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.CambiarEstadoClase;
using GimnasioJena.AccesoADatos.Clases.CambiarEstadoClase;
using GimnasioJena.LogicaDeNegocio.General.Fechas;
using System;

namespace GimnasioJena.LogicaDeNegocio.Clases.CambiarEstadoClase
{
    public class CambiarEstadoClaseLN : ICambiarEstadoClaseLN
    {
        private readonly ICambiarEstadoClaseAD
            _cambiarEstadoClaseAD;
        private readonly IFechasLN _fechasLN;

        public CambiarEstadoClaseLN()
        {
            _cambiarEstadoClaseAD =
                new CambiarEstadoClaseAD();
            _fechasLN = new FechasLN();
        }

        public bool CambiarEstadoClase(int idClaseProgramada)
        {
            DateTime fechaActual = _fechasLN.ObtenerFechaActual();

            return _cambiarEstadoClaseAD.CambiarEstadoClase(
                idClaseProgramada,
                fechaActual
            );
        }
    }
}
