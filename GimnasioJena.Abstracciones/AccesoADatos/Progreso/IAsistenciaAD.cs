using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.AccesoADatos.Progreso
{
    public interface IAsistenciaAD
    {
        Task<int> ObtenerAsistenciaUltimos30DiasAD(int idUsuario);
    }
}
