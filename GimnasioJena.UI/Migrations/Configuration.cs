namespace GimnasioJena.UI.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GimnasioJena.UI.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GimnasioJena.UI.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            string[] roles = { "ADMINISTRADOR", "CLIENTE", "ENTRENADOR", "RECEPCIONISTA" };

            foreach (var rol in roles)
            {
                if (!roleManager.RoleExists(rol))
                {
                    roleManager.Create(new IdentityRole(rol));
                }
            }
        }
    }
}
