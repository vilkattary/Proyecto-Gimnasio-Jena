using GimnasioJena.Abstracciones.AccesoADatos;
using GimnasioJena.Abstracciones.AccesoADatos.Usuarios.ObtenerUsuarioPorId;
using GimnasioJena.Abstracciones.AccesoADatos.Usuarios.RegistrarUsuario;
using GimnasioJena.Abstracciones.LogicaDeNegocio;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.RegistrarUsuario;
using GimnasioJena.AccesoADatos;
using GimnasioJena.AccesoADatos.Usuarios.ObtenerUsuarioPorId;
using GimnasioJena.AccesoADatos.Usuarios.RegistrarUsuario;
using GimnasioJena.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId;
using GimnasioJena.LogicaDeNegocio.Usuarios.RegistrarUsuario;
using Microsoft.AspNet.Identity.Owin;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using System;
using System.Web;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(GimnasioJena.UI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(GimnasioJena.UI.App_Start.NinjectWebCommon), "Stop")]

namespace GimnasioJena.UI.App_Start
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start()
        {
            bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            RegisterServices(kernel);
            return kernel;
        }

        private static void RegisterServices(IKernel kernel)
        {
            // Contexto de base de datos
            kernel.Bind<Contexto>()
                  .ToSelf()
                  .InRequestScope();

            // Acceso a Datos
            kernel.Bind<IRegistrarUsuarioAD>()
                  .To<RegistrarUsuarioAD>()
                  .InRequestScope();

            // Lógica del Negocio
            kernel.Bind<IRegistrarUsuarioLN>()
                  .To<RegistrarUsuarioLN>()
                  .InRequestScope();

            // Identity
            kernel.Bind<ApplicationUserManager>()
                  .ToMethod(c => HttpContext.Current.GetOwinContext()
                  .GetUserManager<ApplicationUserManager>())
                  .InRequestScope();

            kernel.Bind<ApplicationSignInManager>()
                  .ToMethod(c => HttpContext.Current.GetOwinContext()
                  .Get<ApplicationSignInManager>())
                  .InRequestScope();

            kernel.Bind<IObtenerUsuarioPorIdAD>()
                  .To<ObtenerUsuarioPorIdAD>()
                  .InRequestScope();

            kernel.Bind<IObtenerUsuarioPorIdLN>()
                  .To<ObtenerUsuarioPorIdLN>()
                  .InRequestScope();
        }
    }
}