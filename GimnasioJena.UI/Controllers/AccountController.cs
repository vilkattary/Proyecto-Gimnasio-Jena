using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using GimnasioJena.UI.Models;
using GimnasioJena.AccesoADatos;
using GimnasioJena.AccesoADatos.Entidades.Usuarios;
using GimnasioJena.Abstracciones.Modelos.Usuarios;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.RegistrarUsuario;


namespace GimnasioJena.UI.Controllers
{ 

    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IRegistrarUsuarioLN _registrarUsuarioServicio;

        public AccountController(ApplicationUserManager userManager,
                          ApplicationSignInManager signInManager,
                          IRegistrarUsuarioLN registrarUsuarioServicio)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _registrarUsuarioServicio = registrarUsuarioServicio;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            // Expulsa cualquier identidad residual para obligar a re-autenticarse
            LimpiarSesionYCookies();

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // No cuenta los errores de inicio de sesión para el bloqueo de la cuenta
            // Para permitir que los errores de contraseña desencadenen el bloqueo de la cuenta, cambie a shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    // Inyectar el nombre real del usuario como claim tras el login exitoso
                    var appUser = await UserManager.FindByEmailAsync(model.Email);
                    if (appUser != null)
                    {
                        string nombreCompleto = model.Email;
                        using (var ctx = new Contexto())
                        {
                            var usuarioPerfil = ctx.Usuarios.FirstOrDefault(u => u.identityUserId == appUser.Id);
                            if (usuarioPerfil != null)
                                nombreCompleto = usuarioPerfil.nombre;
                        }
                        var identity = await appUser.GenerateUserIdentityAsync(UserManager, nombreCompleto);
                        var authManager = HttpContext.GetOwinContext().Authentication;
                        authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        authManager.SignIn(new Microsoft.Owin.Security.AuthenticationProperties { IsPersistent = model.RememberMe }, identity);

                        // Redirigir según rol (User.IsInRole no aplica aún en este request)
                        if (!Url.IsLocalUrl(returnUrl))
                        {
                            bool esAdmin       = await UserManager.IsInRoleAsync(appUser.Id, "ADMINISTRADOR");
                            bool esCliente     = await UserManager.IsInRoleAsync(appUser.Id, "CLIENTE");
                            bool esEntrenador  = await UserManager.IsInRoleAsync(appUser.Id, "ENTRENADOR");
                            if (esAdmin)
                                return RedirectToAction("Dashboard", "Admin");
                            if (esCliente)
                                return RedirectToAction("MiPerfil", "Clientes");
                            if (esEntrenador)
                                return RedirectToAction("Index", "Entrenadores");
                        }
                    }
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Intento de inicio de sesión no válido.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Requerir que el usuario haya iniciado sesión con nombre de usuario y contraseña o inicio de sesión externo
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // El código siguiente protege de los ataques por fuerza bruta a los códigos de dos factores. 
            // Si un usuario introduce códigos incorrectos durante un intervalo especificado de tiempo, la cuenta del usuario 
            // se bloqueará durante un período de tiempo especificado. 
            // Puede configurar el bloqueo de la cuenta en IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Código no válido.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var contexto = new Contexto())
                {
                    bool identificacionExiste = contexto.Usuarios
                        .Any(u => u.identificacion == model.Identificacion);

                    if (identificacionExiste)
                    {
                        ModelState.AddModelError("Identificacion", "Ya existe un usuario registrado con esta identificación.");
                        return View(model);
                    }

                    bool correoPerfilExiste = contexto.Usuarios
                        .Any(u => u.correo == model.Email);

                    if (correoPerfilExiste)
                    {
                        ModelState.AddModelError("Email", "Ya existe un usuario registrado con este correo electrónico.");
                        return View(model);
                    }
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    bool perfilGuardado = false;
                    try
                    {
                        await UserManager.AddToRoleAsync(user.Id, "CLIENTE");

                        using (var contexto = new Contexto())
                        {
                            var usuario = new UsuarioEntidad
                            {
                                identityUserId = user.Id,
                                nombre = model.Nombre,
                                apellido1 = model.Apellido1,
                                apellido2 = model.Apellido2,
                                identificacion = model.Identificacion,
                                correo = model.Email,
                                telefono = model.Telefono,
                                fechaRegistro = DateTime.Now,
                                fechaModificacion = null,
                                estado = true
                            };

                            contexto.Usuarios.Add(usuario);
                            contexto.SaveChanges();
                            perfilGuardado = true;
                        }

                        try
                        {
                            await UserManager.SendEmailAsync(
                                user.Id,
                                "¡Bienvenido a JÉNA Training Methodology!",
                                ConstruirCorreoBienvenida(model.Nombre)
                            );
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Trace.TraceError(
                                "No se pudo enviar el correo de bienvenida al usuario {0}: {1}",
                                user.Id,
                                ex.ToString()
                            );
                        }

                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError(
                            "Error durante el registro del usuario {0}: {1}",
                            user.Id,
                            ex.ToString()
                        );

                        if (!perfilGuardado)
                        {
                            var resultadoEliminacion = await UserManager.DeleteAsync(user);

                            if (!resultadoEliminacion.Succeeded)
                            {
                                System.Diagnostics.Trace.TraceError(
                                    "No se pudo revertir la cuenta Identity del usuario {0}.",
                                    user.Id
                                );
                            }
                        }

                        ModelState.AddModelError(
                            "",
                            perfilGuardado
                                ? "Tu cuenta fue creada correctamente, pero ocurrió un problema al iniciar sesión. Intentá ingresar desde la página de inicio de sesión."
                                : "Ocurrió un error al guardar los datos del perfil. Intentá registrarte nuevamente."
                        );

                        return View(model);
                    }
                }

                AddErrors(result);
            }

            return View(model);
        }

        private static string ConstruirCorreoBienvenida(string nombre)
        {
            string nombreSeguro = System.Web.HttpUtility.HtmlEncode(nombre ?? "");

            return $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='margin:0;padding:0;background-color:#F3F5F8;
             font-family:Arial,Helvetica,sans-serif;'>

<table role='presentation' width='100%' cellpadding='0'
       cellspacing='0' style='background-color:#F3F5F8;'>
<tr>
<td align='center' style='padding:30px 15px;'>

<table role='presentation' width='100%' cellpadding='0'
       cellspacing='0'
       style='max-width:600px;background-color:#FFFFFF;
              border-collapse:collapse;'>

    <tr>
        <td align='center'
            style='background-color:#1C2636;padding:32px 20px;'>
            <div style='font-size:38px;font-weight:bold;
                        letter-spacing:4px;color:#FF6335;'>
                JÉNA
            </div>
            <div style='font-size:12px;letter-spacing:3px;
                        color:#FFFFFF;margin-top:6px;'>
                TRAINING METHODOLOGY
            </div>
        </td>
    </tr>

    <tr>
        <td style='padding:35px 30px;color:#1C2636;'>

            <h1 style='font-size:26px;text-align:center;
                       margin:0 0 12px;'>
                ¡Bienvenido a JÉNA!
            </h1>

            <p style='text-align:center;color:#FF6335;
                      font-weight:bold;font-size:13px;
                      letter-spacing:1px;margin-bottom:30px;'>
                TU ENTRENAMIENTO COMIENZA AQUÍ
            </p>

            <p style='font-size:16px;line-height:1.7;'>
                ¡Hola, <strong>{nombreSeguro}</strong>!
            </p>

            <p style='font-size:15px;line-height:1.8;
                      color:#394456;'>
                Nos alegra darte la bienvenida a nuestra comunidad.
                Tu cuenta ha sido creada correctamente y ya podés
                comenzar tu experiencia con nosotros.
            </p>

            <table role='presentation' width='100%'
                   cellpadding='0' cellspacing='0'
                   style='background-color:#F3F5F8;
                          margin:25px 0;'>
                <tr>
                    <td style='padding:22px;'>
                        <p style='font-size:16px;font-weight:bold;
                                  color:#1C2636;margin:0 0 10px;'>
                            &#10004; Registro completado
                        </p>

                        <p style='font-size:14px;line-height:1.7;
                                  color:#394456;margin:0;'>
                            Desde tu cuenta podés consultar tu perfil,
                            actualizar tus datos personales y acceder
                            a los servicios disponibles del gimnasio.
                        </p>
                    </td>
                </tr>
            </table>

            <p style='background-color:#FF6335;
                      color:#FFFFFF;text-align:center;
                      padding:18px;font-weight:bold;
                      font-size:16px;'>
                ¡Nos vemos en tu próximo entrenamiento!
            </p>

            <p style='font-size:14px;line-height:1.7;
                      text-align:center;color:#394456;
                      margin-top:30px;'>
                Gracias por confiar en nosotros.
            </p>

        </td>
    </tr>

    <tr>
        <td align='center'
            style='background-color:#1C2636;padding:25px 20px;'>

            <p style='color:#FFFFFF;font-size:13px;
                      font-weight:bold;margin:0 0 8px;'>
                JÉNA TRAINING METHODOLOGY
            </p>

            <p style='color:#B8C0CC;font-size:11px;
                      margin:0;'>
                Este es un mensaje automático de bienvenida.
            </p>

        </td>
    </tr>

</table>
</td>
</tr>
</table>

</body>
</html>";
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                    return View("ForgotPasswordConfirmation");
                }

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                var callbackUrl = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { userId = user.Id, code = code },
                    protocol: Request.Url.Scheme
                );

                await UserManager.SendEmailAsync(
                    user.Id,
                    "Restablecer contraseña - Gimnasio Jena",
                    "Para restablecer tu contraseña, haz clic en el siguiente enlace: <br/><br/>" +
                    "<a href=\"" + callbackUrl + "\">Restablecer contraseña</a>"
                );

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // No revelar que el usuario no existe
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Solicitar redireccionamiento al proveedor de inicio de sesión externo
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generar el token y enviarlo
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Si el usuario ya tiene un inicio de sesión, iniciar sesión del usuario con este proveedor de inicio de sesión externo
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // Si el usuario no tiene ninguna cuenta, solicitar que cree una
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Obtener datos del usuario del proveedor de inicio de sesión externo
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            LimpiarSesionYCookies();
            return RedirectToAction("Login", "Account");
        }

        private void LimpiarSesionYCookies()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut(
                DefaultAuthenticationTypes.ApplicationCookie,
                DefaultAuthenticationTypes.ExternalCookie,
                DefaultAuthenticationTypes.TwoFactorCookie);

            if (Session != null)
            {
                Session.Clear();
                Session.Abandon();
            }

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                var sessionCookie = new HttpCookie("ASP.NET_SessionId", string.Empty)
                {
                    Expires = DateTime.UtcNow.AddYears(-1),
                    HttpOnly = true
                };
                Response.Cookies.Add(sessionCookie);
            }

            if (Request.Cookies[".AspNet.ApplicationCookie"] != null)
            {
                var authCookie = new HttpCookie(".AspNet.ApplicationCookie", string.Empty)
                {
                    Expires = DateTime.UtcNow.AddYears(-1),
                    HttpOnly = true
                };
                Response.Cookies.Add(authCookie);
            }

            // La cookie "__RequestVerificationToken" no se elimina: la vista de Login
            // emite un token nuevo en la misma respuesta y expirarla provocaría
            // HttpAntiForgeryException al enviar el formulario.

            // SignOut solo surte efecto en la siguiente petición; se anonimiza el
            // principal actual para que el layout no renderice el estado autenticado.
            var anonimo = new ClaimsPrincipal(new ClaimsIdentity());
            HttpContext.User = anonimo;
            System.Threading.Thread.CurrentPrincipal = anonimo;
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Aplicaciones auxiliares
        // Se usa para la protección XSRF al agregar inicios de sesión externos
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            if (User.IsInRole("ADMINISTRADOR"))
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}