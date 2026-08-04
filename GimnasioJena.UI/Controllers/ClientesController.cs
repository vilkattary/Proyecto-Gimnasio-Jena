using GimnasioJena.Abstracciones.LogicaDeNegocio.Bitacora;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerMembresiaPorCliente;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerPlanesMembresia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerPlanMembresiaPorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Pagos.RegistrarPago;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.RegistrarMembresia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.CancelarReserva;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerReservaPorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerReservasPorUsuario;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId;
using GimnasioJena.Abstracciones.Modelos.Bitacora;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.Abstracciones.Modelos.Pagos;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.LogicaDeNegocio.Bitacora;
using GimnasioJena.LogicaDeNegocio.Membresias.ObtenerMembresiaPorCliente;
using GimnasioJena.LogicaDeNegocio.Membresias.ObtenerPlanesMembresia;
using GimnasioJena.LogicaDeNegocio.Membresias.ObtenerPlanMembresiaPorId;
using GimnasioJena.LogicaDeNegocio.Membresias.RegistrarMembresia;
using GimnasioJena.LogicaDeNegocio.Pagos.RegistrarPago;
using GimnasioJena.LogicaDeNegocio.Reservas.CancelarReserva;
using GimnasioJena.LogicaDeNegocio.Reservas.ObtenerReservaPorId;
using GimnasioJena.LogicaDeNegocio.Reservas.ObtenerReservasPorUsuario;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        private readonly IRegistrarPagoLN _registrarPagoLN;
        private readonly IRegistrarMembresiaLN _registrarMembresiaLN;

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
            _registrarPagoLN = new RegistrarPagoLN();
            _registrarMembresiaLN = new RegistrarMembresiaLN();
        }

        public async Task<ActionResult> MiPerfil()
        {
            var identityUserId = User.Identity.GetUserId();
            var perfil = await _obtenerUsuarioServicio.ObtenerUsuarioPorId(identityUserId);
            if (perfil != null)
            {
                ViewBag.Reservas = _obtenerReservasPorUsuarioServicio.ObtenerReservasPorUsuario(perfil.idUsuario);
            }
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
            var modelo = await PrepararOperacionMembresia(idPlan);

            if (modelo == null)
            {
                TempData["MensajeError"] =
                    "No fue posible preparar la operación de la membresía.";

                return RedirectToAction("MiMembresia");
            }

            return View(modelo);
        }

        // TODO: [TILOPAY EN PAUSA] Descomentar y continuar cuando se tengan credenciales reales.
#if false
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> IniciarPago(int idUsuario, int idPlan)
        {
            try
            {
                var apiUrl = ConfigurationManager.AppSettings["TilopayApiUrl"];
                var apiUser = ConfigurationManager.AppSettings["TilopayApiUser"];
                var apiPassword = ConfigurationManager.AppSettings["TilopayApiPassword"];
                var redirectUrl = ConfigurationManager.AppSettings["TilopayRedirectUrl"];

                var referencia = $"JENA-{idUsuario}-{idPlan}-{DateTime.UtcNow.Ticks}";

                // MOCK MODE
                if (string.IsNullOrWhiteSpace(apiUser) || apiUser == "DUMMY_USER")
                {
                    var urlMock = Url.Action(
                        "RespuestaTilopay",
                        "Clientes",
                        new { status = "APPROVED", orderNumber = referencia });

                    return Json(new
                    {
                        success = true,
                        url = urlMock
                    });
                }

                // TODO: [ALERTA TILOPAY] Quitar el bloque Mock Mode de arriba cuando se tengan las credenciales reales de producción.

                var plan =
                    _obtenerPlanMembresiaPorIdServicio
                        .ObtenerPlanMembresiaPorId(idPlan);

                if (plan == null)
                {
                    return Json(new
                    {
                        success = false,
                        mensaje = "El plan seleccionado no es válido."
                    });
                }

                decimal monto = plan.precio;

                string baseUrl = (apiUrl ?? string.Empty).TrimEnd('/');
                string accessToken;

                using (var cliente = new HttpClient())
                {
                    cliente.Timeout = TimeSpan.FromSeconds(30);

                    var loginPayload = new
                    {
                        apiuser = apiUser,
                        password = apiPassword
                    };

                    var loginContenido = new StringContent(
                        JsonConvert.SerializeObject(loginPayload),
                        Encoding.UTF8,
                        "application/json");

                    var loginRespuesta = await cliente.PostAsync(
                        baseUrl + "/api/v1/login",
                        loginContenido);

                    var loginCuerpo =
                        await loginRespuesta.Content.ReadAsStringAsync();

                    if (!loginRespuesta.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException(
                            $"Tilopay login respondió {(int)loginRespuesta.StatusCode}: {loginCuerpo}");
                    }

                    var loginResultado =
                        JsonConvert.DeserializeObject<JObject>(loginCuerpo);

                    accessToken = (string)loginResultado?["access_token"];

                    if (string.IsNullOrWhiteSpace(accessToken))
                    {
                        throw new InvalidOperationException(
                            "Tilopay no devolvió un access_token válido.");
                    }

                    var payload = new
                    {
                        amount = monto.ToString("F2", CultureInfo.InvariantCulture),
                        currency = "CRC",
                        orderNumber = referencia,
                        billToFirstName = "Cliente",
                        billToLastName = "GimnasioJena",
                        subscription = 0,
                        platform = "api",
                        returnData = referencia,
                        redirect = redirectUrl,
                        capture = 1,
                        description = $"Pago membresía plan {idPlan}"
                    };

                    var pagoContenido = new StringContent(
                        JsonConvert.SerializeObject(payload),
                        Encoding.UTF8,
                        "application/json");

                    cliente.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue(
                            "Bearer", accessToken);

                    var pagoRespuesta = await cliente.PostAsync(
                        baseUrl + "/api/v1/processPayment",
                        pagoContenido);

                    var pagoCuerpo =
                        await pagoRespuesta.Content.ReadAsStringAsync();

                    if (!pagoRespuesta.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException(
                            $"Tilopay processPayment respondió {(int)pagoRespuesta.StatusCode}: {pagoCuerpo}");
                    }

                    var pagoResultado =
                        JsonConvert.DeserializeObject<JObject>(pagoCuerpo);

                    string checkoutUrl =
                        (string)(pagoResultado?["url"]
                                 ?? pagoResultado?["urlPay"]
                                 ?? pagoResultado?["checkoutUrl"]);

                    if (string.IsNullOrWhiteSpace(checkoutUrl))
                    {
                        throw new InvalidOperationException(
                            "Tilopay no devolvió una URL de pago válida.");
                    }

                    return Json(new
                    {
                        success = true,
                        url = checkoutUrl
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(
                    "Error al iniciar el pago con Tilopay: " + ex);

                return Json(new
                {
                    success = false,
                    mensaje = "No fue posible iniciar el proceso de pago. " +
                              "Inténtalo de nuevo más tarde."
                });
            }
        }

        [HttpGet]
        public ActionResult RespuestaTilopay(
            string status,
            string orderNumber,
            string transactionId,
            string token)
        {
            if (!string.Equals(status, "APPROVED", StringComparison.Ordinal))
            {
                TempData["MensajeError"] =
                    "El pago no pudo completarse. Si el cargo fue realizado, " +
                    "comunícate con soporte indicando tu número de referencia.";

                return View("ErrorPago");
            }

            try
            {
                if (!TryParseReferencia(orderNumber, out int idUsuario, out int idPlan))
                {
                    TempData["MensajeError"] =
                        "La referencia del pago no es válida.";

                    return View("ErrorPago");
                }

                var referenciaPago = JsonConvert.SerializeObject(new
                {
                    transactionId,
                    orderNumber,
                    token
                });

                /*
                 * Idempotencia: si esta referencia ya fue registrada
                 * (por ejemplo por el webhook), no se duplica el pago.
                 */
                if (_registrarPagoLN.ExisteReferenciaPago(referenciaPago))
                {
                    TempData["MensajeExito"] =
                        "Tu pago ya fue registrado correctamente.";

                    return RedirectToAction("MiMembresia");
                }

                /*
                 * Creación diferida: se crea la membresía del plan elegido
                 * en estado NO activo; el registro del pago confirmado es
                 * quien la activa (mismo flujo que el pago administrativo).
                 */
                int idMembresiaCliente =
                    _registrarMembresiaLN
                        .RegistrarMembresiaPendiente(idUsuario, idPlan);

                if (idMembresiaCliente <= 0)
                {
                    TempData["MensajeError"] =
                        "No fue posible activar la membresía tras el pago. " +
                        "Comunícate con soporte indicando tu número de referencia.";

                    return View("ErrorPago");
                }

                var pagoDto = new PagoCrearDto
                {
                    idMembresiaCliente = idMembresiaCliente,
                    idMetodoPago = LeerEnteroConfig("TilopayMetodoPagoId", 1),
                    idEstadoPago = LeerEnteroConfig("TilopayEstadoPagoAprobado", 2),
                    fechaPago = DateTime.Now,
                    referenciaPago = referenciaPago
                };

                int idPago = _registrarPagoLN.RegistrarPago(pagoDto);

                if (idPago <= 0)
                {
                    TempData["MensajeError"] =
                        "El pago fue aprobado pero no pudo registrarse. " +
                        "Comunícate con soporte indicando tu número de referencia.";

                    return View("ErrorPago");
                }

                _registrarBitacoraLN.RegistrarBitacora(new BitacoraDto
                {
                    idUsuario = idUsuario,
                    tablaAfectada = "Pago",
                    accionRealizada = "PAGO_TILOPAY_APROBADO",
                    idRegistroAfectado = idPago,
                    detalle =
                        $"Pago Tilopay aprobado. Membresía {idMembresiaCliente} activada.",
                    ipUsuario = Request?.UserHostAddress
                });

                TempData["MensajeExito"] =
                    "Tu pago fue aprobado y tu membresía fue activada correctamente.";

                return RedirectToAction("MiMembresia");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(
                    "Error procesando la respuesta de Tilopay: " + ex);

                TempData["MensajeError"] =
                    "Ocurrió un error al procesar tu pago. " +
                    "Comunícate con soporte indicando tu número de referencia.";

                return View("ErrorPago");
            }
        }

        private static int LeerEnteroConfig(string clave, int valorPorDefecto)
        {
            var valor = ConfigurationManager.AppSettings[clave];

            return int.TryParse(valor, out int resultado)
                ? resultado
                : valorPorDefecto;
        }

        private static bool TryParseReferencia(
            string referencia,
            out int idUsuario,
            out int idPlan)
        {
            idUsuario = 0;
            idPlan = 0;

            if (string.IsNullOrWhiteSpace(referencia))
                return false;

            var coincidencia =
                Regex.Match(referencia, @"JENA-(\d+)-(\d+)");

            if (!coincidencia.Success)
                return false;

            return int.TryParse(coincidencia.Groups[1].Value, out idUsuario)
                   && int.TryParse(coincidencia.Groups[2].Value, out idPlan)
                   && idUsuario > 0
                   && idPlan > 0;
        }
#endif
        // TODO: [TILOPAY EN PAUSA] Fin del bloque de integración Tilopay en pausa.

        private async Task<OperacionMembresiaDto> PrepararOperacionMembresia(int idPlan)
        {
            if (idPlan <= 0)
                return null;

            var identityUserId = User.Identity.GetUserId();

            var perfil =
                await _obtenerUsuarioServicio
                    .ObtenerUsuarioPorId(identityUserId);

            if (perfil == null)
                return null;

            var plan =
                _obtenerPlanMembresiaPorIdServicio
                    .ObtenerPlanMembresiaPorId(idPlan);

            if (plan == null)
                return null;

            var membresiaActual =
                _obtenerMembresiaPorClienteServicio
                    .ObtenerUltimaMembresiaPorCliente(
                        perfil.idUsuario);

            DateTime hoy = DateTime.Today;

            if (membresiaActual != null &&
                membresiaActual.fechaFin.Date > hoy)
            {
                return null;
            }

            string tipoOperacion;

            if (membresiaActual == null)
            {
                tipoOperacion = "Adquisición";
            }
            else if (membresiaActual.idPlanMembresia ==
                     plan.idPlanMembresia)
            {
                tipoOperacion = "Renovación";
            }
            else
            {
                tipoOperacion = "Cambio de plan";
            }

            return new OperacionMembresiaDto
            {
                idUsuario = perfil.idUsuario,
                idPlan = plan.idPlanMembresia,
                nombrePlan = plan.nombrePlan,
                precio = plan.precio,
                duracionDias = plan.duracionDias,
                tipoOperacion = tipoOperacion,
                fechaInicioPropuesta = hoy,
                fechaFinPropuesta = hoy.AddDays(plan.duracionDias - 1),
                clasesAsignadas = plan.cantidadClases,
                descripcionPlan =
                    plan.cantidadClases.HasValue
                    ? $"Incluye {plan.cantidadClases.Value} clases durante {plan.duracionDias} días."
                    : $"Incluye clases ilimitadas durante {plan.duracionDias} días."
            };
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