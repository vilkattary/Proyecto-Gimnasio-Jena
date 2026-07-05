using GimnasioJena.Abstracciones.Modelos.Bitacora;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Bitacora
{
    public interface IObtenerBitacorasLN
    {
        List<BitacoraDto> ObtenerBitacoras();
    }
}