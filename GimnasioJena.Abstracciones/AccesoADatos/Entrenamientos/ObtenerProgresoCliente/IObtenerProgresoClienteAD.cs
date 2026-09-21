using GimnasioJena.Abstracciones.Modelos.Entrenamientos;

namespace GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ObtenerProgresoCliente
{
    public interface IObtenerProgresoClienteAD
    {
        // Devuelve los agregados de progreso del cliente en el rango indicado.
        ProgresoClienteDto ObtenerProgreso(int userId, int ultimosDias);
    }
}
