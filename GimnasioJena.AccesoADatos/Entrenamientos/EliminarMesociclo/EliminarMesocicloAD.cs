using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.EliminarMesociclo;
using GimnasioJena.AccesoADatos.Entidades.Entrenamientos;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Entrenamientos.EliminarMesociclo
{
    public class EliminarMesocicloAD : IEliminarMesocicloAD
    {
        public bool EliminarMesociclo(int idMesociclo)
        {
            using (Contexto contexto = new Contexto())
            {
                MesocicloEntidad entidad = contexto.Mesociclos
                    .FirstOrDefault(m => m.idMesociclo == idMesociclo);

                if (entidad == null)
                {
                    return false;
                }

                contexto.Mesociclos.Remove(entidad);
                contexto.SaveChanges();

                return true;
            }
        }
    }
}
