using GimnasioJena.Abstracciones.AccesoADatos.Entrenadores.EditarEntrenador;
using GimnasioJena.Abstracciones.Modelos.Entrenadores;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Entrenadores.EditarEntrenador
{
    public class EditarEntrenadorAD : IEditarEntrenadorAD
    {
        public bool EditarEntrenador(EntrenadorEditarDto entrenador)
        {
            using (var contexto = new Contexto())
            {
                var entidad = contexto.Entrenadores
                    .FirstOrDefault(e => e.idEntrenador == entrenador.idEntrenador);

                if (entidad == null)
                    return false;

                entidad.especialidad = entrenador.especialidad;
                entidad.descripcion = entrenador.descripcion;
                entidad.fechaContratacion = entrenador.fechaContratacion;
                entidad.estado = entrenador.estado;

                return contexto.SaveChanges() > 0;
            }
        }
    }
}