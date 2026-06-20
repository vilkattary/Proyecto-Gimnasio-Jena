using GimnasioJena.Abstracciones.AccesoADatos.Entrenadores.EditarEntrenador;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenadores.EditarEntrenador;
using GimnasioJena.Abstracciones.Modelos.Entrenadores;
using GimnasioJena.AccesoADatos.Entrenadores.EditarEntrenador;

namespace GimnasioJena.LogicaDeNegocio.Entrenadores.EditarEntrenador
{
    public class EditarEntrenadorLN : IEditarEntrenadorLN
    {
        private readonly IEditarEntrenadorAD _editarEntrenadorAD;

        public EditarEntrenadorLN()
        {
            _editarEntrenadorAD = new EditarEntrenadorAD();
        }

        public bool EditarEntrenador(EntrenadorEditarDto entrenador)
        {
            if (entrenador == null)
                return false;

            if (entrenador.idEntrenador <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(entrenador.especialidad))
                return false;

            if (string.IsNullOrWhiteSpace(entrenador.descripcion))
                return false;

            return _editarEntrenadorAD.EditarEntrenador(entrenador);
        }
    }
}