using GimnasioJena.Abstracciones.Modelos.Entrenadores;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenadores.EditarEntrenador
{
    public interface IEditarEntrenadorLN
    {
        bool EditarEntrenador(EntrenadorEditarDto entrenador);
    }
}