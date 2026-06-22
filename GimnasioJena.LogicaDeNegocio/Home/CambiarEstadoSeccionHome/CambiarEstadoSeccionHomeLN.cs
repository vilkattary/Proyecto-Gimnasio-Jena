using GimnasioJena.Abstracciones.AccesoADatos.Home.CambiarEstadoSeccionHome;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.CambiarEstadoSeccionHome;
using GimnasioJena.AccesoADatos.Home.CambiarEstadoSeccionHome;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Home.CambiarEstadoSeccionHome
{
    public class CambiarEstadoSeccionHomeLN : ICambiarEstadoSeccionHomeLN
    {
        private readonly ICambiarEstadoSeccionHomeAD _repositorio;

        public CambiarEstadoSeccionHomeLN()
        {
            _repositorio = new CambiarEstadoSeccionHomeAD();
        }

        public async Task<bool> EjecutarAsync(int id)
        {
            if (id <= 0)
                return false;

            return await _repositorio.ToggleEstadoAsync(id);
        }
    }
}
