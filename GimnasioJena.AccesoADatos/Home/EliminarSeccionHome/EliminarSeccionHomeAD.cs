using GimnasioJena.Abstracciones.AccesoADatos.Home.EliminarSeccionHome;
using System.Data.Entity;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Home.EliminarSeccionHome
{
    public class EliminarSeccionHomeAD : IEliminarSeccionHomeAD
    {
        public async Task<bool> EliminarSeccionHome(int id)
        {
            using (var contexto = new Contexto())
            {
                var seccion = await contexto.ContenidoWeb
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (seccion == null)
                    return false;

                contexto.ContenidoWeb.Remove(seccion);
                int filasAfectadas = await contexto.SaveChangesAsync();
                return filasAfectadas > 0;
            }
        }
    }
}
