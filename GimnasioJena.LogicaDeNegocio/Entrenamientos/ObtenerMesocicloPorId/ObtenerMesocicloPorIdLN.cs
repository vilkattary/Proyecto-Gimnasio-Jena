using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ObtenerMesocicloPorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.ObtenerMesocicloPorId;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.AccesoADatos.Entrenamientos.ObtenerMesocicloPorId;

namespace GimnasioJena.LogicaDeNegocio.Entrenamientos.ObtenerMesocicloPorId
{
    public class ObtenerMesocicloPorIdLN : IObtenerMesocicloPorIdLN
    {
        private readonly IObtenerMesocicloPorIdAD _obtenerMesocicloPorIdAD;

        public ObtenerMesocicloPorIdLN()
        {
            _obtenerMesocicloPorIdAD = new ObtenerMesocicloPorIdAD();
        }

        public MesocicloDto ObtenerMesocicloPorId(int idMesociclo)
        {
            return _obtenerMesocicloPorIdAD.ObtenerMesocicloPorId(idMesociclo);
        }
    }
}
