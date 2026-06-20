using GimnasioJena.Abstracciones.AccesoADatos.Nosotros.AgregarSeccionNosotros;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Nosotros.AgregarSeccionNosotros;
using GimnasioJena.Abstracciones.Modelos.Home;
using GimnasioJena.AccesoADatos.Nosotros.AgregarSeccionNosotros;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Nosotros.AgregarSeccionNosotros
{
    public class AgregarSeccionNosotrosLN : IAgregarSeccionNosotrosLN
    {
        private readonly IAgregarSeccionNosotrosAD _repositorio;

        public AgregarSeccionNosotrosLN()
        {
            _repositorio = new AgregarSeccionNosotrosAD();
        }

        public async Task<bool> AgregarSeccionNosotros(AgregarSeccionHomeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Seccion))
                return false;

            return await _repositorio.AgregarSeccionNosotros(dto);
        }
    }
}
