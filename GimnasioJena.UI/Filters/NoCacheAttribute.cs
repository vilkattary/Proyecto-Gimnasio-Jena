using System;
using System.Web;
using System.Web.Mvc;

namespace GimnasioJena.UI.Filters
{
    /// <summary>
    /// Evita que el navegador almacene en caché (incluido el BFCache) las vistas
    /// autenticadas, de modo que el botón "Atrás" no vuelva a mostrarlas tras
    /// cerrar sesión.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
            cache.SetCacheability(HttpCacheability.NoCache);
            cache.SetNoStore();
            cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            cache.SetExpires(DateTime.UtcNow.AddYears(-1));
            cache.AppendCacheExtension("no-cache, no-store, must-revalidate, max-age=0");

            filterContext.HttpContext.Response.AppendHeader("Pragma", "no-cache");
            filterContext.HttpContext.Response.AppendHeader("Expires", "0");

            base.OnResultExecuting(filterContext);
        }
    }
}
