using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ObtenerMesociclos;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.ObtenerMesociclos;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.AccesoADatos.Entrenamientos.ObtenerMesociclos;
using System.Collections.Generic;

namespace GimnasioJena.LogicaDeNegocio.Entrenamientos.ObtenerMesociclos
{
    public class ObtenerMesociclosLN : IObtenerMesociclosLN
    {
        private readonly IObtenerMesociclosAD _obtenerMesociclosAD;

        public ObtenerMesociclosLN()
        {
            _obtenerMesociclosAD = new ObtenerMesociclosAD();
        }

        public List<MesocicloListadoDto> ObtenerMesociclos()
        {
            return _obtenerMesociclosAD.ObtenerMesociclos();
        }
    }
}
