using GimnasioJena.Abstracciones.Modelos.Clases;

namespace GimnasioJena.Abstracciones.AccesoADatos.Clases.ObtenerClasePorId
{
    public interface IObtenerClasePorIdAD
    {
        ClaseListadoDto ObtenerClasePorId(int id);
    }
}
