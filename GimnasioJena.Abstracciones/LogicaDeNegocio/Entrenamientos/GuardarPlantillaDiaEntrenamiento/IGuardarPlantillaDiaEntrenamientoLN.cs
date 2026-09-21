using GimnasioJena.Abstracciones.Modelos.Entrenamientos;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.GuardarPlantillaDiaEntrenamiento
{
    public interface IGuardarPlantillaDiaEntrenamientoLN
    {
        ResultadoGuardarPlantillaDto GuardarPlantillaDiaEntrenamiento(
            GuardarPlantillaDiaDto modelo
        );
    }
}
