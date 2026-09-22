using GimnasioJena.AccesoADatos.Entidades.Progreso;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Progreso
{
    public class EntrenamientoAvanzadoAD : IEntrenamientoAvanzadoAD
    {
        private readonly Contexto _elContexto;

        public EntrenamientoAvanzadoAD()
        {
            _elContexto = new Contexto();
        }

        public async Task<List<EntrenamientoEntidad>> ObtenerPlantillasDisponiblesAD()
        {
            var plantillas =
                await (from e in _elContexto.Entrenamientos
                       where e.EsPlantilla
                       orderby e.NombreRutina
                       select e)
                    .ToListAsync();

            return plantillas;
        }

        public async Task<EntrenamientoEntidad> ObtenerEntrenamientoPorIdAD(int idEntrenamiento)
        {
            var entrenamiento =
                await _elContexto.Entrenamientos
                    .FirstOrDefaultAsync(e => e.IdEntrenamiento == idEntrenamiento);

            return entrenamiento;
        }

        public async Task<int> InsertarEntrenamientoFlexibleAD(EntrenamientoEntidad entidad)
        {
            _elContexto.Entrenamientos.Add(entidad);
            await _elContexto.SaveChangesAsync();

            return entidad.IdEntrenamiento;
        }

        public async Task GuardarEntrenamientoAD(EntrenamientoEntidad entidad)
        {
            _elContexto.Entrenamientos.Add(entidad);
            await _elContexto.SaveChangesAsync();
        }

        public async Task<EntrenamientoEntidad> ObtenerEntrenamientoPorClaseAD(int idClaseProgramada)
        {
            // 1. Buscar entrenamiento asignado directamente a la clase aislada.
            var entrenamientoDirecto =
                await _elContexto.Entrenamientos
                    .FirstOrDefaultAsync(e => e.idClaseProgramada == idClaseProgramada);

            if (entrenamientoDirecto != null)
            {
                return entrenamientoDirecto;
            }

            // 2. Si no existe, resolver la plantilla recurrente vía el horario de la clase.
            var idHorario =
                await (from c in _elContexto.Clases
                       where c.idClaseProgramada == idClaseProgramada
                       select c.idHorario)
                    .FirstOrDefaultAsync();

            if (idHorario == null)
            {
                return null;
            }

            var entrenamientoRecurrente =
                await _elContexto.Entrenamientos
                    .FirstOrDefaultAsync(e => e.idHorario == idHorario);

            return entrenamientoRecurrente;
        }
    }
}
