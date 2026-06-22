using GimnasioJena.Abstracciones.AccesoADatos.Nosotros.CambiarEstadoSeccionNosotros;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Nosotros.CambiarEstadoSeccionNosotros
{
    public class CambiarEstadoSeccionNosotrosAD : ICambiarEstadoSeccionNosotrosAD
    {
        public async Task<bool> ToggleEstadoAsync(int id)
        {
            using (var contexto = new Contexto())
            {
                var seccion = await contexto.ContenidoWeb
                    .FirstOrDefaultAsync(s => s.Id == id && s.Pagina == "Nosotros");

                if (seccion == null)
                    return false;

                // Header y CTA no deben desactivarse
                if (seccion.Seccion == "Header" || seccion.Seccion == "CTA")
                    return false;

                seccion.Estado = !seccion.Estado;
                seccion.FechaModificacion = DateTime.Now;

                int filasAfectadas = await contexto.SaveChangesAsync();
                return filasAfectadas > 0;
            }
        }
    }
}
