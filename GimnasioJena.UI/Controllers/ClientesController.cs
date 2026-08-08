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

        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<ActionResult> IniciarPago(int idPlan)
{
    try
    {
        var identityUserId = User.Identity.GetUserId();

        var perfil =
            await _obtenerUsuarioServicio
                .ObtenerUsuarioPorId(identityUserId);

        if (perfil == null)
        {
            return Json(new
            {
                success = false,
                mensaje = "No se encontró la información del usuario."
            });
        }

        int idUsuario = perfil.idUsuario;

        var apiUrl =
            ConfigurationManager.AppSettings["TilopayApiUrl"];

        var apiKey =
            ConfigurationManager.AppSettings["TilopayApiKey"];

        var apiUser =
            ConfigurationManager.AppSettings["TilopayApiUser"];

        var apiPassword =
            ConfigurationManager.AppSettings["TilopayApiPassword"];

        var redirectUrl =
            ConfigurationManager.AppSettings["TilopayRedirectUrl"];

        var referencia =
            $"JENA-{idUsuario}-{idPlan}-{DateTime.UtcNow.Ticks}";

        // MOCK MODE
        if (string.IsNullOrWhiteSpace(apiUser) ||
            apiUser == "DUMMY_USER")
        {
            var urlMock = Url.Action(
                "RespuestaTilopay",
                "Clientes",
                new
                {
                    status = "APPROVED",
                    orderNumber = referencia
                });

            return Json(new
            {
                success = true,
                url = urlMock
            });
        }

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

        string baseUrl =
            (apiUrl ?? string.Empty).TrimEnd('/');

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

            var loginRespuesta =
                await cliente.PostAsync(
                    baseUrl + "/login",
                    loginContenido);

            var loginCuerpo =
                await loginRespuesta.Content.ReadAsStringAsync();

            if (!loginRespuesta.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Tilopay login respondió {(int)loginRespuesta.StatusCode}: {loginCuerpo}");
            }

            var loginResultado =
                JsonConvert.DeserializeObject<JObject>(
                    loginCuerpo);

            accessToken =
                (string)loginResultado?["access_token"];

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new InvalidOperationException(
                    "Tilopay no devolvió un access_token válido.");
            }

            var payload = new
            {
                redirect = redirectUrl,
                key = apiKey,
                amount = monto.ToString(
                    "F2",
                    CultureInfo.InvariantCulture),
                currency = "CRC",

                billToFirstName = "Cliente",
                billToLastName = "GimnasioJena",
                billToEmail = perfil.correo,

                orderNumber = referencia,
                capture = "1",
                subscription = "0",
                platform = "api",
                returnData = referencia,
                hashVersion = "V2"
            };

            var pagoContenido = new StringContent(
                JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json");

            cliente.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    accessToken);

            cliente.DefaultRequestHeaders.Accept.Clear();

            cliente.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(
                    "application/json"));

            var pagoRespuesta =
                await cliente.PostAsync(
                    baseUrl + "/processPayment",
                    pagoContenido);

            var pagoCuerpo =
                await pagoRespuesta.Content.ReadAsStringAsync();

            if (!pagoRespuesta.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Tilopay processPayment respondió {(int)pagoRespuesta.StatusCode}: {pagoCuerpo}");
            }

            var pagoResultado =
                JsonConvert.DeserializeObject<JObject>(
                    pagoCuerpo);

            string checkoutUrl =
                (string)pagoResultado?["url"];

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
            mensaje =
                "No fue posible iniciar el proceso de pago. " +
                "Inténtalo de nuevo más tarde."
        });
    }
}

[HttpGet]
public async Task<ActionResult> RespuestaTilopay(
    string code,
    string description,
    string order,
    string tpt,
    string OrderHash,
    string returnData)
{
    try
    {
        /*
         * La respuesta del navegador NO se considera confirmación
         * suficiente del pago.
         *
         * "order" solamente se utiliza para consultar posteriormente
         * la transacción directamente contra Tilopay.
         */
        if (string.IsNullOrWhiteSpace(order))
        {
            TempData["MensajeError"] =
                "No se recibió una referencia válida del pago.";

            return View("ErrorPago");
        }

        if (!TryParseReferencia(
            order,
            out int idUsuario,
            out int idPlan))
        {
            TempData["MensajeError"] =
                "La referencia del pago no es válida.";

            return View("ErrorPago");
        }

        /*
         * Seguridad:
         * verificamos que la referencia corresponda al usuario
         * que actualmente está autenticado.
         */
        var identityUserId = User.Identity.GetUserId();

        var perfil =
            await _obtenerUsuarioServicio
                .ObtenerUsuarioPorId(identityUserId);

        if (perfil == null ||
            perfil.idUsuario != idUsuario)
        {
            TempData["MensajeError"] =
                "No fue posible validar el usuario asociado al pago.";

            return View("ErrorPago");
        }

        var apiUrl =
            ConfigurationManager.AppSettings["TilopayApiUrl"];

        var apiKey =
            ConfigurationManager.AppSettings["TilopayApiKey"];

        var apiUser =
            ConfigurationManager.AppSettings["TilopayApiUser"];

        var apiPassword =
            ConfigurationManager.AppSettings["TilopayApiPassword"];

        if (string.IsNullOrWhiteSpace(apiUrl) ||
            string.IsNullOrWhiteSpace(apiKey) ||
            string.IsNullOrWhiteSpace(apiUser) ||
            string.IsNullOrWhiteSpace(apiPassword))
        {
            throw new InvalidOperationException(
                "La configuración de Tilopay está incompleta.");
        }

        string baseUrl =
            apiUrl.TrimEnd('/');

        JObject transaccionTilopay;

        using (var cliente = new HttpClient())
        {
            cliente.Timeout =
                TimeSpan.FromSeconds(30);

            /*
             * 1. Obtener token oficial de Tilopay.
             */
            var loginPayload = new
            {
                apiuser = apiUser,
                password = apiPassword
            };

            var loginContenido = new StringContent(
                JsonConvert.SerializeObject(loginPayload),
                Encoding.UTF8,
                "application/json");

            var loginRespuesta =
                await cliente.PostAsync(
                    baseUrl + "/login",
                    loginContenido);

            var loginCuerpo =
                await loginRespuesta.Content
                    .ReadAsStringAsync();

            if (!loginRespuesta.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Tilopay login respondió " +
                    $"{(int)loginRespuesta.StatusCode}: " +
                    loginCuerpo);
            }

            var loginResultado =
                JsonConvert.DeserializeObject<JObject>(
                    loginCuerpo);

            string accessToken =
                (string)loginResultado?["access_token"];

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new InvalidOperationException(
                    "Tilopay no devolvió un access_token válido.");
            }

            cliente.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers
                    .AuthenticationHeaderValue(
                        "Bearer",
                        accessToken);

            cliente.DefaultRequestHeaders.Accept.Clear();

            cliente.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers
                    .MediaTypeWithQualityHeaderValue(
                        "application/json"));

            /*
             * 2. Consultar la transacción directamente en Tilopay.
             */
            var consultaPayload = new
            {
                key = apiKey,
                orderNumber = order,
                merchantId = ""
            };

            var consultaContenido =
                new StringContent(
                    JsonConvert.SerializeObject(
                        consultaPayload),
                    Encoding.UTF8,
                    "application/json");

            var consultaRespuesta =
                await cliente.PostAsync(
                    baseUrl + "/consult",
                    consultaContenido);

            var consultaCuerpo =
                await consultaRespuesta.Content
                    .ReadAsStringAsync();

                    if (!consultaRespuesta.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Tilopay consult respondió " +
                    $"{(int)consultaRespuesta.StatusCode}: " +
                    consultaCuerpo);
            }

            var consultaResultado =
                JsonConvert.DeserializeObject<JObject>(
                    consultaCuerpo);

            var transacciones =
                consultaResultado?["response"] as JArray;

            if (transacciones == null ||
                transacciones.Count == 0)
            {
                TempData["MensajeError"] =
                    "Tilopay no confirmó la transacción solicitada.";

                return View("ErrorPago");
            }

                    /*
                     * Localizamos exactamente la transacción
                     * correspondiente al orderNumber generado por Jéna.
                     */
            transaccionTilopay =
                transacciones
                        .OfType<JObject>()
                        .FirstOrDefault(t =>
                        {
                            string orderNumberTilopay =
                                (string)t["orderNumber"];

                            return !string.IsNullOrWhiteSpace(orderNumberTilopay)
                                   && orderNumberTilopay.EndsWith(
                                       order,
                                       StringComparison.Ordinal);
                        });

                    if (transaccionTilopay == null)
            {
                TempData["MensajeError"] =
                    "La transacción devuelta por Tilopay " +
                    "no corresponde a la operación solicitada.";

                return View("ErrorPago");
            }
        }

        /*
         * 3. Tilopay documenta code = "1"
         * como transacción aprobada.
         */
        string codigoTilopay =
            (string)transaccionTilopay["code"];

        if (!string.Equals(
            codigoTilopay,
            "1",
            StringComparison.Ordinal))
        {
            TempData["MensajeError"] =
                "El pago no fue aprobado por Tilopay.";

            return View("ErrorPago");
        }

        /*
         * 4. Validar plan, monto y moneda contra
         * información controlada por el servidor.
         */
        var plan =
            _obtenerPlanMembresiaPorIdServicio
                .ObtenerPlanMembresiaPorId(idPlan);

        if (plan == null)
        {
            TempData["MensajeError"] =
                "El plan asociado al pago no es válido.";

            return View("ErrorPago");
        }

        string monedaTilopay =
            (string)transaccionTilopay["currency"];

        string montoTexto =
            (string)transaccionTilopay["amount"];

        if (!decimal.TryParse(
            montoTexto,
            NumberStyles.Number,
            CultureInfo.InvariantCulture,
            out decimal montoTilopay))
        {
            TempData["MensajeError"] =
                "No fue posible validar el monto recibido de Tilopay.";

            return View("ErrorPago");
        }

        if (!string.Equals(
                monedaTilopay,
                "CRC",
                StringComparison.OrdinalIgnoreCase) ||
            montoTilopay != plan.precio)
        {
            TempData["MensajeError"] =
                "Los datos del pago no coinciden con el plan seleccionado.";

            return View("ErrorPago");
        }

        /*
         * 5. La referencia idempotente será el identificador
         * real de la transacción de Tilopay.
         */
        string idTilopay =
            transaccionTilopay["id_tilopay"]?
                .ToString();

        if (string.IsNullOrWhiteSpace(idTilopay))
        {
            throw new InvalidOperationException(
                "Tilopay no devolvió un identificador de transacción válido.");
        }

        string referenciaPago =
            "TILOPAY-" + idTilopay;

        /*
         * 6. Idempotencia.
         */
        if (_registrarPagoLN
            .ExisteReferenciaPago(referenciaPago))
        {
            TempData["MensajeExito"] =
                "Tu pago ya fue registrado correctamente.";

            return RedirectToAction("MiMembresia");
        }

        /*
         * 7. Crear membresía pendiente.
         */
        int idMembresiaCliente =
            _registrarMembresiaLN
                .RegistrarMembresiaPendiente(
                    idUsuario,
                    idPlan);

        if (idMembresiaCliente <= 0)
        {
            TempData["MensajeError"] =
                "El pago fue aprobado, pero no fue posible " +
                "preparar la membresía. Comunícate con soporte.";

            return View("ErrorPago");
        }

        string autorizacion =
            (string)transaccionTilopay["auth"];

        string ambiente =
            (string)transaccionTilopay["environment"];

        var pagoDto = new PagoCrearDto
        {
            idMembresiaCliente =
                idMembresiaCliente,

            idMetodoPago =
                LeerEnteroConfig(
                    "TilopayMetodoPagoId",
                    5),

            idEstadoPago =
                LeerEnteroConfig(
                    "TilopayEstadoPagoAprobado",
                    2),

            fechaPago =
                DateTime.Now,

            referenciaPago =
                referenciaPago,

            observaciones =
                $"Tilopay Order: {order}. " +
                $"Auth: {autorizacion}. " +
                $"Ambiente: {ambiente}."
        };

        /*
         * RegistrarPagoLN vuelve a obtener el precio
         * oficial del plan desde la BD.
         */
        int idPago =
            _registrarPagoLN
                .RegistrarPago(pagoDto);

        if (idPago <= 0)
        {
            TempData["MensajeError"] =
                "El pago fue aprobado pero no pudo registrarse. " +
                "Comunícate con soporte indicando tu referencia.";

            return View("ErrorPago");
        }

        _registrarBitacoraLN
            .RegistrarBitacora(
                new BitacoraDto
                {
                    idUsuario =
                        idUsuario,

                    tablaAfectada =
                        "Pago",

                    accionRealizada =
                        "INSERT",

                    idRegistroAfectado =
                        idPago,

                    detalle =
                        $"Pago Tilopay {idTilopay} aprobado. " +
                        $"Membresía {idMembresiaCliente} activada.",

                    ipUsuario =
                        Request?.UserHostAddress
                });

        TempData["MensajeExito"] =
            "Tu pago fue aprobado y tu membresía " +
            "fue activada correctamente.";

        return RedirectToAction(
            "MiMembresia");
    }
    catch (Exception ex)
    {
        System.Diagnostics.Trace.TraceError(
            "Error procesando la respuesta de Tilopay: " +
            ex);

        TempData["MensajeError"] =
            "Ocurrió un error al verificar tu pago. " +
            "Comunícate con soporte indicando tu referencia.";

        return View("ErrorPago");
    }
}


private static int LeerEnteroConfig(
    string clave,
    int valorPorDefecto)
{
    var valor =
        ConfigurationManager.AppSettings[clave];

    return int.TryParse(
        valor,
        out int resultado)
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
        Regex.Match(
            referencia,
            @"^JENA-(\d+)-(\d+)-\d+$");

    if (!coincidencia.Success)
        return false;

    return
        int.TryParse(
            coincidencia.Groups[1].Value,
            out idUsuario)
        &&
        int.TryParse(
            coincidencia.Groups[2].Value,
            out idPlan)
        &&
        idUsuario > 0
        &&
        idPlan > 0;
}

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