using GimnasioJena.Abstracciones.AccesoADatos.Entrenadores.ObtenerEntrenadorPorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenadores.ObtenerEntrenadorPorId;
using GimnasioJena.Abstracciones.Modelos.Entrenadores;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Entrenadores.ObtenerEntrenadorPorId
{
    public class ObtenerEntrenadorPorIdLN : IObtenerEntrenadorPorIdLN
    {

        private readonly IObtenerEntrenadorPorIdAD _repositorio;

        public ObtenerEntrenadorPorIdLN(IObtenerEntrenadorPorIdAD repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<EntrenadorDto> ObtenerEntrenadorPorId(string identityUserId)
        {

            if (string.IsNullOrWhiteSpace(identityUserId))
                return null;

            return await _repositorio.ObtenerEntrenadorPorId(identityUserId);

        }

    }
}
