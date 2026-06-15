using GimnasioJena.Abstracciones.AccesoADatos.Home.AgregarSeccionHome;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.AgregarSeccionHome;
using GimnasioJena.Abstracciones.Modelos.Home;
using GimnasioJena.AccesoADatos.Home.AgregarSeccionHome;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Home.AgregarSeccionHome
{
    public class AgregarSeccionHomeLN : IAgregarSeccionHomeLN
    {
        private readonly IAgregarSeccionHomeAD _repositorio;

        public AgregarSeccionHomeLN()
        {
            _repositorio = new AgregarSeccionHomeAD();
        }

        public async Task<bool> AgregarSeccionHome(AgregarSeccionHomeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Seccion))
                return false;

            return await _repositorio.AgregarSeccionHome(dto);
        }
    }
}
