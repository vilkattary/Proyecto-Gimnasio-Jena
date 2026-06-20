using GimnasioJena.Abstracciones.AccesoADatos.Home.ModificarSeccionHome;
using GimnasioJena.Abstracciones.Modelos.Home;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Home.ModificarSeccionHome
{
    public class ModificarContenidoWebAD : IModificarContenidoWebAD
    {
        public async Task<bool> ModificarContenidoWeb(ModificarContenidoWebDto dto)
        {
            using (var contexto = new Contexto())
            {
                var contenido = await contexto.ContenidoWeb
                    .FirstOrDefaultAsync(s => s.Id == dto.Id);

                if (contenido == null)
                    return false;

                contenido.TextoPrincipal  = dto.TextoPrincipal;
                contenido.TextoSecundario = dto.TextoSecundario;

                if (!string.IsNullOrWhiteSpace(dto.UrlImagen))
                    contenido.UrlImagen = dto.UrlImagen;
                // Si dto.UrlImagen llega null/vacío (no se subió imagen nueva),
                // se conserva el valor ya guardado en BD.

                contenido.FechaModificacion = DateTime.Now;

                await contexto.SaveChangesAsync();
                return true;
            }
        }
    }
}
