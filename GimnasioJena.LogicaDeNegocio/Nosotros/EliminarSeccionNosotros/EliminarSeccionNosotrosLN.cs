using GimnasioJena.Abstracciones.AccesoADatos.Nosotros.EliminarSeccionNosotros;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Nosotros.EliminarSeccionNosotros;
using GimnasioJena.AccesoADatos.Nosotros.EliminarSeccionNosotros;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Nosotros.EliminarSeccionNosotros
{
    public class EliminarSeccionNosotrosLN : IEliminarSeccionNosotrosLN
    {
        private readonly IEliminarSeccionNosotrosAD _repositorio;

        public EliminarSeccionNosotrosLN()
        {
            _repositorio = new EliminarSeccionNosotrosAD();
        }

        public async Task<bool> EliminarSeccionNosotros(int id)
        {
            if (id <= 0)
                return false;

            return await _repositorio.EliminarSeccionNosotros(id);
        }
    }
}
