using GimnasioJena.Abstracciones.LogicaDeNegocio.Pagos.RegistrarPago;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.RegistrarMembresia;
using GimnasioJena.Abstracciones.Modelos.Pagos;
using GimnasioJena.LogicaDeNegocio.Pagos.RegistrarPago;
using GimnasioJena.LogicaDeNegocio.Membresias.RegistrarMembresia;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

// TODO: [TILOPAY EN PAUSA] Descomentar y continuar cuando se tengan credenciales reales.
#if false
namespace GimnasioJena.UI.Controllers
{
    [AllowAnonymous]
    public class WebhookController : Controller
    {
        private readonly IRegistrarPagoLN _registrarPagoLN;
        private readonly IRegistrarMembresiaLN _registrarMembresiaLN;

        public WebhookController()
        {
            _registrarPagoLN = new RegistrarPagoLN();
            _registrarMembresiaLN = new RegistrarMembresiaLN();
        }

        [HttpPost]
        public async Task<ActionResult> Tilopay()
        {
            try
            {
                string cuerpo;

                Request.InputStream.Position = 0;

                using (var lector = new StreamReader(
                    Request.InputStream, Encoding.UTF8, true, 1024, true))
                {
                    cuerpo = await lector.ReadToEndAsync();
                }

                /*
                 * 1. Validación de la firma/hash de seguridad de Tilopay.
                 *    Un cuerpo no firmado o con firma inválida se descarta
                 *    silenciosamente respondiendo 200 para evitar reintentos
                 *    maliciosos, sin registrar nada.
                 */
                if (!FirmaEsValida(cuerpo))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }

                JObject datos = string.IsNullOrWhiteSpace(cuerpo)
                    ? new JObject()
                    : JObject.Parse(cuerpo);

                /*
                 * 2. Extracción del estado del pago.
                 */
                string status = LeerCampo(datos, "status", "orderStatus", "code");

                if (!string.Equals(status, "APPROVED", StringComparison.Ordinal))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }

                string transactionId =
                    LeerCampo(datos, "transactionId", "transaction", "tilopay-transaction");
                string token =
                    LeerCampo(datos, "token", "OrderHash", "orderHash");
                string orderNumber =
                    LeerCampo(datos, "orderNumber", "reference", "returnData");

                /*
                 * Se estructura la referencia igual que en RespuestaTilopay
                 * para que la idempotencia funcione sin importar qué canal
                 * (navegador o webhook) confirme primero el pago.
                 */
                string referenciaPago = JsonConvert.SerializeObject(new
                {
                    transactionId,
                    orderNumber,
                    token
                });

                /*
                 * 3. Verificación de idempotencia: si la referencia ya existe
                 *    en base de datos, se evita el doble registro.
                 */
                if (_registrarPagoLN.ExisteReferenciaPago(referenciaPago))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }

                /*
                 * 4. Creación diferida: se extrae idUsuario e idPlan de la
                 *    referencia (JENA-{idUsuario}-{idPlan}-{ticks}), se crea la
                 *    membresía en estado NO activo y el registro del pago la
                 *    activa, reutilizando estrictamente las interfaces de LN.
                 */
                if (TryParseReferencia(orderNumber, out int idUsuario, out int idPlan))
                {
                    int idMembresiaCliente =
                        _registrarMembresiaLN
                            .RegistrarMembresiaPendiente(idUsuario, idPlan);

                    if (idMembresiaCliente > 0)
                    {
                        var pagoDto = new PagoCrearDto
                        {
                            idMembresiaCliente = idMembresiaCliente,
                            idMetodoPago = LeerEnteroConfig("TilopayMetodoPagoId", 1),
                            idEstadoPago = LeerEnteroConfig("TilopayEstadoPagoAprobado", 2),
                            fechaPago = DateTime.Now,
                            referenciaPago = referenciaPago
                        };

                        _registrarPagoLN.RegistrarPago(pagoDto);
                    }
                }

                /*
                 * 5. Respuesta 200 OK rápida.
                 */
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(
                    "Error procesando webhook de Tilopay: " + ex);

                /*
                 * Se responde 200 para no provocar reintentos en cascada.
                 * El error queda registrado en el log del servidor.
                 */
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }

        private static bool FirmaEsValida(string cuerpo)
        {
            var secreto =
                ConfigurationManager.AppSettings["TilopayWebhookSecret"];

            /*
             * Si no hay secreto configurado no se puede validar la firma;
             * se rechaza por seguridad.
             */
            if (string.IsNullOrWhiteSpace(secreto))
                return false;

            var firmaRecibida =
                System.Web.HttpContext.Current.Request.Headers["tilopay-signature"]
                ?? System.Web.HttpContext.Current.Request.Headers["Tilopay-Signature"]
                ?? System.Web.HttpContext.Current.Request.Headers["x-tilopay-signature"];

            if (string.IsNullOrWhiteSpace(firmaRecibida))
                return false;

            string firmaCalculada = CalcularHmacSha256(cuerpo ?? string.Empty, secreto);

            return ComparacionSegura(firmaRecibida.Trim(), firmaCalculada);
        }

        private static string CalcularHmacSha256(string mensaje, string secreto)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secreto)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(mensaje));

                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        private static bool ComparacionSegura(string a, string b)
        {
            if (a == null || b == null || a.Length != b.Length)
                return false;

            int resultado = 0;

            for (int i = 0; i < a.Length; i++)
            {
                resultado |= a[i] ^ b[i];
            }

            return resultado == 0;
        }

        private static string LeerCampo(JObject datos, params string[] nombres)
        {
            foreach (var nombre in nombres)
            {
                var token = datos[nombre];

                if (token != null && token.Type != JTokenType.Null)
                {
                    return token.ToString();
                }
            }

            return null;
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

            /*
             * Referencia con formato "JENA-{idUsuario}-{idPlan}-{ticks}".
             */
            var coincidencia = Regex.Match(referencia, @"JENA-(\d+)-(\d+)");

            if (!coincidencia.Success)
                return false;

            return int.TryParse(coincidencia.Groups[1].Value, out idUsuario)
                   && int.TryParse(coincidencia.Groups[2].Value, out idPlan)
                   && idUsuario > 0
                   && idPlan > 0;
        }
    }
}
#endif
// TODO: [TILOPAY EN PAUSA] Fin del endpoint Webhook de Tilopay en pausa.
