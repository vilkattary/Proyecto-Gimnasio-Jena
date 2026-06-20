using GimnasioJena.Abstracciones.Modelos.Entrenadores;

namespace GimnasioJena.Abstracciones.AccesoADatos.Entrenadores.EditarEntrenador
{
    public interface IEditarEntrenadorAD
    {
        bool EditarEntrenador(EntrenadorEditarDto entrenador);
    }
}