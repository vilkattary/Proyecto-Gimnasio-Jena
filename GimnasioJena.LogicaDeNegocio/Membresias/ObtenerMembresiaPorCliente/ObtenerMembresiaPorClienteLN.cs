using GimnasioJena.Abstracciones.AccesoADatos.Membresias.ObtenerMembresiaPorCliente;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerMembresiaPorCliente;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.AccesoADatos.Membresias.ObtenerMembresiaPorCliente;

namespace GimnasioJena.LogicaDeNegocio.Membresias.ObtenerMembresiaPorCliente
{
    public class ObtenerMembresiaPorClienteLN : IObtenerMembresiaPorClienteLN
    {
        private readonly IObtenerMembresiaPorClienteAD _obtenerMembresiaPorClienteAD;

        public ObtenerMembresiaPorClienteLN()
        {
            _obtenerMembresiaPorClienteAD = new ObtenerMembresiaPorClienteAD();
        }

        public MembresiaClienteDto ObtenerMembresiaActivaPorCliente(int idUsuario)
        {
            if (idUsuario <= 0)
                return null;

            return _obtenerMembresiaPorClienteAD.ObtenerMembresiaActivaPorCliente(idUsuario);
        }
    }
}