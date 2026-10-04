using System.Web;
using System.Web.Mvc;
using GimnasioJena.UI.Filters;

namespace GimnasioJena.UI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new NoCacheAttribute());
        }
    }
}
