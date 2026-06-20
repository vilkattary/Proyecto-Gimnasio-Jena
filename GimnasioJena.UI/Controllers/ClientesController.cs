using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerReservasPorUsuario;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.AccesoADatos;
using GimnasioJena.LogicaDeNegocio.Reservas.ObtenerReservasPorUsuario;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [Authorize(Roles = "CLIENTE")]
    public class ClientesController : Controller
    {
        private readonly IObtenerUsuarioPorIdLN _obtenerUsuarioServicio;
        private readonly IObtenerReservasPorUsuarioLN _obtenerReservasPorUsuarioServicio;

        public ClientesController(IObtenerUsuarioPorIdLN obtenerUsuarioServicio)
        {
            _obtenerUsuarioServicio = obtenerUsuarioServicio;
            _obtenerReservasPorUsuarioServicio = new ObtenerReservasPorUsuarioLN();
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
            var perfil = await _obtenerUsuarioServicio.ObtenerUsuarioPorId(identityUserId);
            return View(perfil);
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

        public async Task<ActionResult> EliminarReserva(int id)
        {
            var identityUserId = User.Identity.GetUserId();
            var perfil = await _obtenerUsuarioServicio.ObtenerUsuarioPorId(identityUserId);
            if (perfil == null)
            {
                TempData["MensajeError"] = "Usuario no autenticado correctamente.";
                return RedirectToAction("MisReservas");
            }

            using (var contexto = new Contexto())
            {
                var reserva = contexto.Reservas.Find(id);
                if (reserva == null || reserva.idUsuario != perfil.idUsuario)
                {
                    TempData["MensajeError"] = "No se encontró la reserva o no tienes permiso para cancelarla.";
                    return RedirectToAction("MisReservas");
                }

                contexto.Reservas.Remove(reserva);
                contexto.SaveChanges();
            }
            TempData["MensajeExito"] = "Reserva cancelada correctamente.";
            return RedirectToAction("MisReservas");
        }
    }
}