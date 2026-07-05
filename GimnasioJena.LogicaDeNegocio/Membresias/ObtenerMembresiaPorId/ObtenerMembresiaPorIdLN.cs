using GimnasioJena.Abstracciones.AccesoADatos.Membresias.ObtenerMembresiaPorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerMembresiaPorId;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.AccesoADatos.Membresias.ObtenerMembresiaPorId;

namespace GimnasioJena.LogicaDeNegocio.Membresias.ObtenerMembresiaPorId
{
    public class ObtenerMembresiaPorIdLN : IObtenerMembresiaPorIdLN
    {
        private readonly IObtenerMembresiaPorIdAD _obtenerMembresiaPorIdAD;

        public ObtenerMembresiaPorIdLN()
        {
            _obtenerMembresiaPorIdAD = new ObtenerMembresiaPorIdAD();
        }

        public MembresiaEditarDto ObtenerMembresiaPorId(int idMembresiaCliente)
        {
            if (idMembresiaCliente <= 0)
                return null;

            return _obtenerMembresiaPorIdAD.ObtenerMembresiaPorId(idMembresiaCliente);
        }
        public MembresiaClienteDto ObtenerDetalleMembresiaPorId(int idMembresiaCliente)
        {
            if (idMembresiaCliente <= 0)
                return null;

            return _obtenerMembresiaPorIdAD.ObtenerDetalleMembresiaPorId(idMembresiaCliente);
        }
    }
}