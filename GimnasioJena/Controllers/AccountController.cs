using System.Web.Mvc;

namespace GimnasioJena.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Register()
        {
            return View(); 
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {   
            return View();
        }
    }
}