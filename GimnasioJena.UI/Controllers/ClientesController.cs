using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.CancelarReserva;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.ObtenerReservasPorUsuario;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId;
using GimnasioJena.Abstracciones.Modelos.Reservas;
using GimnasioJena.AccesoADatos;
using GimnasioJena.LogicaDeNegocio.Reservas.CancelarReserva;
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
        private readonly ICancelarReservaLN _cancelarReservaServicio;

        public ClientesController(IObtenerUsuarioPorIdLN obtenerUsuarioServicio)
        {
            _obtenerUsuarioServicio = obtenerUsuarioServicio;
            _obtenerReservasPorUsuarioServicio = new ObtenerReservasPorUsuarioLN();
            _cancelarReservaServicio = new CancelarReservaLN();
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

            bool resultado = _cancelarReservaServicio.CancelarReserva(id, perfil.idUsuario);

            if (resultado)
            {
                TempData["MensajeExito"] = "Reserva cancelada correctamente.";
            }
            else
            {
                TempData["MensajeError"] = "No se pudo cancelar la reserva. Verifica que esté activa.";
            }

            return RedirectToAction("MisReservas");
        }
    }
}