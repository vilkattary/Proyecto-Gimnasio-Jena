using GimnasioJena.Abstracciones.Modelos.Entrenamientos;

namespace GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.EditarMesociclo
{
    public interface IEditarMesocicloAD
    {
        bool EditarMesociclo(MesocicloDto modelo);
    }
}
