using GimnasioJena.Abstracciones.Modelos.Entrenamientos;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.ObtenerMesocicloPorId
{
    public interface IObtenerMesocicloPorIdLN
    {
        MesocicloDto ObtenerMesocicloPorId(int idMesociclo);
    }
}
