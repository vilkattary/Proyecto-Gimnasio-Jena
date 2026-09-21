using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ObtenerProgresoCliente;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.ObtenerProgresoCliente;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.AccesoADatos.Entrenamientos.ObtenerProgresoCliente;

namespace GimnasioJena.LogicaDeNegocio.Entrenamientos.ObtenerProgresoCliente
{
    public class ObtenerProgresoClienteLN : IObtenerProgresoClienteLN
    {
        private readonly IObtenerProgresoClienteAD _obtenerProgresoClienteAD;

        public ObtenerProgresoClienteLN()
        {
            _obtenerProgresoClienteAD = new ObtenerProgresoClienteAD();
        }

        public ProgresoClienteDto ObtenerProgreso(int userId, int ultimosDias)
        {
            // Regla: userId inválido no devuelve datos; rango por defecto 90 días.
            if (userId <= 0)
            {
                return new ProgresoClienteDto();
            }

            if (ultimosDias < 0)
            {
                ultimosDias = 0;
            }

            return _obtenerProgresoClienteAD.ObtenerProgreso(userId, ultimosDias);
        }
    }
}
