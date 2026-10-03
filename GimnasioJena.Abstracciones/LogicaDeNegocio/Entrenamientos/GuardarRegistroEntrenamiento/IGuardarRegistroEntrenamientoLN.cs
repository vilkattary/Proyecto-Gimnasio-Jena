using GimnasioJena.Abstracciones.Modelos.Entrenamientos;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.GuardarRegistroEntrenamiento
{
    public interface IGuardarRegistroEntrenamientoLN
    {
        ResultadoRegistroEntrenamientoDto GuardarRegistroEntrenamiento(
            RegistrarEntrenamientoClienteDto modelo);
    }
}
