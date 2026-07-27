using GimnasioJena.Abstracciones.AccesoADatos.HorariosSemanales.GenerarClasesProgramadas;
using GimnasioJena.Abstracciones.General.Fechas;
using GimnasioJena.Abstracciones.LogicaDeNegocio.HorariosSemanales.GenerarClasesProgramadas;
using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;
using GimnasioJena.AccesoADatos.HorariosSemanales.GenerarClasesProgramadas;
using GimnasioJena.LogicaDeNegocio.General.Fechas;
using System;

namespace GimnasioJena.LogicaDeNegocio.HorariosSemanales.GenerarClasesProgramadas
{
    public class GenerarClasesProgramadasLN :
        IGenerarClasesProgramadasLN
    {
        private readonly IGenerarClasesProgramadasAD
            _generarClasesProgramadasAD;

        private readonly IFechasLN
            _fechasLN;

        public GenerarClasesProgramadasLN()
        {
            _generarClasesProgramadasAD =
                new GenerarClasesProgramadasAD();

            _fechasLN =
                new FechasLN();
        }

        public ResultadoGeneracionClasesDto
            GenerarClasesProgramadas(
                GenerarClasesProgramadasDto modelo
            )
        {
            ResultadoGeneracionClasesDto
                resultadoValidacion =
                    ValidarSolicitud(modelo);

            if (!resultadoValidacion.fueExitosa)
            {
                return resultadoValidacion;
            }

            DateTime fechaActual =
                _fechasLN
                    .ObtenerFechaActual()
                    .Date;

            /*
             * Si el periodo comienza antes de hoy,
             * se ajusta automáticamente para impedir
             * la creación de clases en fechas pasadas.
             */
            if (modelo.fechaInicio.Date < fechaActual)
            {
                modelo.fechaInicio =
                    fechaActual;
            }

            return _generarClasesProgramadasAD
                .GenerarClasesProgramadas(modelo);
        }

        private ResultadoGeneracionClasesDto
            ValidarSolicitud(
                GenerarClasesProgramadasDto modelo
            )
        {
            if (modelo == null)
            {
                return CrearResultadoError(
                    "No se recibieron los datos necesarios para generar las clases."
                );
            }

            DateTime fechaActual =
                _fechasLN
                    .ObtenerFechaActual()
                    .Date;

            DateTime fechaInicio =
                modelo.fechaInicio.Date;

            DateTime fechaFin =
                modelo.fechaFin.Date;

            if (
                fechaInicio == DateTime.MinValue.Date ||
                fechaFin == DateTime.MinValue.Date
            )
            {
                return CrearResultadoError(
                    "Debe indicar la fecha inicial y la fecha final."
                );
            }

            if (fechaFin < fechaInicio)
            {
                return CrearResultadoError(
                    "La fecha final no puede ser anterior a la fecha inicial."
                );
            }

            if (fechaFin < fechaActual)
            {
                return CrearResultadoError(
                    "El periodo seleccionado pertenece completamente al pasado."
                );
            }

            const int cantidadMaximaDias = 93;

            double cantidadDias =
                (fechaFin - fechaInicio)
                    .TotalDays + 1;

            if (cantidadDias > cantidadMaximaDias)
            {
                return CrearResultadoError(
                    "Solo se permite generar un máximo de 93 días por operación."
                );
            }

            modelo.fechaInicio =
                fechaInicio;

            modelo.fechaFin =
                fechaFin;

            return new ResultadoGeneracionClasesDto
            {
                fueExitosa = true,
                mensaje =
                    "La solicitud de generación es válida."
            };
        }

        private ResultadoGeneracionClasesDto
            CrearResultadoError(
                string mensaje
            )
        {
            return new ResultadoGeneracionClasesDto
            {
                fueExitosa = false,
                mensaje = mensaje,
                clasesGeneradas = 0,
                clasesOmitidas = 0,
                horariosProcesados = 0
            };
        }
    }
}