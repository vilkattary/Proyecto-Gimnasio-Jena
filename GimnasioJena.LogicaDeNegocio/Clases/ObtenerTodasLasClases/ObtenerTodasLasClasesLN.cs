using GimnasioJena.Abstracciones.AccesoADatos.Clases.ObtenerTodasLasClases;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.ObtenerTodasLasClases;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.AccesoADatos.Clases.ObtenerTodasLasClases;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.LogicaDeNegocio.Clases.ObtenerTodasLasClases
{
    public class ObtenerTodasLasClasesLN : IObtenerTodasLasClasesLN
    {
        private IObtenerTodasLasClasesAD _obtenerTodasLasClasesAD;

        public ObtenerTodasLasClasesLN()
        {
            _obtenerTodasLasClasesAD = new ObtenerTodasLasClasesAD();
        }

        public List<ClaseListadoDto> ObtenerTodasLasClases()
        {
            List<ClaseListadoDto> laListaDeClases = _obtenerTodasLasClasesAD.ObtenerTodasLasClases();
            laListaDeClases = laListaDeClases.OrderBy(clase => clase.nombreClase).ToList();

            return laListaDeClases;
        }
    }
}
