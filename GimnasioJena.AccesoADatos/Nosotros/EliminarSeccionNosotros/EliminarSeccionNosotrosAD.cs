using GimnasioJena.Abstracciones.AccesoADatos.Nosotros.EliminarSeccionNosotros;
using System.Data.Entity;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Nosotros.EliminarSeccionNosotros
{
    public class EliminarSeccionNosotrosAD : IEliminarSeccionNosotrosAD
    {
        public async Task<bool> EliminarSeccionNosotros(int id)
        {
            using (var contexto = new Contexto())
            {
                var seccion = await contexto.ContenidoWeb
                    .FirstOrDefaultAsync(s => s.Id == id && s.Pagina == "Nosotros");

                if (seccion == null)
                    return false;

                contexto.ContenidoWeb.Remove(seccion);
                int filas = await contexto.SaveChangesAsync();
                return filas > 0;
            }
        }
    }
}
