using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.AccesoADatos.Membresias.RegistrarMembresia
{
    public interface IRegistrarMembresiaAD
    {
        int RegistrarMembresia(MembresiaCrearDto membresia);
        bool UsuarioTieneMembresiaActiva(int idUsuario);
    }
}