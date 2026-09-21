using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.RegistrarMesociclo;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.AccesoADatos.Entidades.Entrenamientos;

namespace GimnasioJena.AccesoADatos.Entrenamientos.RegistrarMesociclo
{
    public class RegistrarMesocicloAD : IRegistrarMesocicloAD
    {
        public int RegistrarMesociclo(MesocicloDto modelo)
        {
            using (Contexto contexto = new Contexto())
            {
                MesocicloEntidad entidad = new MesocicloEntidad
                {
                    Nombre = modelo.Nombre.Trim(),
                    FechaInicio = modelo.FechaInicio.Date,
                    FechaFin = modelo.FechaFin.Date,
                    EsActivo = modelo.EsActivo
                };

                contexto.Mesociclos.Add(entidad);
                contexto.SaveChanges();

                return entidad.idMesociclo;
            }
        }
    }
}
