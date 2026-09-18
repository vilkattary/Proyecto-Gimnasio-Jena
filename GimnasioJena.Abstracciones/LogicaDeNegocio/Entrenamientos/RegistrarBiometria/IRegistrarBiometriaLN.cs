using GimnasioJena.Abstracciones.Modelos.Entrenamientos;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.RegistrarBiometria
{
    public interface IRegistrarBiometriaLN
    {
        ResultadoBiometriaDto RegistrarBiometria(RegistrarBiometriaDto modelo);
    }
}
