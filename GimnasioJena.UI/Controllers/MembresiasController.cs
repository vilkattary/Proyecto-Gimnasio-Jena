using GimnasioJena.Abstracciones.LogicaDeNegocio.Bitacora;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.EditarMembresia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerMembresiaPorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerTodasLasMembresias;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.RegistrarMembresia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.RenovarMembresia;
using GimnasioJena.Abstracciones.Modelos.Bitacora;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.AccesoADatos;
using GimnasioJena.LogicaDeNegocio.Bitacora;
using GimnasioJena.LogicaDeNegocio.Membresias.EditarMembresia;
using GimnasioJena.LogicaDeNegocio.Membresias.ObtenerMembresiaPorId;
using GimnasioJena.LogicaDeNegocio.Membresias.ObtenerTodasLasMembresias;
using GimnasioJena.LogicaDeNegocio.Membresias.RegistrarMembresia;
using GimnasioJena.LogicaDeNegocio.Membresias.RenovarMembresia;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class MembresiasController : Controller
    {
        private readonly IObtenerTodasLasMembresiasLN _obtenerTodasLasMembresiasLN;
        private readonly IRegistrarMembresiaLN _registrarMembresiaLN;
        private readonly IObtenerMembresiaPorIdLN _obtenerMembresiaPorIdLN;
        private readonly IEditarMembresiaLN _editarMembresiaLN;
        private readonly IRenovarMembresiaLN _renovarMembresiaLN;
        private readonly IRegistrarBitacoraLN _registrarBitacoraLN;



        public MembresiasController()
        {
            _obtenerTodasLasMembresiasLN = new ObtenerTodasLasMembresiasLN();
            _registrarMembresiaLN = new RegistrarMembresiaLN();
            _obtenerMembresiaPorIdLN = new ObtenerMembresiaPorIdLN();
            _editarMembresiaLN = new EditarMembresiaLN();
            _renovarMembresiaLN = new RenovarMembresiaLN();
            _registrarBitacoraLN = new RegistrarBitacoraLN();
        }

        public ActionResult Index()
        {
            var membresias = _obtenerTodasLasMembresiasLN.ObtenerTodasLasMembresias();
            return View(membresias);
        }

        private void CargarCombos()
        {
            using (var contexto = new Contexto())
            {
                ViewBag.Usuarios = contexto.Usuarios
                    .Where(u => u.estado)
                    .OrderBy(u => u.nombre)
                    .Select(u => new SelectListItem
                    {
                        Value = u.idUsuario.ToString(),
                        Text = u.nombre + " " + u.apellido1 + " " + u.apellido2
                    })
                    .ToList();

                ViewBag.Planes = contexto.PlanesMembresia
                    .Where(p => p.estado)
                    .OrderBy(p => p.nombrePlan)
                    .Select(p => new SelectListItem
                    {
                        Value = p.idPlanMembresia.ToString(),
                        Text = p.nombrePlan
                    })
                    .ToList();

                ViewBag.Estados = contexto.EstadoMembresias
                    .Where(e => e.estado)
                    .OrderBy(e => e.idEstadoMembresia)
                    .Select(e => new SelectListItem
                    {
                        Value = e.idEstadoMembresia.ToString(),
                        Text = e.nombreEstado
                    })
                    .ToList();
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            CargarCombos();

            var modelo = new MembresiaCrearDto
            {
                fechaInicio = DateTime.Today,
                fechaFin = DateTime.Today.AddMonths(1),
                idEstadoMembresia = 1
            };

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MembresiaCrearDto modelo)
        {
            if (!ModelState.IsValid)
            {
                CargarCombos();
                return View(modelo);
            }

            bool resultado = _registrarMembresiaLN.RegistrarMembresia(modelo);

            if (resultado)
            {
                RegistrarBitacora(
                    "MembresiaCliente",
                    "CREATE",
                    modelo.idUsuario,
                    "Se registró una membresía para el cliente con idUsuario: " + modelo.idUsuario
                );

                TempData["MensajeExito"] = "Membresía registrada correctamente.";
                return RedirectToAction("Index");
            }

            TempData["MensajeError"] = "No se pudo registrar la membresía. Verifica que el cliente no tenga una membresía activa.";
            CargarCombos();
            return View(modelo);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var membresia = _obtenerMembresiaPorIdLN.ObtenerMembresiaPorId(id);

            if (membresia == null)
            {
                TempData["MensajeError"] = "No se encontró la membresía solicitada.";
                return RedirectToAction("Index");
            }

            CargarCombos();
            return View(membresia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MembresiaEditarDto modelo)
        {
            if (!ModelState.IsValid)
            {
                CargarCombos();
                return View(modelo);
            }

            bool resultado = _editarMembresiaLN.EditarMembresia(modelo);

            if (resultado)
            {
                RegistrarBitacora(
                    "MembresiaCliente",
                    "UPDATE",
                    modelo.idMembresiaCliente,
                    "Se actualizó la membresía con id: " + modelo.idMembresiaCliente
                );

                TempData["MensajeExito"] = "Membresía actualizada correctamente.";
                return RedirectToAction("Index");
            }

            TempData["MensajeError"] = "No se pudo actualizar la membresía. Verifica los datos ingresados.";
            CargarCombos();
            return View(modelo);
        }
        public ActionResult Details(int id)
        {
            var membresia = _obtenerMembresiaPorIdLN.ObtenerDetalleMembresiaPorId(id);

            if (membresia == null)
            {
                TempData["MensajeError"] = "No se encontró la membresía.";
                return RedirectToAction("Index");
            }

            return View(membresia);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Renovar(int id)
        {
            var modelo = new MembresiaRenovarDto
            {
                idMembresiaCliente = id
            };

            bool resultado = _renovarMembresiaLN.RenovarMembresia(modelo);

            if (resultado)
            {
                RegistrarBitacora(
                    "MembresiaCliente",
                    "UPDATE",
                    id,
                    "Se renovó la membresía con id: " + id
                );

                TempData["MensajeExito"] = "Membresía renovada correctamente.";
            }
            else
            {
                TempData["MensajeError"] = "No se pudo renovar la membresía.";
            }

            return RedirectToAction("Index");
        }
        private int? ObtenerIdUsuarioActual()
        {
            var identityUserId = User.Identity.GetUserId();

            using (var contexto = new Contexto())
            {
                var usuario = contexto.Usuarios
                    .FirstOrDefault(u => u.identityUserId == identityUserId);

                return usuario?.idUsuario;
            }
        }

        private string ObtenerIpUsuario()
        {
            return Request.UserHostAddress;
        }

        private void RegistrarBitacora(string tabla, string accion, int? idRegistro, string detalle)
        {
            _registrarBitacoraLN.RegistrarBitacora(new BitacoraDto
            {
                idUsuario = ObtenerIdUsuarioActual(),
                tablaAfectada = tabla,
                accionRealizada = accion,
                idRegistroAfectado = idRegistro,
                detalle = detalle,
                ipUsuario = ObtenerIpUsuario()
            });
        }
    }
}