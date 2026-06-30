using GimnasioJena.Abstracciones.AccesoADatos.Reservas.RegistrarReserva;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerMembresiaPorCliente;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.RegistrarReserva;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.AccesoADatos.Reservas.RegistrarReserva;
using GimnasioJena.LogicaDeNegocio.Membresias.ObtenerMembresiaPorCliente;
using System;

namespace GimnasioJena.LogicaDeNegocio.Reservas.RegistrarReserva
{
    public class RegistrarReservaLN : IRegistrarReservaLN
    {
        private readonly IRegistrarReservaAD _registrarReservaAD;
        private readonly IObtenerMembresiaPorClienteLN _obtenerMembresiaPorClienteLN;

        public RegistrarReservaLN()
        {
            _registrarReservaAD = new RegistrarReservaAD();
            _obtenerMembresiaPorClienteLN = new ObtenerMembresiaPorClienteLN();
        }

        public bool RegistrarReserva(ReservaCrearDto reserva)
        {
            if (reserva == null)
                return false;

            if (reserva.idUsuario <= 0)
                return false;

            if (reserva.idClaseProgramada <= 0)
                return false;

            var clase = _registrarReservaAD.ObtenerClaseParaValidacion(reserva.idClaseProgramada);

            if (clase == null)
                return false;

            if (clase.idEstadoClase != 1)
                return false;

            DateTime fechaHoraClase = clase.fechaClase.Date.Add(clase.horaInicio);

            if (fechaHoraClase <= DateTime.Now)
                return false;

            var membresia = _obtenerMembresiaPorClienteLN.ObtenerMembresiaActivaPorCliente(reserva.idUsuario);

            if (membresia == null)
                return false;

            if (membresia.clasesDisponibles.HasValue && membresia.clasesDisponibles.Value <= 0)
                return false;

            bool yaTieneReservaActiva = _registrarReservaAD.UsuarioTieneReservaActiva(
                reserva.idUsuario,
                reserva.idClaseProgramada
            );

            if (yaTieneReservaActiva)
                return false;

            int reservasActivas = _registrarReservaAD.ContarReservasActivasPorClase(reserva.idClaseProgramada);

            if (reservasActivas >= clase.cupoMaximo)
                return false;

            reserva.idEstadoReserva = 1;

            return _registrarReservaAD.RegistrarReserva(reserva) > 0;
        }
    }
}