using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GimnasioJena.UI.Startup))]
namespace GimnasioJena.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
