using GimnasioJena.Abstracciones.Modelos.Entrenamientos;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.ObtenerProgresoCliente
{
    public interface IObtenerProgresoClienteLN
    {
        ProgresoClienteDto ObtenerProgreso(int userId, int ultimosDias);
    }
}
