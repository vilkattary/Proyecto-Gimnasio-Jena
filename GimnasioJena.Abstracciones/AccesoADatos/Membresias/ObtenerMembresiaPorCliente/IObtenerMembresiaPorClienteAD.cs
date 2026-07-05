using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.AccesoADatos.Membresias.ObtenerMembresiaPorCliente
{
    public interface IObtenerMembresiaPorClienteAD
    {
        MembresiaClienteDto ObtenerMembresiaActivaPorCliente(int idUsuario);
    }
}