using GimnasioJena.Abstracciones.AccesoADatos.Home.AgregarSeccionHome;
using GimnasioJena.AccesoADatos.Entidades.Home;
using GimnasioJena.Abstracciones.Modelos.Home;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Home.AgregarSeccionHome
{
    public class AgregarSeccionHomeAD : IAgregarSeccionHomeAD
    {
        private const int MaxPorSeccion = 3;

        public async Task<bool> AgregarSeccionHome(AgregarSeccionHomeDto dto)
        {
            using (var contexto = new Contexto())
            {
                int cantidad = contexto.ContenidoWeb
                    .Count(s => s.Seccion == dto.Seccion);

                if (cantidad >= MaxPorSeccion)
                    return false;

                int maxOrden = contexto.ContenidoWeb.Any()
                    ? contexto.ContenidoWeb.Max(s => s.Orden)
                    : 0;

                string clave = dto.Seccion.ToLower() + "-nuevo-" + (cantidad + 1);

                var nueva = new ContenidoWeb
                {
                    Pagina            = "Home",
                    Seccion           = dto.Seccion,
                    Clave             = clave,
                    TextoPrincipal    = "Nueva " + dto.Seccion,
                    TextoSecundario   = "",
                    UrlImagen         = dto.Seccion == "Clases" ? "bi-star-fill" : "",
                    Orden             = maxOrden + 1,
                    FechaModificacion = DateTime.Now
                };

                contexto.ContenidoWeb.Add(nueva);
                int filasAfectadas = await contexto.SaveChangesAsync();
                return filasAfectadas > 0;
            }
        }
    }
}
