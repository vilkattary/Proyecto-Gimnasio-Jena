using GimnasioJena.Abstracciones.AccesoADatos.Progreso;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Progreso
{
    public class AsistenciaAD : IAsistenciaAD
    {
        private readonly Contexto _elContexto;

        public AsistenciaAD()
        {
            _elContexto = new Contexto();
        }

        public async Task<int> ObtenerAsistenciaUltimos30DiasAD(int idUsuario)
        {
            var fechaLimite = DateTime.Now.AddDays(-30);

            var total =
                await (from asistencia in _elContexto.Asistencias
                       join reserva in _elContexto.Reservas
                          on asistencia.idReserva equals reserva.idReserva
                       where reserva.idUsuario == idUsuario
                          && asistencia.asistio
                          && asistencia.fechaRegistro >= fechaLimite
                       select asistencia.idAsistencia)
                    .CountAsync();

            return total;
        }
    }
}
