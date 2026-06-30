using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerMembresiaPorCliente
{
    public interface IObtenerMembresiaPorClienteLN
    {
        MembresiaClienteDto ObtenerMembresiaActivaPorCliente(int idUsuario);
    }
}