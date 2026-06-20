using GimnasioJena.Abstracciones.AccesoADatos.Entrenadores.ObtenerEntrenadorPorId;
using GimnasioJena.Abstracciones.AccesoADatos.EstadoReservas.ObtenerEstadoReservas;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.EstadoReservas.ObtenerEstadoReservas
{
    public class ObtenerEstadoReservas : IObtenerEstadoReservaAD
    {
        private Contexto _elContexto;

        
        public ObtenerEstadoReservas()
        {
            _elContexto = new Contexto();
        }

        
        public EstadoReservasDto ObtenerEstadoReservaPorId(int id)
        {
            return _elContexto.EstadoReservas
                .Where(er => er.idEstadoReserva == id)
                .Select(er => new EstadoReservasDto
                {
                    idEstadoReserva = er.idEstadoReserva,
                    nombreEstado = er.nombreEstado
                })
                .FirstOrDefault();
        }

     
        public IEnumerable<EstadoReservasDto> ObtenerTodosLosEstadosReservas()
        {
            return _elContexto.EstadoReservas
                .Select(er => new EstadoReservasDto
                {
                    idEstadoReserva = er.idEstadoReserva,
                    nombreEstado = er.nombreEstado
                })
                .ToList();
        }
    }
}
