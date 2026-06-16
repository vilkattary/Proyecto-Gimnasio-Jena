using GimnasioJena.Abstracciones.AccesoADatos.Home.EliminarSeccionHome;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.EliminarSeccionHome;
using GimnasioJena.AccesoADatos.Home.EliminarSeccionHome;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Home.EliminarSeccionHome
{
    public class EliminarSeccionHomeLN : IEliminarSeccionHomeLN
    {
        private readonly IEliminarSeccionHomeAD _repositorio;

        public EliminarSeccionHomeLN()
        {
            _repositorio = new EliminarSeccionHomeAD();
        }

        public async Task<bool> EliminarSeccionHome(int id)
        {
            if (id <= 0)
                return false;

            return await _repositorio.EliminarSeccionHome(id);
        }
    }
}
