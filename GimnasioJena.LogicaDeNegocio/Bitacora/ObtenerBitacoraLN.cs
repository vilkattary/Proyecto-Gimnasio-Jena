using GimnasioJena.Abstracciones.AccesoADatos.Bitacora;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Bitacora;
using GimnasioJena.Abstracciones.Modelos.Bitacora;
using GimnasioJena.AccesoADatos.Bitacora;
using System.Collections.Generic;

namespace GimnasioJena.LogicaDeNegocio.Bitacora
{
    public class ObtenerBitacoraLN : IObtenerBitacorasLN
    {
        private readonly IObtenerBitacorasAD _obtenerBitacorasAD;

        public ObtenerBitacoraLN()
        {
            _obtenerBitacorasAD = new ObtenerBitacoraAD();
        }

        public List<BitacoraDto> ObtenerBitacoras()
        {
            return _obtenerBitacorasAD.ObtenerBitacoras();
        }
    }
}