using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GimnasioJena.UI.Filters
{
    public class SoloAdministradorAttribute : AuthorizeAttribute
    {
        public SoloAdministradorAttribute()
        {
            Roles = "ADMINISTRADOR";
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new
                {
                    controller = "Home",
                    action = "Index",
                    area = ""
                })
            );
        }
    }
}
