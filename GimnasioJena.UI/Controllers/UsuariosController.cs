using GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.RegistrarUsuario;
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
    public class UsuariosController : Controller
    {
        private readonly IRegistrarUsuarioLN _servicio;
        public UsuariosController(IRegistrarUsuarioLN servicio)
        {
            _servicio = servicio;
        }
        /* -------------------------------------------------------
         * ----------Registro de Usuarios
           ------------------------------------------------------- */
        // GET: /Registro
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        // POST: /Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var userManager = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>();
            // 1. Crear usuario en Identity (AspNetUsers)
            var identityUser = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var resultado = await userManager.CreateAsync(identityUser, model.Password);
            if (!resultado.Succeeded)
            {
                foreach (var error in resultado.Errors)
                    ModelState.AddModelError("", error);
                return View(model);
            }
            // 2. Guardar datos extra en tu tabla Usuario
            var dto = new UsuarioCrearDto
            {
                identityUserId = identityUser.Id,
                nombre = model.Nombre,
                apellido1 = model.Apellido1,
                apellido2 = model.Apellido2,
                identificacion = model.Identificacion,
                correo = model.Email,
                telefono = model.Telefono
            };
            await _servicio.RegistrarUsuario(dto);
            return RedirectToAction("Index", "Home");
        }
    }
}