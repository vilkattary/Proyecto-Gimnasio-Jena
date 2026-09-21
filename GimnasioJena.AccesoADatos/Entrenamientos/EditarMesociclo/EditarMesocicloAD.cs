using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.EditarMesociclo;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.AccesoADatos.Entidades.Entrenamientos;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Entrenamientos.EditarMesociclo
{
    public class EditarMesocicloAD : IEditarMesocicloAD
    {
        public bool EditarMesociclo(MesocicloDto modelo)
        {
            using (Contexto contexto = new Contexto())
            {
                MesocicloEntidad entidad = contexto.Mesociclos
                    .FirstOrDefault(m => m.idMesociclo == modelo.idMesociclo);

                if (entidad == null)
                {
                    return false;
                }

                entidad.Nombre = modelo.Nombre.Trim();
                entidad.FechaInicio = modelo.FechaInicio.Date;
                entidad.FechaFin = modelo.FechaFin.Date;
                entidad.EsActivo = modelo.EsActivo;

                contexto.SaveChanges();

                return true;
            }
        }
    }
}
