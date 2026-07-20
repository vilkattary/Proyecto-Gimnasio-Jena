using GimnasioJena.Abstracciones.Modelos.Bitacora;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.AccesoADatos.Bitacora
{
    public interface IObtenerBitacorasAD
    {
        List<BitacoraDto> ObtenerBitacoras();
    }
}