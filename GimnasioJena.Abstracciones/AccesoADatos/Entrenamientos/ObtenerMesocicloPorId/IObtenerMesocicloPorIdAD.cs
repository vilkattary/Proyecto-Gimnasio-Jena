using GimnasioJena.Abstracciones.Modelos.Entrenamientos;

namespace GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ObtenerMesocicloPorId
{
    public interface IObtenerMesocicloPorIdAD
    {
        MesocicloDto ObtenerMesocicloPorId(int idMesociclo);
    }
}
