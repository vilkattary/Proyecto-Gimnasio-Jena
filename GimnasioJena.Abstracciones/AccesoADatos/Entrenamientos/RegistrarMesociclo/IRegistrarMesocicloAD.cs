using GimnasioJena.Abstracciones.Modelos.Entrenamientos;

namespace GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.RegistrarMesociclo
{
    public interface IRegistrarMesocicloAD
    {
        int RegistrarMesociclo(MesocicloDto modelo);
    }
}
