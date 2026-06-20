using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.RegistrarReserva;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.AccesoADatos;
using GimnasioJena.LogicaDeNegocio.Reservas.RegistrarReserva;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [Authorize]
    public class ReservasController : Controller
    {
        private readonly IRegistrarReservaLN _registrarReservaServicio;

        public ReservasController()
        {
            _registrarReservaServicio = new RegistrarReservaLN();
        }

        [HttpGet]
        [Authorize(Roles = "CLIENTE")]
        public ActionResult ReservarClase(int id)
        {
            using (var contexto = new Contexto())
            {
                var clase = contexto.Clases.FirstOrDefault(c => c.idClaseProgramada == id);

                if (clase == null)
                {
                    TempData["MensajeError"] = "No se encontró la clase seleccionada.";
                    return RedirectToAction("ObtenerTodasLasClases", "Clases");
                }

                var modelo = new ReservaCrearDto
                {
                    idClaseProgramada = id,
                    idEstadoReserva = 1
                };

                return View(modelo);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CLIENTE")]
        public ActionResult ReservarClase(ReservaCrearDto modelo)
        {
            if (modelo == null || modelo.idClaseProgramada <= 0)
            {
                TempData["MensajeError"] = "No se recibió información válida para reservar.";
                return RedirectToAction("ObtenerTodasLasClases", "Clases");
            }

            using (var contexto = new Contexto())
            {
                var identityUserId = User.Identity.GetUserId();

                var usuario = contexto.Usuarios
                    .FirstOrDefault(u => u.identityUserId == identityUserId);

                if (usuario == null)
                {
                    TempData["MensajeError"] = "No se encontró el usuario actual.";
                    return RedirectToAction("ObtenerTodasLasClases", "Clases");
                }

                modelo.idUsuario = usuario.idUsuario;
                modelo.idEstadoReserva = 1;

                bool resultado = _registrarReservaServicio.RegistrarReserva(modelo);

                if (resultado)
                {
                    TempData["MensajeExito"] = "La reserva se registró correctamente.";
                    return RedirectToAction("ObtenerTodasLasClases", "Clases");
                }

                TempData["MensajeError"] = "No se pudo registrar la reserva.";
                return View(modelo);
            }
        }
    }
}