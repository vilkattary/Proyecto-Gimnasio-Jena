using GimnasioJena.Abstracciones.AccesoADatos.Progreso;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Progreso
{
    public class UsuarioActualAD : IUsuarioActualAD
    {
        private readonly Contexto _elContexto;

        public UsuarioActualAD()
        {
            _elContexto = new Contexto();
        }

        public async Task<int?> ObtenerIdUsuarioPorIdentityAD(string identityUserId)
        {
            var usuario = await _elContexto.Usuarios
                .FirstOrDefaultAsync(u => u.identityUserId == identityUserId);

            return usuario?.idUsuario;
        }
    }
}
