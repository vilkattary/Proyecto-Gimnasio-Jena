using GimnasioJena.Abstracciones.AccesoADatos.Nosotros.AgregarSeccionNosotros;
using GimnasioJena.Abstracciones.Modelos.Home;
using GimnasioJena.AccesoADatos.Entidades.Home;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Nosotros.AgregarSeccionNosotros
{
    public class AgregarSeccionNosotrosAD : IAgregarSeccionNosotrosAD
    {
        // Stats máximo 3, Valores máximo 4
        private const int MaxStats  = 3;
        private const int MaxValores = 4;

        public async Task<bool> AgregarSeccionNosotros(AgregarSeccionHomeDto dto)
        {
            using (var contexto = new Contexto())
            {
                int limite = dto.Seccion == "Stats" ? MaxStats : MaxValores;

                int cantidad = contexto.ContenidoWeb
                    .Count(s => s.Pagina == "Nosotros" && s.Seccion == dto.Seccion);

                if (cantidad >= limite)
                    return false;

                int maxOrden = contexto.ContenidoWeb.Any(s => s.Pagina == "Nosotros")
                    ? contexto.ContenidoWeb.Where(s => s.Pagina == "Nosotros").Max(s => s.Orden)
                    : 0;

                string sufijo = (cantidad + 1).ToString();
                string clave  = "nosotros-" + dto.Seccion.ToLower() + "-" + sufijo;

                var nueva = new ContenidoWeb
                {
                    Pagina            = "Nosotros",
                    Seccion           = dto.Seccion,
                    Clave             = clave,
                    TextoPrincipal    = dto.Seccion == "Stats"   ? "0+"          : "Nuevo Valor",
                    TextoSecundario   = dto.Seccion == "Stats"   ? "Nueva estadística" : "Descripción del valor",
                    UrlImagen         = "bi-star-fill",
                    Orden             = maxOrden + 1,
                    FechaModificacion = DateTime.Now
                };

                contexto.ContenidoWeb.Add(nueva);
                int filas = await contexto.SaveChangesAsync();
                return filas > 0;
            }
        }
    }
}
