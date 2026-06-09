using GimnasioJena.Abstracciones.AccesoADatos.Clases.ObtenerClasePorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.ObtenerClasePorId;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.AccesoADatos.Clases.ObtenerClasePorId;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.LogicaDeNegocio.Clases.ObtenerClasePorId
{
    public class ObtenerClasePorIdLN : IObtenerClasePorIdLN
    {
        private IObtenerClasePorIdAD _obtenerClasePorIdAD;

        public ObtenerClasePorIdLN()
        {
            _obtenerClasePorIdAD = new ObtenerClasePorIdAD();
        }

        public ClaseListadoDto ObtenerClasePorId(int id)
        {
            ClaseListadoDto laClase = _obtenerClasePorIdAD.ObtenerClasePorId(id);
            return laClase;
        }
    }
}
