using GimnasioJena.Abstracciones.AccesoADatos.Progreso;
using GimnasioJena.Abstracciones.Modelos.Progreso;
using GimnasioJena.AccesoADatos.Entidades.Progreso;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Progreso
{
    public class ProgresoAD : IProgresoAD
    {
        private readonly Contexto _elContexto;

        public ProgresoAD()
        {
            _elContexto = new Contexto();
        }

        public async Task<List<ProgresoDiaDto>> ObtenerProgresoPorRangoFechaAD(int idUsuario, DateTime fechaInicio, DateTime fechaFin)
        {
            var progreso =
                await (from p in _elContexto.ProgresosCliente
                       join e in _elContexto.Entrenamientos
                          on p.IdEntrenamiento equals e.IdEntrenamiento
                       where p.idUsuario == idUsuario
                          && p.FechaRegistro >= fechaInicio
                          && p.FechaRegistro <= fechaFin
                       orderby p.FechaRegistro
                       select new ProgresoDiaDto
                       {
                           idUsuario = p.idUsuario,
                           IdEntrenamiento = p.IdEntrenamiento,
                           NombreRutina = e.NombreRutina,
                           PesoAlcanzado = p.PesoAlcanzado ?? 0,
                           Repeticiones = p.Repeticiones ?? 0,
                           Notas = p.Notas,
                           FechaRegistro = p.FechaRegistro
                       })
                    .ToListAsync();

            return progreso;
        }

        public async Task GuardarProgresoAD(ProgresoDiaDto progreso)
        {
            var nuevoProgreso = new ProgresoClienteEntidad
            {
                idUsuario = progreso.idUsuario,
                IdEntrenamiento = progreso.IdEntrenamiento,
                PesoAlcanzado = progreso.PesoAlcanzado,
                Repeticiones = progreso.Repeticiones,
                Notas = progreso.Notas,
                FechaRegistro = progreso.FechaRegistro == default(DateTime)
                    ? DateTime.Now
                    : progreso.FechaRegistro
            };

            _elContexto.ProgresosCliente.Add(nuevoProgreso);
            await _elContexto.SaveChangesAsync();
        }
    }
}
