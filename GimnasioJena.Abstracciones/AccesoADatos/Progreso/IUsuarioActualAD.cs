using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.AccesoADatos.Progreso
{
    public interface IUsuarioActualAD
    {
        Task<int?> ObtenerIdUsuarioPorIdentityAD(string identityUserId);
    }
}
