using GimnasioJena.Abstracciones.AccesoADatos.Clases.ObtenerTodasLasClases;
using GimnasioJena.Abstracciones.General.Fechas;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.ObtenerTodasLasClases;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.AccesoADatos.Clases.ObtenerTodasLasClases;
using GimnasioJena.LogicaDeNegocio.General.Fechas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.LogicaDeNegocio.Clases.ObtenerTodasLasClases
{
    public class ObtenerTodasLasClasesLN : IObtenerTodasLasClasesLN
    {
        private readonly IObtenerTodasLasClasesAD _obtenerTodasLasClasesAD;
        private readonly IFechasLN _fechasLN;

        public ObtenerTodasLasClasesLN()
        {
            _obtenerTodasLasClasesAD = new ObtenerTodasLasClasesAD();
            _fechasLN = new FechasLN();
        }

        public List<ClaseListadoDto> ObtenerTodasLasClases()
        {
            return _obtenerTodasLasClasesAD
                .ObtenerTodasLasClases()
                .OrderByDescending(c => c.fechaClase)
                .ThenBy(c => c.horaInicio)
                .ToList();
        }

        public List<ClaseListadoDto> ObtenerProximasClasesParaCliente()
        {
            DateTime ahora = _fechasLN.ObtenerFechaActual();

            var clases = _obtenerTodasLasClasesAD
                .ObtenerTodasLasClases()
                .Where(c =>
                    c.estadoClase == "Activa" &&
                    c.fechaHoraInicio > ahora)
                .OrderBy(c => c.fechaHoraInicio)
                .Take(10)
                .ToList();

            foreach (var clase in clases)
            {
                DateTime aperturaReserva =
                    clase.fechaHoraInicio.AddHours(-24);

                DateTime cierreReserva =
                    clase.fechaHoraInicio.AddMinutes(-10);

                if (clase.cuposDisponibles <= 0)
                {
                    clase.reservaHabilitada = false;
                    clase.mensajeReserva = "Sin cupos disponibles";
                }
                else if (ahora < aperturaReserva)
                {
                    clase.reservaHabilitada = false;
                    clase.mensajeReserva =
                        "Disponible desde " +
                        aperturaReserva.ToString("dd/MM/yyyy HH:mm");
                }
                else if (ahora >= cierreReserva)
                {
                    clase.reservaHabilitada = false;
                    clase.mensajeReserva = "Reservas cerradas";
                }
                else
                {
                    clase.reservaHabilitada = true;
                    clase.mensajeReserva = "Reserva disponible";
                }
            }

            return clases;
        }
    }
}