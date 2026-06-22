using GimnasioJena.Abstracciones.LogicaDeNegocio.Contacto;
using GimnasioJena.Abstracciones.Modelos.Contacto;
using GimnasioJena.LogicaDeNegocio.Contacto;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    public class ContactoController : Controller
    {
        private readonly IContactoLN _contactoLN;

        public ContactoController()
        {
            _contactoLN = new ContactoLN();
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
            if (!ModelState.IsValid)
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

                return View("FormularioContacto", dto);
            }

            try
            {
                await _contactoLN.ProcesarNuevoMensajeAsync(dto);
                TempData["MensajeExito"] = "Mensaje enviado correctamente. Revisad tu correo para la confirmacion.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Error al enviar el mensaje: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
