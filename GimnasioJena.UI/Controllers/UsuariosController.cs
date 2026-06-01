using GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.CambiarRolUsuario;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerTodosLosUsuarios;
using GimnasioJena.Abstracciones.Modelos.Usuarios;
using GimnasioJena.LogicaDeNegocio.Usuarios.CambiarRolUsuario;
using GimnasioJena.LogicaDeNegocio.Usuarios.ObtenerTodosLosUsuarios;
using System.Linq;
﻿using GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.RegistrarUsuario;
using GimnasioJena.Abstracciones.Modelos.Usuarios;
using GimnasioJena.UI.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class UsuariosController : Controller
    {
        private readonly IObtenerTodosLosUsuariosLN _obtenerTodosLosUsuariosLN;
        private readonly ICambiarRolUsuarioLN _cambiarRolUsuarioLN;

        public UsuariosController()
        {
            _obtenerTodosLosUsuariosLN = new ObtenerTodosLosUsuariosLN();
            _cambiarRolUsuarioLN = new CambiarRolUsuarioLN();
        }

        public ActionResult Index()
        {
            var usuarios = _obtenerTodosLosUsuariosLN.ObtenerTodosLosUsuarios();
            return View(usuarios);
        }

        [HttpGet]
        public ActionResult CambiarRol(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return RedirectToAction("Index");
            }

            var usuarios = _obtenerTodosLosUsuariosLN.ObtenerTodosLosUsuarios();
            var usuario = usuarios.FirstOrDefault(u => u.identityUserId == identityUserId);

            if (usuario == null)
            {
                TempData["MensajeError"] = "No se encontró el usuario solicitado.";
                return RedirectToAction("Index");
            }

            var modelo = new UsuarioCambiarRolDto
            {
                idUsuario = usuario.idUsuario,
                identityUserId = usuario.identityUserId,
                rolActual = usuario.rol,
                rolNuevo = usuario.rol
            };

            CargarRoles(modelo.rolNuevo);

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarRol(UsuarioCambiarRolDto modelo)
        {
            if (modelo == null)
            {
                TempData["MensajeError"] = "No se recibió información válida.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(modelo.identityUserId))
            {
                ModelState.AddModelError("", "El identificador del usuario es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(modelo.rolNuevo))
            {
                ModelState.AddModelError("rolNuevo", "Debe seleccionar un rol.");
            }

            if (!ModelState.IsValid)
            {
                CargarRoles(modelo.rolNuevo);
                return View(modelo);
            }

            bool resultado = _cambiarRolUsuarioLN.CambiarRolUsuario(modelo);

            if (resultado)
            {
                TempData["MensajeExito"] = "El rol del usuario se actualizó correctamente.";
                return RedirectToAction("Index");
            }

            TempData["MensajeError"] = "No se pudo actualizar el rol del usuario.";
            CargarRoles(modelo.rolNuevo);
            return View(modelo);
        }

        private void CargarRoles(string rolSeleccionado = null)
        {
            var roles = new[]
            {
                "ADMINISTRADOR",
                "CLIENTE",
                "ENTRENADOR",
                "RECEPCIONISTA"
            };

            ViewBag.Roles = new SelectList(roles, rolSeleccionado);
        }
    }
}