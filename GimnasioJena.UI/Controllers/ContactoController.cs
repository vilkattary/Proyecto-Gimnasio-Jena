using GimnasioJena.Abstracciones.LogicaDeNegocio.Contacto;
using GimnasioJena.Abstracciones.Modelos.Contacto;
using GimnasioJena.LogicaDeNegocio.Contacto;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace GimnasioJena.UI.Controllers
{
    public class ContactoController : Controller
    {
        private readonly IContactoLN _contactoLN;

        public ContactoController()
        {
            _contactoLN = new ContactoLN();
        }

        private async Task<bool> ValidarTurnstileAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            string secretKey =
                ConfigurationManager.AppSettings["TurnstileSecretKey"];

            if (string.IsNullOrWhiteSpace(secretKey))
                return false;

            try
            {
                using (var cliente = new HttpClient())
                {
                    var datos = new FormUrlEncodedContent(
                        new Dictionary<string, string>
                        {
                    { "secret", secretKey },
                    { "response", token }
                        });

                    var respuesta = await cliente.PostAsync(
                        "https://challenges.cloudflare.com/turnstile/v0/siteverify",
                        datos);

                    if (!respuesta.IsSuccessStatusCode)
                        return false;

                    string contenido =
                        await respuesta.Content.ReadAsStringAsync();

                    JObject resultado = JObject.Parse(contenido);

                    return resultado.Value<bool>("success");
                }
            }
            catch
            {
                // Si Cloudflare no responde o ocurre un error,
                // no se considera válida la verificación.
                return false;
            }
        }

        [HttpGet]
        public async Task<ActionResult> Index(string disciplina = "")
        {
            try
            {
                var info = await _contactoLN.ObtenerInfoContactoAsync();
                ViewBag.Direccion = info?.Direccion;
                ViewBag.Telefono  = info?.Telefono;
                ViewBag.Correo    = info?.Correo;
            }
            catch
            {
                ViewBag.Direccion = null;
                ViewBag.Telefono  = null;
                ViewBag.Correo    = null;
            }

            var dto = new ContactoFormDto();

            if (!string.IsNullOrWhiteSpace(disciplina))
            {
                dto.Asunto  = "Clase de Prueba - " + disciplina;
                dto.Mensaje = "Hola, me gustaria agendar una clase de prueba para la disciplina: " + disciplina + ".";
            }

            return View("FormularioContacto", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnviarMensaje(ContactoFormDto dto)
        {
            // Obtener el token generado por Cloudflare Turnstile.
            string tokenTurnstile =
                Request.Form["cf-turnstile-response"];

            // Validar el token con Cloudflare.
            bool captchaValido =
                await ValidarTurnstileAsync(tokenTurnstile);

            if (!captchaValido)
            {
                ModelState.AddModelError(
                    "",
                    "No se pudo verificar la seguridad del formulario. " +
                    "Por favor, inténtalo nuevamente.");
            }

            // Validar tanto los campos del formulario como el CAPTCHA.
            if (!ModelState.IsValid)
            {
                try
                {
                    var info = await _contactoLN.ObtenerInfoContactoAsync();

                    ViewBag.Direccion = info?.Direccion;
                    ViewBag.Telefono = info?.Telefono;
                    ViewBag.Correo = info?.Correo;
                }
                catch
                {
                    ViewBag.Direccion = null;
                    ViewBag.Telefono = null;
                    ViewBag.Correo = null;
                }

                return View("FormularioContacto", dto);
            }

            try
            {
                await _contactoLN.ProcesarNuevoMensajeAsync(dto);

                TempData["MensajeExito"] =
                    "Mensaje enviado correctamente. Revisa tu correo para la confirmacion.";
            }
            catch
            {
                TempData["MensajeError"] =
                    "Ocurrió un error al enviar el mensaje. Inténtalo nuevamente más tarde.";
            }

            return RedirectToAction("Index");
        }
    }
}
