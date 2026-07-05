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
                .FirstOrDefault(m => m.idMembresiaCliente == membresia.idMembresiaCliente);

            if (membresiaBD == null)
                return 0;

            membresiaBD.idUsuario = membresia.idUsuario;
            membresiaBD.idPlanMembresia = membresia.idPlanMembresia;
            membresiaBD.idEstadoMembresia = membresia.idEstadoMembresia;
            membresiaBD.fechaInicio = membresia.fechaInicio;
            membresiaBD.fechaFin = membresia.fechaFin;
            membresiaBD.clasesDisponibles = membresia.clasesDisponibles;
            membresiaBD.observaciones = membresia.observaciones;

            return _contexto.SaveChanges();
        }
    }
}