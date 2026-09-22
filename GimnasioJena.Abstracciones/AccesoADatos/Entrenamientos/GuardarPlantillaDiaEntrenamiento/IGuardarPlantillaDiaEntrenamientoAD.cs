using GimnasioJena.Abstracciones.Modelos.Entrenamientos;

namespace GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.GuardarPlantillaDiaEntrenamiento
{
    public interface IGuardarPlantillaDiaEntrenamientoAD
    {
        ResultadoGuardarPlantillaDto GuardarPlantillaDiaEntrenamiento(
            GuardarPlantillaDiaDto modelo
        );
    }
}
