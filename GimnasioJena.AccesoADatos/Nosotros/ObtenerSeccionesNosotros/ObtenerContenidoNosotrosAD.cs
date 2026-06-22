using GimnasioJena.Abstracciones.AccesoADatos.Home.ObtenerSeccionesHome;
using GimnasioJena.Abstracciones.Modelos.Home;
using GimnasioJena.AccesoADatos.Entidades.Home;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Nosotros.ObtenerSeccionesNosotros
{
    public class ObtenerContenidoNosotrosAD : IObtenerContenidoWebAD
    {
        public async Task<IEnumerable<ContenidoWebDto>> ObtenerPorPaginaAsync(string pagina)
        {
            using (var contexto = new Contexto())
            {
                bool tablaVacia = !await contexto.ContenidoWeb.AnyAsync(x => x.Pagina == pagina);

                if (tablaVacia)
                    await SembrarContenidoNosotros(contexto);
                else
                    await ParchearRegistrosFaltantes(contexto);

                var entidades = await contexto.ContenidoWeb
                    .Where(x => x.Pagina == pagina && x.Estado)
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
                    Estado            = s.Estado,
                    FechaModificacion = s.FechaModificacion
                });
            }
        }

        public async Task<IEnumerable<ContenidoWebDto>> ObtenerTodosPorPaginaAsync(string pagina)
        {
            using (var contexto = new Contexto())
            {
                bool tablaVacia = !await contexto.ContenidoWeb.AnyAsync(x => x.Pagina == pagina);

                if (tablaVacia)
                    await SembrarContenidoNosotros(contexto);
                else
                    await ParchearRegistrosFaltantes(contexto);

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
                    Estado            = s.Estado,
                    FechaModificacion = s.FechaModificacion
                });
            }
        }

        private async Task ParchearRegistrosFaltantes(Contexto contexto)
        {
            var registrosEsperados = new List<ContenidoWeb>
            {
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Stats",
                    Clave             = "stat-miembros",
                    TextoPrincipal    = "500+",
                    TextoSecundario   = "Miembros Activos",
                    UrlImagen         = "bi-people-fill",
                    Orden             = 4,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Stats",
                    Clave             = "stat-experiencia",
                    TextoPrincipal    = "10+",
                    TextoSecundario   = "A\u00f1os de Experiencia",
                    UrlImagen         = "bi-calendar-check-fill",
                    Orden             = 5,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Stats",
                    Clave             = "stat-clases",
                    TextoPrincipal    = "20+",
                    TextoSecundario   = "Clases Semanales",
                    UrlImagen         = "bi-lightning-charge-fill",
                    Orden             = 6,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Stats",
                    Clave             = "stat-instructores",
                    TextoPrincipal    = "15+",
                    TextoSecundario   = "Instructores Certificados",
                    UrlImagen         = "bi-award-fill",
                    Orden             = 7,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Valores",
                    Clave             = "valor-excelencia",
                    TextoPrincipal    = "Excelencia",
                    TextoSecundario   = "Buscamos la mejora continua en cada aspecto: instalaciones, metodolog\u00eda de entrenamiento y atenci\u00f3n al cliente. Nunca conformarse con el m\u00ednimo.",
                    UrlImagen         = "bi-trophy-fill",
                    Orden             = 11,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Valores",
                    Clave             = "valor-compromiso",
                    TextoPrincipal    = "Compromiso",
                    TextoSecundario   = "Estar presentes en cada paso del camino de nuestros miembros, desde el primer d\u00eda hasta que sus metas se convierten en realidad.",
                    UrlImagen         = "bi-heart-fill",
                    Orden             = 12,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Valores",
                    Clave             = "valor-comunidad",
                    TextoPrincipal    = "Comunidad",
                    TextoSecundario   = "Construir relaciones aut\u00e9nticas entre miembros y entrenadores que van m\u00e1s all\u00e1 del gimnasio, creando una red de apoyo mutuo.",
                    UrlImagen         = "bi-people-fill",
                    Orden             = 13,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Valores",
                    Clave             = "valor-integridad",
                    TextoPrincipal    = "Integridad",
                    TextoSecundario   = "Actuar con honestidad y transparencia en todo lo que hacemos, cumpliendo nuestras promesas y siendo responsables con nuestros miembros.",
                    UrlImagen         = "bi-shield-check",
                    Orden             = 14,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                }
            };

            bool huboCambios = false;

            foreach (var esperado in registrosEsperados)
            {
                bool existe = await contexto.ContenidoWeb
                    .AnyAsync(x => x.Pagina == "Nosotros" && x.Clave == esperado.Clave);

                if (!existe)
                {
                    contexto.ContenidoWeb.Add(esperado);
                    huboCambios = true;
                }
            }

            if (huboCambios)
                await contexto.SaveChangesAsync();
        }

        private async Task SembrarContenidoNosotros(Contexto contexto)
        {
            var secciones = new List<ContenidoWeb>
            {
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Header",
                    Clave             = "nosotros-header",
                    TextoPrincipal    = "SOBRE NOSOTROS",
                    TextoSecundario   = "Qui\u00e9nes somos",
                    UrlImagen         = "",
                    Orden             = 1,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Historia",
                    Clave             = "nosotros-historia",
                    TextoPrincipal    = "M\u00c1S DE 10 A\u00d1OS FORJANDO CAMPEONES",
                    TextoSecundario   = "Nacimos en 2014 con una visi\u00f3n clara: crear un espacio donde cada persona, sin importar su condici\u00f3n f\u00edsica, pudiera transformarse. Lo que comenz\u00f3 como un peque\u00f1o estudio de entrenamiento funcional se convirti\u00f3 en el centro de alto rendimiento que ves hoy.",
                    UrlImagen         = "",
                    Orden             = 2,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Historia",
                    Clave             = "nosotros-historia-p2",
                    TextoPrincipal    = "",
                    TextoSecundario   = "Hoy somos m\u00e1s que un gimnasio. Somos una comunidad que celebra cada logro, que empuja sus l\u00edmites y que sabe que el verdadero cambio ocurre cuando el compromiso se convierte en h\u00e1bito. Cada m\u00e1quina, cada clase, cada entrenador ha sido elegido con un prop\u00f3sito: el tuyo.",
                    UrlImagen         = "",
                    Orden             = 3,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Stats",
                    Clave             = "stat-miembros",
                    TextoPrincipal    = "500+",
                    TextoSecundario   = "Miembros Activos",
                    UrlImagen         = "bi-people-fill",
                    Orden             = 4,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Stats",
                    Clave             = "stat-experiencia",
                    TextoPrincipal    = "10+",
                    TextoSecundario   = "A\u00f1os de Experiencia",
                    UrlImagen         = "bi-calendar-check-fill",
                    Orden             = 5,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Stats",
                    Clave             = "stat-clases",
                    TextoPrincipal    = "20+",
                    TextoSecundario   = "Clases Semanales",
                    UrlImagen         = "bi-lightning-charge-fill",
                    Orden             = 6,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Stats",
                    Clave             = "stat-instructores",
                    TextoPrincipal    = "15+",
                    TextoSecundario   = "Instructores Certificados",
                    UrlImagen         = "bi-award-fill",
                    Orden             = 7,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "MisionVision",
                    Clave             = "mision",
                    TextoPrincipal    = "Nuestra Misi\u00f3n",
                    TextoSecundario   = "Empoderar a cada persona para alcanzar su m\u00e1ximo potencial f\u00edsico y mental, ofreciendo instalaciones de primer nivel, entrenadores certificados y un ambiente de comunidad inclusiva que inspire la transformaci\u00f3n continua.",
                    UrlImagen         = "bi-bullseye",
                    Orden             = 8,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "MisionVision",
                    Clave             = "vision",
                    TextoPrincipal    = "Nuestra Visi\u00f3n",
                    TextoSecundario   = "Ser el referente de excelencia en entrenamiento funcional y bienestar integral en la regi\u00f3n, reconocidos por formar no solo atletas, sino personas m\u00e1s fuertes, disciplinadas y comprometidas con una vida saludable.",
                    UrlImagen         = "bi-eye-fill",
                    Orden             = 9,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Valores",
                    Clave             = "valores-titulo",
                    TextoPrincipal    = "Lo que nos define",
                    TextoSecundario   = "",
                    UrlImagen         = "",
                    Orden             = 10,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Valores",
                    Clave             = "valor-excelencia",
                    TextoPrincipal    = "Excelencia",
                    TextoSecundario   = "Buscamos la mejora continua en cada aspecto: instalaciones, metodolog\u00eda de entrenamiento y atenci\u00f3n al cliente. Nunca conformarse con el m\u00ednimo.",
                    UrlImagen         = "bi-trophy-fill",
                    Orden             = 11,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Valores",
                    Clave             = "valor-compromiso",
                    TextoPrincipal    = "Compromiso",
                    TextoSecundario   = "Estar presentes en cada paso del camino de nuestros miembros, desde el primer d\u00eda hasta que sus metas se convierten en realidad.",
                    UrlImagen         = "bi-heart-fill",
                    Orden             = 12,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Valores",
                    Clave             = "valor-comunidad",
                    TextoPrincipal    = "Comunidad",
                    TextoSecundario   = "Construir relaciones aut\u00e9nticas entre miembros y entrenadores que van m\u00e1s all\u00e1 del gimnasio, creando una red de apoyo mutuo.",
                    UrlImagen         = "bi-people-fill",
                    Orden             = 13,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "Valores",
                    Clave             = "valor-integridad",
                    TextoPrincipal    = "Integridad",
                    TextoSecundario   = "Actuar con honestidad y transparencia en todo lo que hacemos, cumpliendo nuestras promesas y siendo responsables con nuestros miembros.",
                    UrlImagen         = "bi-shield-check",
                    Orden             = 14,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                },
                new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = "CTA",
                    Clave             = "nosotros-cta",
                    TextoPrincipal    = "\u00bfQUIERES SER PARTE DE NUESTRA FAMILIA?",
                    TextoSecundario   = "\u00danete a m\u00e1s de 500 miembros que ya transformaron su vida con J\u00e9na Training Methodology. Tu primer paso es el m\u00e1s importante.",
                    UrlImagen         = "",
                    Orden             = 15,
                    FechaModificacion = DateTime.Now,
                    Estado            = true
                }
            };

            contexto.ContenidoWeb.AddRange(secciones);
            await contexto.SaveChangesAsync();
        }
    }
}
