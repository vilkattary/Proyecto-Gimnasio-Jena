using GimnasioJena.Abstracciones.AccesoADatos.Nosotros.CambiarEstadoSeccionNosotros;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Nosotros.CambiarEstadoSeccionNosotros;
using GimnasioJena.AccesoADatos.Nosotros.CambiarEstadoSeccionNosotros;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Nosotros.CambiarEstadoSeccionNosotros
{
    public class CambiarEstadoSeccionNosotrosLN : ICambiarEstadoSeccionNosotrosLN
    {
        private readonly ICambiarEstadoSeccionNosotrosAD _repositorio;

        public CambiarEstadoSeccionNosotrosLN()
        {
            _repositorio = new CambiarEstadoSeccionNosotrosAD();
        }

        public async Task<bool> EjecutarAsync(int id)
        {
            if (id <= 0)
                return false;

            return await _repositorio.ToggleEstadoAsync(id);
        }
    }
}
