using GimnasioJena.Abstracciones.AccesoADatos.Progreso;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Progreso;
using GimnasioJena.Abstracciones.Modelos.Progreso;
using GimnasioJena.AccesoADatos.Progreso;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Progreso
{
    public class EntrenamientoLN : IEntrenamientoLN
    {
        private readonly IEntrenamientoAD _entrenamientoAD;

        public EntrenamientoLN()
        {
            _entrenamientoAD = new EntrenamientoAD();
        }

        public async Task<List<EntrenamientoDto>> ObtenerTodosLN()
        {
            return await _entrenamientoAD.ObtenerTodosAD();
        }

        public async Task<EntrenamientoDto> ObtenerEntrenamientoDeHoyLN()
        {
            var cultura = new System.Globalization.CultureInfo("es-CR");
            var diaHoy = cultura.DateTimeFormat.GetDayName(System.DateTime.Now.DayOfWeek);
            diaHoy = cultura.TextInfo.ToTitleCase(diaHoy);

            return await _entrenamientoAD.ObtenerPorDiaAD(diaHoy);
        }

        public async Task CrearEntrenamientoLN(CrearEntrenamientoDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("No se recibió la información del entrenamiento.");
            }

            if (string.IsNullOrWhiteSpace(dto.DiaSemana))
            {
                throw new ArgumentException("Debe seleccionar el día de la semana.");
            }

            if (string.IsNullOrWhiteSpace(dto.NombreRutina))
            {
                throw new ArgumentException("Debe indicar el nombre de la rutina.");
            }

            if (string.IsNullOrWhiteSpace(dto.EjerciciosDetalle))
            {
                throw new ArgumentException("Debe indicar el detalle de los ejercicios.");
            }

            dto.DiaSemana = dto.DiaSemana.Trim();
            dto.NombreRutina = dto.NombreRutina.Trim();
            dto.EjerciciosDetalle = dto.EjerciciosDetalle.Trim();

            await _entrenamientoAD.CrearAD(dto);
        }
    }
}
