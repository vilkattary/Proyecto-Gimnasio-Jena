using GimnasioJena.Abstracciones.AccesoADatos.Home.ObtenerSeccionesHome;
using GimnasioJena.Abstracciones.Modelos.Home;
using GimnasioJena.AccesoADatos.Entidades.Home;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Home.ObtenerSeccionesHome
{
    public class ObtenerContenidoWebAD : IObtenerContenidoWebAD
    {
        public async Task<IEnumerable<ContenidoWebDto>> ObtenerPorPaginaAsync(string pagina)
        {
            using (var contexto = new Contexto())
            {
                bool tablaVacia = !await contexto.ContenidoWeb.AnyAsync(x => x.Pagina == pagina);

                if (tablaVacia)
                    await SembrarContenidoInicial(contexto, pagina);

                var entidades = await contexto.ContenidoWeb
                    .Where(x => x.Pagina == pagina)
                    .OrderBy(s => s.Orden)
                    .ToListAsync();

                return entidades.Select(s => new ContenidoWebDto
                {
                    Id                = s.Id,
                    Pagina            = s.Pagina,
                    Seccion           = s.Seccion,
                    Clave             = s.Clave,
                    TextoPrincipal    = s.TextoPrincipal,
                    TextoSecundario   = s.TextoSecundario,
                    UrlImagen         = s.UrlImagen,
                    Orden             = s.Orden,
                    FechaModificacion = s.FechaModificacion
                });
            }
        }

        private async Task SembrarContenidoInicial(Contexto contexto, string pagina)
        {
            if (pagina != "Home")
                return;

            var secciones = new List<ContenidoWeb>
            {
                new ContenidoWeb
                {
                    Pagina           = "Home",
                    Seccion          = "Hero",
                    Clave            = "hero-principal",
                    TextoPrincipal   = "Entrena En Jéna Training Methodology",
                    TextoSecundario  = "Desbloquea tu máximo potencial con entrenamientos de alta intensidad y una comunidad que te impulsa a superar tus límites cada día.",
                    UrlImagen        = "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?w=1920",
                    Orden            = 1,
                    FechaModificacion = DateTime.Now
                },
                new ContenidoWeb
                {
                    Pagina           = "Home",
                    Seccion          = "Clases",
                    Clave            = "clase-fuerza",
                    TextoPrincipal   = "Fuerza Intensa",
                    TextoSecundario  = "Desarrolla músculo y potencia con nuestro programa de entrenamiento de fuerza especializado. Barras olímpicas, mancuernas y máquinas de última generación te esperan.",
                    UrlImagen        = "",
                    Orden            = 2,
                    FechaModificacion = DateTime.Now
                },
                new ContenidoWeb
                {
                    Pagina           = "Home",
                    Seccion          = "Clases",
                    Clave            = "clase-yoga",
                    TextoPrincipal   = "Yoga Flow",
                    TextoSecundario  = "Encuentra tu equilibrio interior y mejora tu flexibilidad con sesiones de yoga guiadas por instructores certificados en un ambiente relajante y motivador.",
                    UrlImagen        = "",
                    Orden            = 3,
                    FechaModificacion = DateTime.Now
                },
                new ContenidoWeb
                {
                    Pagina           = "Home",
                    Seccion          = "Clases",
                    Clave            = "clase-cardio",
                    TextoPrincipal   = "Cardio Boost",
                    TextoSecundario  = "Quema calorías y mejora tu resistencia cardiovascular con sesiones intensas de cardio. Elípticas, cintas y bicicletas de spinning a tu disposición.",
                    UrlImagen        = "",
                    Orden            = 4,
                    FechaModificacion = DateTime.Now
                },
                new ContenidoWeb
                {
                    Pagina           = "Home",
                    Seccion          = "Testimonios",
                    Clave            = "testimonio-maria",
                    TextoPrincipal   = "María Rodríguez",
                    TextoSecundario  = "Gimnasio Jena cambió mi vida completamente. Los entrenadores son increíbles y el ambiente te motiva a dar el 100% en cada sesión. ¡Recomendado al 200%!",
                    UrlImagen        = "",
                    Orden            = 5,
                    FechaModificacion = DateTime.Now
                },
                new ContenidoWeb
                {
                    Pagina           = "Home",
                    Seccion          = "Testimonios",
                    Clave            = "testimonio-carlos",
                    TextoPrincipal   = "Carlos Gómez",
                    TextoSecundario  = "La mejor inversión que he hecho en mi salud. Las instalaciones son de primer nivel y las clases grupales son súper dinámicas. ¡Me encanta!",
                    UrlImagen        = "",
                    Orden            = 6,
                    FechaModificacion = DateTime.Now
                },
                new ContenidoWeb
                {
                    Pagina           = "Home",
                    Seccion          = "Testimonios",
                    Clave            = "testimonio-laura",
                    TextoPrincipal   = "Laura Pérez",
                    TextoSecundario  = "Nunca pensé que entrenar podría ser tan divertido. El equipo de JENA es como una familia que te apoya en cada paso. ¡Resultados garantizados!",
                    UrlImagen        = "",
                    Orden            = 7,
                    FechaModificacion = DateTime.Now
                }
            };

            contexto.ContenidoWeb.AddRange(secciones);
            await contexto.SaveChangesAsync();
        }
    }
}
