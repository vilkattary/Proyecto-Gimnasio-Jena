using GimnasioJena.Abstracciones.Modelos.Entrenamientos;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.RegistrarMesociclo
{
    public interface IRegistrarMesocicloLN
    {
        int RegistrarMesociclo(MesocicloDto modelo);
    }
}
