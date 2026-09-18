using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ObtenerWorkoutClase;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.ObtenerWorkoutClase;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.AccesoADatos.Entrenamientos.ObtenerWorkoutClase;

namespace GimnasioJena.LogicaDeNegocio.Entrenamientos.ObtenerWorkoutClase
{
    public class ObtenerWorkoutClaseLN : IObtenerWorkoutClaseLN
    {
        private readonly IObtenerWorkoutClaseAD _obtenerWorkoutClaseAD;

        public ObtenerWorkoutClaseLN()
        {
            _obtenerWorkoutClaseAD = new ObtenerWorkoutClaseAD();
        }

        public ClaseWorkoutClienteDto ObtenerWorkoutClase(int classId, int userId)
        {
            return _obtenerWorkoutClaseAD.ObtenerWorkoutClase(classId, userId);
        }
    }
}
