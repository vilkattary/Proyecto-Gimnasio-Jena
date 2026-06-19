using GimnasioJena.Abstracciones.AccesoADatos.Reservas.ObtenerReservasPorClase;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Reservas.ObtenerReservasPorClase
{
    public class ObtenerReservasPorClaseAD : IObtenerReservasPorClaseAD
    {
        private readonly Contexto _elContexto;

        public ObtenerReservasPorClaseAD()
        {
            _elContexto = new Contexto();
        }

        public List<ReservaClaseDto> ObtenerReservasPorClase(int idClaseProgramada)
        {
            var reservas =
                (from reserva in _elContexto.Reservas
                 join usuario in _elContexto.Usuarios
                    on reserva.idUsuario equals usuario.idUsuario
                 join estadoReserva in _elContexto.EstadoReservas
                    on reserva.idEstadoReserva equals estadoReserva.idEstadoReserva
                 where reserva.idClaseProgramada == idClaseProgramada
                 select new ReservaClaseDto
                 {
                     idReserva = reserva.idReserva,
                     idClaseProgramada = reserva.idClaseProgramada,
                     nombreCliente = usuario.nombre + " " + usuario.apellido1 + " " + usuario.apellido2,
                     correoCliente = usuario.correo,
                     telefonoCliente = usuario.telefono,
                     estadoReserva = estadoReserva.nombreEstado
                 })
                .OrderBy(r => r.nombreCliente)
                .ToList();

            return reservas;
        }
    }
}