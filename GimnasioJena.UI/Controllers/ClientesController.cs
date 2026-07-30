using GimnasioJena.Abstracciones.LogicaDeNegocio.Bitacora;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerMembresiaPorCliente;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerPlanesMembresia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerPlanMembresiaPorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.CancelarReserva;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerReservaPorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerReservasPorUsuario;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId;
using GimnasioJena.Abstracciones.Modelos.Bitacora;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.LogicaDeNegocio.Bitacora;
using GimnasioJena.LogicaDeNegocio.Membresias.ObtenerMembresiaPorCliente;
using GimnasioJena.LogicaDeNegocio.Membresias.ObtenerPlanesMembresia;
using GimnasioJena.LogicaDeNegocio.Membresias.ObtenerPlanMembresiaPorId;
using GimnasioJena.LogicaDeNegocio.Reservas.CancelarReserva;
using GimnasioJena.LogicaDeNegocio.Reservas.ObtenerReservaPorId;
using GimnasioJena.LogicaDeNegocio.Reservas.ObtenerReservasPorUsuario;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [Authorize(Roles = "CLIENTE")]
    public class ClientesController : Controller
    {
        private readonly IObtenerUsuarioPorIdLN _obtenerUsuarioServicio;
        private readonly IObtenerReservasPorUsuarioLN _obtenerReservasPorUsuarioServicio;
        private readonly ICancelarReservaLN _cancelarReservaServicio;
        private readonly IObtenerReservaPorIdLN _obtenerReservaPorIdServicio;
        private readonly IObtenerMembresiaPorClienteLN _obtenerMembresiaPorClienteServicio;
        private readonly IObtenerPlanMembresiaPorIdLN _obtenerPlanMembresiaPorIdServicio;
        private readonly IObtenerPlanesMembresiaLN _obtenerPlanesMembresiaServicio;
        private readonly IRegistrarBitacoraLN _registrarBitacoraLN;
        
        public ClientesController(IObtenerUsuarioPorIdLN obtenerUsuarioServicio)
        {
            _obtenerUsuarioServicio = obtenerUsuarioServicio;
            _obtenerReservasPorUsuarioServicio = new ObtenerReservasPorUsuarioLN();
            _cancelarReservaServicio = new CancelarReservaLN();
            _obtenerReservaPorIdServicio = new ObtenerReservaPorIdLN();
            _obtenerMembresiaPorClienteServicio = new ObtenerMembresiaPorClienteLN();
            _obtenerPlanMembresiaPorIdServicio = new ObtenerPlanMembresiaPorIdLN();
            _obtenerPlanesMembresiaServicio = new ObtenerPlanesMembresiaLN();
            _registrarBitacoraLN = new RegistrarBitacoraLN();
        }

        public async Task<ActionResult> MiPerfil()
        {
            var identityUserId = User.Identity.GetUserId();
            var perfil = await _obtenerUsuarioServicio.ObtenerUsuarioPorId(identityUserId);
            return View(perfil);
        }

        public async Task<ActionResult> MiMembresia()
        {
            var identityUserId = User.Identity.GetUserId();

            var perfil =
                await _obtenerUsuarioServicio
                    .ObtenerUsuarioPorId(identityUserId);

            if (perfil == null)
            {
                TempData["MensajeError"] =
                    "No se encontró la información del usuario.";

                return RedirectToAction("MiPerfil");
            }

            var membresia =
    _obtenerMembresiaPorClienteServicio
        .ObtenerUltimaMembresiaPorCliente(
            perfil.idUsuario);

            var planes =
                _obtenerPlanesMembresiaServicio
                    .ObtenerPlanesActivos();

            var modelo =
                new MiMembresiaViewModel
                {
                    membresiaActual = membresia,
                    planesDisponibles = planes
                };

            return View(modelo);
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmarCompra(int idPlan)
        {
            if (idPlan <= 0)
            {
                TempData["MensajeError"] =
                    "El plan seleccionado no es válido.";

                return RedirectToAction("MiMembresia");
            }

            var identityUserId = User.Identity.GetUserId();

            var perfil =
                await _obtenerUsuarioServicio
                    .ObtenerUsuarioPorId(identityUserId);

            if (perfil == null)
            {
                TempData["MensajeError"] =
                    "No se encontró la información del usuario.";

                return RedirectToAction("MiPerfil");
            }

            var plan =
                _obtenerPlanMembresiaPorIdServicio
                    .ObtenerPlanMembresiaPorId(idPlan);

            if (plan == null)
            {
                TempData["MensajeError"] =
                    "El plan seleccionado no existe o no se encuentra disponible.";

                return RedirectToAction("MiMembresia");
            }

            var membresiaActual =
                _obtenerMembresiaPorClienteServicio
                    .ObtenerUltimaMembresiaPorCliente(
                        perfil.idUsuario);

            DateTime hoy = DateTime.Today;

            if (membresiaActual != null &&
                membresiaActual.fechaFin.Date > hoy)
            {
                TempData["MensajeError"] =
                    "Podrás renovar o cambiar de plan a partir del " +
                    membresiaActual.fechaFin.ToString("dd/MM/yyyy") + ".";

                return RedirectToAction("MiMembresia");
            }

            string tipoOperacion;

            if (membresiaActual == null)
            {
                tipoOperacion = "Adquisición";
            }
            else if (
                membresiaActual.idPlanMembresia ==
                plan.idPlanMembresia)
            {
                tipoOperacion = "Renovación";
            }
            else
            {
                tipoOperacion = "Cambio de plan";
            }

            var modelo = new OperacionMembresiaDto
            {
                idUsuario =
                    perfil.idUsuario,

                idPlan =
                    plan.idPlanMembresia,

                nombrePlan =
                    plan.nombrePlan,

                precio =
                    plan.precio,

                duracionDias =
                    plan.duracionDias,

                tipoOperacion =
                    tipoOperacion,

                fechaInicioPropuesta =
                    hoy,

                fechaFinPropuesta =
                    hoy.AddDays(plan.duracionDias - 1),

                clasesAsignadas =
                    plan.cantidadClases,

                descripcionPlan =
                    plan.cantidadClases.HasValue
                        ? "Incluye " +
                          plan.cantidadClases.Value +
                          " clases durante " +
                          plan.duracionDias +
                          " días."
                        : "Incluye clases ilimitadas durante " +
                          plan.duracionDias +
                          " días."
            };

            return View(modelo);
        }
        public ActionResult ReservarClases()
        {
            return View();
        }

        public async Task<ActionResult> MisReservas()
        {
            var identityUserId = User.Identity.GetUserId();
            var perfil = await _obtenerUsuarioServicio.ObtenerUsuarioPorId(identityUserId);
            if (perfil == null)
            {
                TempData["MensajeError"] = "No se encontró la información del usuario.";
                return RedirectToAction("Index", "Home");
            }

            List<ReservaListadoDto> reservas = _obtenerReservasPorUsuarioServicio.ObtenerReservasPorUsuario(perfil.idUsuario);
            return View(reservas);
        }

        public async Task<ActionResult> DetalleReserva(int id)
        {
            var identityUserId = User.Identity.GetUserId();
            var perfil = await _obtenerUsuarioServicio.ObtenerUsuarioPorId(identityUserId);

            if (perfil == null)
            {
                TempData["MensajeError"] = "No se encontró la información del usuario.";
                return RedirectToAction("MisReservas");
            }

            var reserva = _obtenerReservaPorIdServicio.ObtenerReservaPorId(id);

            if (reserva == null)
            {
                TempData["MensajeError"] = "No se encontró la reserva solicitada.";
                return RedirectToAction("MisReservas");
            }

            if (reserva.idUsuario != perfil.idUsuario)
            {
                TempData["MensajeError"] = "No tienes permiso para ver esta reserva.";
                return RedirectToAction("MisReservas");
            }

            return View(reserva);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelarReserva(int id)
        {
            var identityUserId = User.Identity.GetUserId();
            var perfil = await _obtenerUsuarioServicio.ObtenerUsuarioPorId(identityUserId);

            if (perfil == null)
            {
                TempData["MensajeError"] = "Usuario no autenticado correctamente.";
                return RedirectToAction("MisReservas");
            }

            var reservaCancelar = new ReservaCancelarDto
            {
                idReserva = id,
                motivoCancelacion = "Cancelación solicitada desde Mis Reservas"
            };

            bool resultado = _cancelarReservaServicio.CancelarReserva(reservaCancelar, perfil.idUsuario);

            if (resultado)
            {
                RegistrarBitacora(
                    "Reserva",
                    "UPDATE",
                    id,
                    "El cliente canceló la reserva con idReserva: " + id,
                    perfil.idUsuario
                );

                TempData["MensajeExito"] = "Reserva cancelada correctamente.";
            }
            else
            {
                TempData["MensajeError"] = "No se pudo cancelar la reserva. Verifica que esté activa y que la clase no haya iniciado.";
            }

            return RedirectToAction("MisReservas");
        }
        private string ObtenerIpUsuario()
        {
            return Request.UserHostAddress;
        }

        private void RegistrarBitacora(string tabla, string accion, int? idRegistro, string detalle, int? idUsuario)
        {
            _registrarBitacoraLN.RegistrarBitacora(new BitacoraDto
            {
                idUsuario = idUsuario,
                tablaAfectada = tabla,
                accionRealizada = accion,
                idRegistroAfectado = idRegistro,
                detalle = detalle,
                ipUsuario = ObtenerIpUsuario()
            });
        }
    }
}