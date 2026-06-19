using GimnasioJena.Abstracciones.AccesoADatos.Reservas.ObtenerReservasPorClase;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerReservasPorClase;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.AccesoADatos.Reservas.ObtenerReservasPorClase;
using System.Collections.Generic;

namespace GimnasioJena.LogicaDeNegocio.Reservas.ObtenerReservasPorClase
{
    public class ObtenerReservasPorClaseLN : IObtenerReservasPorClaseLN
    {
        private readonly IObtenerReservasPorClaseAD _obtenerReservasPorClaseAD;

        public ObtenerReservasPorClaseLN()
        {
            _obtenerReservasPorClaseAD = new ObtenerReservasPorClaseAD();
        }

        public List<ReservaClaseDto> ObtenerReservasPorClase(int idClaseProgramada)
        {
            if (idClaseProgramada <= 0)
                return new List<ReservaClaseDto>();

            return _obtenerReservasPorClaseAD.ObtenerReservasPorClase(idClaseProgramada);
        }
    }
}