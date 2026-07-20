using GimnasioJena.Abstracciones.LogicaDeNegocio.Bitacora;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Pagos.ObtenerTodosLosPagos;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Pagos.RegistrarPago;
using GimnasioJena.Abstracciones.Modelos.Bitacora;
using GimnasioJena.Abstracciones.Modelos.Pagos;
using GimnasioJena.AccesoADatos;
using GimnasioJena.LogicaDeNegocio.Bitacora;
using GimnasioJena.LogicaDeNegocio.Pagos.ObtenerTodosLosPagos;
using GimnasioJena.LogicaDeNegocio.Pagos.RegistrarPago;
using Microsoft.AspNet.Identity;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class PagosController : Controller
    {
        private readonly IObtenerTodosLosPagosLN _obtenerTodosLosPagosLN;
        private readonly IRegistrarPagoLN _registrarPagoLN;
        private readonly IRegistrarBitacoraLN _registrarBitacoraLN;

        public PagosController()
        {
            _obtenerTodosLosPagosLN = new ObtenerTodosLosPagosLN();
            _registrarPagoLN = new RegistrarPagoLN();
            _registrarBitacoraLN = new RegistrarBitacoraLN();
        }

        public ActionResult Index()
        {
            var pagos = _obtenerTodosLosPagosLN.ObtenerTodosLosPagos();
            return View(pagos);
        }

        [HttpGet]
        public ActionResult Create()
        {
            CargarCombos();

            var modelo = new PagoCrearDto
            {
                fechaPago = DateTime.Now,
                idEstadoPago = 2
            };

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PagoCrearDto modelo)
        {
            if (!ModelState.IsValid)
            {
                CargarCombos();
                return View(modelo);
            }

            int idPagoRegistrado =
                _registrarPagoLN.RegistrarPago(modelo);

            if (idPagoRegistrado > 0)
            {
                string nombreEstadoPago = ObtenerNombreEstadoPago(modelo.idEstadoPago);

                RegistrarBitacoraPago(
                    idPagoRegistrado,
                    modelo,
                    nombreEstadoPago
                );

                TempData["MensajeExito"] =
                    modelo.idEstadoPago == 2
                        ? "Pago registrado correctamente y membresía actualizada."
                        : "Pago registrado correctamente.";

                return RedirectToAction("Index");
            }

            TempData["MensajeError"] =
                "No se pudo registrar el pago. Verifica la información ingresada.";

            CargarCombos();
            return View(modelo);
        }

        [HttpGet]
        public JsonResult ObtenerPrecioMembresia(int idMembresiaCliente)
        {
            using (var contexto = new Contexto())
            {
                var datos = (
                    from membresia in contexto.Membresias
                    join plan in contexto.PlanesMembresia
                        on membresia.idPlanMembresia equals plan.idPlanMembresia
                    where membresia.idMembresiaCliente == idMembresiaCliente
                    select new
                    {
                        plan.precio
                    }
                ).FirstOrDefault();

                if (datos == null)
                {
                    return Json(
                        new
                        {
                            exito = false,
                            mensaje = "No se encontró la membresía o su plan."
                        },
                        JsonRequestBehavior.AllowGet
                    );
                }

                return Json(
                    new
                    {
                        exito = true,
                        precio = datos.precio.ToString(
                            "0.00",
                            CultureInfo.InvariantCulture
                        )
                    },
                    JsonRequestBehavior.AllowGet
                );
            }
        }
        private void CargarCombos()
        {
            using (var contexto = new Contexto())
            {
                ViewBag.Membresias = contexto.Membresias
                    .Join(
                        contexto.Usuarios,
                        membresia => membresia.idUsuario,
                        usuario => usuario.idUsuario,
                        (membresia, usuario) => new
                        {
                            membresia.idMembresiaCliente,
                            nombreCliente = usuario.nombre + " " +
                                            usuario.apellido1 + " " +
                                            usuario.apellido2,
                            membresia.idPlanMembresia
                        })
                    .Join(
                        contexto.PlanesMembresia,
                        resultado => resultado.idPlanMembresia,
                        plan => plan.idPlanMembresia,
                        (resultado, plan) => new
                        {
                            resultado.idMembresiaCliente,
                            texto = resultado.nombreCliente + " - " + plan.nombrePlan
                        })
                    .OrderBy(x => x.texto)
                    .Select(x => new SelectListItem
                    {
                        Value = x.idMembresiaCliente.ToString(),
                        Text = x.texto
                    })
                    .ToList();

                ViewBag.MetodosPago = contexto.MetodoPagos
                    .Where(m => m.estado)
                    .OrderBy(m => m.nombreMetodo)
                    .Select(m => new SelectListItem
                    {
                        Value = m.idMetodoPago.ToString(),
                        Text = m.nombreMetodo
                    })
                    .ToList();

                ViewBag.EstadosPago = contexto.EstadoPagos
                    .Where(e => e.estado)
                    .OrderBy(e => e.idEstadoPago)
                    .Select(e => new SelectListItem
                    {
                        Value = e.idEstadoPago.ToString(),
                        Text = e.nombreEstado
                    })
                    .ToList();
            }
        }
        private int? ObtenerIdUsuarioActual()
        {
            string identityUserId = User.Identity.GetUserId();

            if (string.IsNullOrWhiteSpace(identityUserId))
                return null;

            using (var contexto = new Contexto())
            {
                return contexto.Usuarios
                    .Where(u => u.identityUserId == identityUserId)
                    .Select(u => (int?)u.idUsuario)
                    .FirstOrDefault();
            }
        }

        private string ObtenerIpUsuario()
        {
            return Request.UserHostAddress;
        }

        private string ObtenerNombreEstadoPago(int idEstadoPago)
        {
            using (var contexto = new Contexto())
            {
                string nombreEstado = contexto.EstadoPagos
                    .Where(e => e.idEstadoPago == idEstadoPago)
                    .Select(e => e.nombreEstado)
                    .FirstOrDefault();

                return string.IsNullOrWhiteSpace(nombreEstado)
                    ? "Desconocido"
                    : nombreEstado;
            }
        }

        private void RegistrarBitacoraPago(
            int idPago,
            PagoCrearDto modelo,
            string nombreEstadoPago)
        {
            string detalle =
                "Se registró el pago con id " + idPago +
                " para la membresía " + modelo.idMembresiaCliente +
                ". Monto: ₡" + modelo.monto.ToString("N2") +
                ". Estado: " + nombreEstadoPago + ".";

            if (modelo.idEstadoPago == 2)
            {
                detalle += " La membresía fue activada o renovada automáticamente.";
            }

            try
            {
                _registrarBitacoraLN.RegistrarBitacora(
                    new BitacoraDto
                    {
                        idUsuario = ObtenerIdUsuarioActual(),
                        tablaAfectada = "Pago",
                        accionRealizada = "INSERT",
                        idRegistroAfectado = idPago,
                        detalle = detalle,
                        ipUsuario = ObtenerIpUsuario()
                    }
                );
            }
            catch
            {
                /*
                 * El pago ya fue registrado correctamente.
                 * Un error secundario de auditoría no debe provocar que
                 * el usuario intente registrar nuevamente el mismo pago.
                 */
            }
        }
    }
}