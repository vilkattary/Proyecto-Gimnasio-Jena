using GimnasioJena.AccesoADatos.Entidades.Asistencias;
using GimnasioJena.AccesoADatos.Entidades.Progreso;
using GimnasioJena.AccesoADatos.Entidades.Reservas;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Progreso
{
    public class ProgresoAvanzadoAD : IProgresoAvanzadoAD
    {
        private readonly Contexto _elContexto;

        public ProgresoAvanzadoAD()
        {
            _elContexto = new Contexto();
        }

        public async Task GuardarProgresoMultimodalAD(ProgresoClienteEntidad progreso)
        {
            _elContexto.ProgresosCliente.Add(progreso);
            await _elContexto.SaveChangesAsync();
        }

        public async Task GuardarProgresoClienteDetalleAD(List<ProgresoClienteEntidad> lista)
        {
            _elContexto.ProgresosCliente.AddRange(lista);
            await _elContexto.SaveChangesAsync();
        }

        public async Task<List<ProgresoClienteEntidad>> ObtenerProgresoRangoAD(int idUsuario, DateTime fechaInicio, DateTime fechaFin)
        {
            var progreso =
                await (from p in _elContexto.ProgresosCliente
                       where p.idUsuario == idUsuario
                          && p.FechaRegistro >= fechaInicio
                          && p.FechaRegistro <= fechaFin
                       orderby p.FechaRegistro
                       select p)
                    .ToListAsync();

            return progreso;
        }

        public async Task<Dictionary<string, int>> ObtenerMatrizAsistenciaAnualAD(int idUsuario, int anio)
        {
            var conteos =
                await (from a in _elContexto.Asistencias
                       join r in _elContexto.Reservas
                          on a.idReserva equals r.idReserva
                       where r.idUsuario == idUsuario
                          && a.asistio
                          && a.fechaRegistro.Year == anio
                       group a by DbFunctions.TruncateTime(a.fechaRegistro) into grupo
                       select new
                       {
                           Fecha = grupo.Key,
                           Conteo = grupo.Count()
                       })
                    .ToListAsync();

            var matriz = conteos
                .Where(x => x.Fecha.HasValue)
                .ToDictionary(
                    x => x.Fecha.Value.ToString("yyyy-MM-dd"),
                    x => x.Conteo);

            return matriz;
        }
    }
}
