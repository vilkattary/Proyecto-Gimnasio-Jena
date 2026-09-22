using GimnasioJena.Abstracciones.AccesoADatos.Progreso;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Progreso;
using GimnasioJena.Abstracciones.Modelos.Progreso;
using GimnasioJena.AccesoADatos.Progreso;
using System;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Progreso
{
    public class ProgresoLN : IProgresoLN
    {
        private readonly IProgresoAD _progresoAD;
        private readonly IMedicionesAD _medicionesAD;
        private readonly IAsistenciaAD _asistenciaAD;
        private readonly IUsuarioActualAD _usuarioActualAD;
        private readonly IEvolucionAvanzadaLN _evolucionAvanzadaLN;

        public ProgresoLN()
        {
            _progresoAD = new ProgresoAD();
            _medicionesAD = new MedicionesAD();
            _asistenciaAD = new AsistenciaAD();
            _usuarioActualAD = new UsuarioActualAD();
            _evolucionAvanzadaLN = new EvolucionAvanzadaLN();
        }

        public async Task<ResumenEvolucionDto> GenerarResumenEvolucionLN(int idUsuario, string filtroTiempo)
        {
            if (idUsuario <= 0)
            {
                throw new ArgumentException("El usuario indicado no es válido.", nameof(idUsuario));
            }

            var fechaFin = DateTime.Today;
            var fechaInicio = ObtenerFechaInicioSegunFiltro(filtroTiempo, fechaFin);

            var resumen = new ResumenEvolucionDto
            {
                AsistenciaUltimos30Dias =
                    await _asistenciaAD.ObtenerAsistenciaUltimos30DiasAD(idUsuario),
                HistorialProgreso =
                    await _progresoAD.ObtenerProgresoPorRangoFechaAD(idUsuario, fechaInicio, fechaFin),
                HistorialMediciones =
                    await _medicionesAD.ObtenerMedicionesUsuarioAD(idUsuario),
                MetricasAvanzadas =
                    await _evolucionAvanzadaLN.CompilarDashboardAvanzadoLN(idUsuario, filtroTiempo)
            };

            return resumen;
        }

        public async Task GuardarProgresoDiarioLN(ProgresoDiaDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("No se recibió la información del progreso.");
            }

            if (dto.idUsuario <= 0)
            {
                throw new ArgumentException("El usuario indicado no es válido.");
            }

            if (dto.IdEntrenamiento <= 0)
            {
                throw new ArgumentException("El entrenamiento indicado no es válido.");
            }

            if (dto.PesoAlcanzado < 0)
            {
                throw new ArgumentException("El peso alcanzado no puede ser negativo.");
            }

            if (dto.Repeticiones < 0)
            {
                throw new ArgumentException("Las repeticiones no pueden ser negativas.");
            }

            if (dto.FechaRegistro == default(DateTime))
            {
                dto.FechaRegistro = DateTime.Now;
            }

            await _progresoAD.GuardarProgresoAD(dto);
        }

        public async Task<int> ObtenerIdUsuarioActualLN(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                throw new ArgumentException("No se pudo identificar al usuario autenticado.");
            }

            var idUsuario = await _usuarioActualAD.ObtenerIdUsuarioPorIdentityAD(identityUserId);

            if (idUsuario == null || idUsuario <= 0)
            {
                throw new ArgumentException("El usuario autenticado no está registrado en el sistema.");
            }

            return idUsuario.Value;
        }

        private DateTime ObtenerFechaInicioSegunFiltro(string filtroTiempo, DateTime fechaFin)
        {
            switch (filtroTiempo)
            {
                case "1Mes":
                    return fechaFin.AddDays(-30);
                case "3Meses":
                    return fechaFin.AddDays(-90);
                case "6Meses":
                    return fechaFin.AddDays(-180);
                case "Anual":
                    return fechaFin.AddYears(-1);
                default:
                    return fechaFin.AddDays(-30);
            }
        }
    }
}
