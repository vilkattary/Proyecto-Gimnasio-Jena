using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.AccesoADatos
    .Membresias.RegistrarMembresia
{
    public interface IRegistrarMembresiaAD
    {
        int RegistrarMembresia(
            MembresiaCrearDto membresia
        );

        int RegistrarMembresiaPendiente(
            int idUsuario,
            int idPlanMembresia
        );

        bool UsuarioTieneMembresiaActiva(
            int idUsuario
        );

        PlanMembresiaDatosDto ObtenerDatosPlan(
            int idPlanMembresia
        );
    }
}