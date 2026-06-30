using GimnasioJena.Abstracciones.AccesoADatos.Home.CambiarEstadoSeccionHome;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Home.CambiarEstadoSeccionHome
{
    public class CambiarEstadoSeccionHomeAD : ICambiarEstadoSeccionHomeAD
    {
        public async Task<bool> ToggleEstadoAsync(int id)
        {
            using (var contexto = new Contexto())
            {
                var seccion = await contexto.ContenidoWeb
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (seccion == null)
                    return false;

                if (seccion.Seccion == "Hero")
                    return false;

                seccion.Estado = !seccion.Estado;
                seccion.FechaModificacion = DateTime.Now;

                int filasAfectadas = await contexto.SaveChangesAsync();
                return filasAfectadas > 0;
            }
        }
    }
}
