using GimnasioJena.Abstracciones.LogicaDeNegocio.Bitacora;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Pagos.ObtenerTodosLosPagos;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Pagos.RegistrarPago;
using GimnasioJena.Abstracciones.Modelos.Bitacora;
using GimnasioJena.Abstracciones.Modelos.Pagos;
using GimnasioJena.AccesoADatos;
using GimnasioJena.LogicaDeNegocio.Bitacora;
using GimnasioJena.LogicaDeNegocio.Pagos.ObtenerTodosLosPagos;
using GimnasioJena.LogicaDeNegocio.Pagos.RegistrarPago;
using GimnasioJena.UI.Models;
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
        private const int ESTADO_PAGO_PAGADO = 2;
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
            if (modelo == null)
                return new HttpStatusCodeResult(400);

            /*
             * Los pagos administrativos se registran
             * únicamente cuando el dinero fue recibido.
             */
            modelo.idEstadoPago = ESTADO_PAGO_PAGADO;

            ModelState.Remove("idEstadoPago");

            /*
             * Determinar qué tipo de pago se está registrando.
             */
            bool esClienteNuevo = modelo.tipoPago == "nuevo";
            bool esMembresiaExistente = modelo.tipoPago == "existente";

            if (!esClienteNuevo && !esMembresiaExistente)
            {
                ModelState.AddModelError(
                    "",
                    "Debe seleccionar un tipo de pago válido.");
            }

            /*
             * Validar los campos correspondientes
             * al tipo de pago seleccionado.
             */
            if (esClienteNuevo)
            {
                if (modelo.idUsuarioNuevo <= 0)
                {
                    ModelState.AddModelError(
                        "idUsuarioNuevo",
                        "Debe seleccionar un cliente.");
                }

                if (modelo.idPlanNuevo <= 0)
                {
                    ModelState.AddModelError(
                        "idPlanNuevo",
                        "Debe seleccionar un plan de membresía.");
                }
            }
            else if (esMembresiaExistente)
            {
                if (modelo.idMembresiaCliente <= 0)
                {
                    ModelState.AddModelError(
                        "idMembresiaCliente",
                        "Debe seleccionar una membresía.");
                }
            }

            if (!ModelState.IsValid)
            {
                CargarCombos();
                return View(modelo);
            }

            /*
             * Ejecutar el proceso correspondiente.
             */
            int idPagoRegistrado;

            if (esClienteNuevo)
            {
                idPagoRegistrado =
                    _registrarPagoLN.RegistrarPrimerPagoManual(modelo);
            }
            else
            {
                idPagoRegistrado =
                    _registrarPagoLN.RegistrarPago(modelo);
            }

            if (idPagoRegistrado > 0)
            {
                string nombreEstadoPago =
                    ObtenerNombreEstadoPago(modelo.idEstadoPago);

                RegistrarBitacoraPago(
                    idPagoRegistrado,
                    modelo,
                    nombreEstadoPago
                );

                TempData["MensajeExito"] = esClienteNuevo
                    ? "Pago registrado correctamente. La nueva membresía fue creada y activada."
                    : "Pago registrado correctamente y membresía actualizada.";

                return RedirectToAction("Index");
            }

            TempData["MensajeError"] = esClienteNuevo
                ? "No se pudo registrar el primer pago. Verifique que el cliente no tenga una membresía y que los datos sean válidos."
                : "No se pudo registrar el pago. La membresía puede seguir vigente o la información ingresada no es válida.";

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

        [HttpGet]
        public JsonResult ObtenerPrecioPlan(int idPlanMembresia)
        {
            using (var contexto = new Contexto())
            {
                var precio = contexto.PlanesMembresia
                    .Where(p => p.idPlanMembresia == idPlanMembresia
                             && p.estado)
                    .Select(p => (decimal?)p.precio)
                    .FirstOrDefault();

                if (!precio.HasValue)
                {
                    return Json(new
                    {
                        exito = false,
                        mensaje = "El plan seleccionado no está disponible."
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    exito = true,
                    precio = precio.Value.ToString(
                        "0.00",
                        System.Globalization.CultureInfo.InvariantCulture)
                }, JsonRequestBehavior.AllowGet);
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
                // Clientes activos que todavía no tienen membresía.
                using (var contextoIdentity = new ApplicationDbContext())
                {
                    // Obtener los IDs de usuarios con rol CLIENTE.
                    var idsClientes = (
                        from usuarioIdentity in contextoIdentity.Users
                        from rolUsuario in usuarioIdentity.Roles
                        join rol in contextoIdentity.Roles
                            on rolUsuario.RoleId equals rol.Id
                        where rol.Name == "CLIENTE"
                        select usuarioIdentity.Id
                    ).Distinct().ToList();

                    // Mostrar únicamente clientes sin membresía.
                    ViewBag.ClientesSinMembresia = contexto.Usuarios
                        .Where(u =>
                            u.estado &&
                            u.identityUserId != null &&
                            idsClientes.Contains(u.identityUserId) &&
                            !contexto.Membresias.Any(m =>
                                m.idUsuario == u.idUsuario))
                        .OrderBy(u => u.nombre)
                        .ThenBy(u => u.apellido1)
                        .Select(u => new
                        {
                            u.idUsuario,
                            nombreCompleto =
                                u.nombre + " " +
                                u.apellido1 + " " +
                                u.apellido2
                        })
                        .ToList()
                        .Select(u => new SelectListItem
                        {
                            Value = u.idUsuario.ToString(),
                            Text = u.nombreCompleto
                        })
                        .ToList();
                }

                // Planes de membresía activos.
                ViewBag.PlanesDisponibles = contexto.PlanesMembresia
                    .Where(p => p.estado)
                    .OrderBy(p => p.nombrePlan)
                    .ToList()
                    .Select(p => new SelectListItem
                    {
                        Value = p.idPlanMembresia.ToString(),
                        Text = p.nombrePlan
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
            string detalle;

            if (modelo.tipoPago == "nuevo")
            {
                // Primer pago manual de un cliente sin membresía.
                detalle =
                    "Se registró el primer pago manual con id " + idPago +
                    " para el cliente " + modelo.idUsuarioNuevo +
                    ", plan " + modelo.idPlanNuevo +
                    ". Monto: ₡" + modelo.monto.ToString("N2") +
                    ". Estado: " + nombreEstadoPago + ".";
            }
            else
            {
                // Pago de una membresía existente.
                detalle =
                    "Se registró el pago con id " + idPago +
                    " para la membresía " + modelo.idMembresiaCliente +
                    ". Monto: ₡" + modelo.monto.ToString("N2") +
                    ". Estado: " + nombreEstadoPago + ".";
            }

            if (modelo.idEstadoPago == 2)
            {
                if (modelo.tipoPago == "nuevo")
                {
                    detalle +=
                        " Se creó y activó automáticamente una nueva membresía.";
                }
                else
                {
                    detalle +=
                        " La membresía fue activada o renovada automáticamente.";
                }
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