using GimnasioJena.Abstracciones.AccesoADatos.Progreso;
using GimnasioJena.Abstracciones.Modelos.Progreso;
using GimnasioJena.AccesoADatos.Entidades.Progreso;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Progreso
{
    public class EntrenamientoAD : IEntrenamientoAD
    {
        private readonly Contexto _elContexto;

        public EntrenamientoAD()
        {
            _elContexto = new Contexto();
        }

        public async Task<List<EntrenamientoDto>> ObtenerTodosAD()
        {
            var entrenamientos =
                await (from e in _elContexto.Entrenamientos
                       orderby e.NombreRutina
                       select new EntrenamientoDto
                       {
                           IdEntrenamiento = e.IdEntrenamiento,
                           DiaSemana = e.DiaSemana,
                           NombreRutina = e.NombreRutina,
                           EjerciciosDetalle = e.EjerciciosDetalle
                       })
                    .ToListAsync();

            return entrenamientos;
        }

        public async Task<EntrenamientoDto> ObtenerPorDiaAD(string diaSemana)
        {
            var entrenamiento =
                await (from e in _elContexto.Entrenamientos
                       where e.DiaSemana == diaSemana
                       orderby e.IdEntrenamiento descending
                       select new EntrenamientoDto
                       {
                           IdEntrenamiento = e.IdEntrenamiento,
                           DiaSemana = e.DiaSemana,
                           NombreRutina = e.NombreRutina,
                           EjerciciosDetalle = e.EjerciciosDetalle
                       })
                    .FirstOrDefaultAsync();

            return entrenamiento;
        }

        public async Task CrearAD(CrearEntrenamientoDto dto)
        {
            var nuevoEntrenamiento = new EntrenamientoEntidad
            {
                DiaSemana = dto.DiaSemana,
                NombreRutina = dto.NombreRutina,
                EjerciciosDetalle = dto.EjerciciosDetalle
            };

            _elContexto.Entrenamientos.Add(nuevoEntrenamiento);
            await _elContexto.SaveChangesAsync();
        }
    }
}
