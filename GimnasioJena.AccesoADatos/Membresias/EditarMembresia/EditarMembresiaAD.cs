using GimnasioJena.Abstracciones.AccesoADatos.Membresias.EditarMembresia;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Membresias.EditarMembresia
{
    public class EditarMembresiaAD : IEditarMembresiaAD
    {
        private readonly Contexto _contexto;

        public EditarMembresiaAD()
        {
            _contexto = new Contexto();
        }

        public int EditarMembresia(MembresiaEditarDto membresia)
        {
            var membresiaBD = _contexto.Membresias
                .FirstOrDefault(m =>
                    m.idMembresiaCliente ==
                    membresia.idMembresiaCliente);

            if (membresiaBD == null)
                return 0;

            var planBD = _contexto.PlanesMembresia
                .FirstOrDefault(p =>
                    p.idPlanMembresia ==
                    membresiaBD.idPlanMembresia);

            if (planBD == null)
                return 0;

            if (planBD.duracionDias <= 0)
                return 0;

            /*
             * Únicamente se permite editar:
             * - Estado
             * - Fecha de inicio
             * - Observaciones
             */

            membresiaBD.idEstadoMembresia =
                membresia.idEstadoMembresia;

            membresiaBD.fechaInicio =
                membresia.fechaInicio;

            membresiaBD.fechaFin =
                membresia.fechaInicio
                    .AddDays(planBD.duracionDias - 1);

            membresiaBD.observaciones =
                membresia.observaciones;

            return _contexto.SaveChanges();
        }
    }
}