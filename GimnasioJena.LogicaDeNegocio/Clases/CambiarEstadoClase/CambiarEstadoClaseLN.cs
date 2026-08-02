using GimnasioJena.Abstracciones.AccesoADatos.Clases.CambiarEstadoClase;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.CambiarEstadoClase;
using GimnasioJena.AccesoADatos.Clases.CambiarEstadoClase;

namespace GimnasioJena.LogicaDeNegocio.Clases.CambiarEstadoClase
{
    public class CambiarEstadoClaseLN : ICambiarEstadoClaseLN
    {
        private readonly ICambiarEstadoClaseAD
            _cambiarEstadoClaseAD;

        public CambiarEstadoClaseLN()
        {
            _cambiarEstadoClaseAD =
                new CambiarEstadoClaseAD();
        }

        public bool CambiarEstadoClase(int idClaseProgramada)
        {
            return _cambiarEstadoClaseAD
                .CambiarEstadoClase(idClaseProgramada);
        }
    }
}
