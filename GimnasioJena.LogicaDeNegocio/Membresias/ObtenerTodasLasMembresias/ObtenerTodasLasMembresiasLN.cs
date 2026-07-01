using GimnasioJena.Abstracciones.AccesoADatos.Membresias.ObtenerTodasLasMembresias;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerTodasLasMembresias;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.AccesoADatos.Membresias.ObtenerTodasLasMembresias;
using System.Collections.Generic;

namespace GimnasioJena.LogicaDeNegocio.Membresias.ObtenerTodasLasMembresias
{
    public class ObtenerTodasLasMembresiasLN : IObtenerTodasLasMembresiasLN
    {
        private readonly IObtenerTodasLasMembresiasAD _obtenerTodasLasMembresiasAD;

        public ObtenerTodasLasMembresiasLN()
        {
            _obtenerTodasLasMembresiasAD = new ObtenerTodasLasMembresiasAD();
        }

        public List<MembresiaListadoDto> ObtenerTodasLasMembresias()
        {
            return _obtenerTodasLasMembresiasAD.ObtenerTodasLasMembresias();
        }
    }
}