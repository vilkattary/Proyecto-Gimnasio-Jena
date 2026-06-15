using GimnasioJena.Abstracciones.AccesoADatos.Home.ModificarSeccionHome;
using GimnasioJena.Abstracciones.Modelos.Home;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Home.ModificarSeccionHome
{
    public class ModificarSeccionHomeAD : IModificarSeccionHomeAD
    {
        public async Task<bool> ModificarSeccionHome(ModificarSeccionHomeDto dto)
        {
            using (var contexto = new Contexto())
            {
                var seccion = await contexto.SeccionesHome
                    .FirstOrDefaultAsync(s => s.Id == dto.Id);

                if (seccion == null)
                    return false;

                seccion.TextoPrincipal  = dto.TextoPrincipal;
                seccion.TextoSecundario = dto.TextoSecundario;

                if (!string.IsNullOrWhiteSpace(dto.UrlImagen))
                    seccion.UrlImagen = dto.UrlImagen;
                // Si dto.UrlImagen llega null/vacío (no se subió imagen nueva),
                // se conserva el valor ya guardado en BD.

                seccion.FechaModificacion = DateTime.Now;

                await contexto.SaveChangesAsync();
                return true;
            }
        }
    }
}
