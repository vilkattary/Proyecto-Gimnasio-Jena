using GimnasioJena.Abstracciones.Entidades.Home;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.AccesoADatos.Home.ObtenerSeccionesHome
{
    public interface IObtenerSeccionesHomeAD
    {
        Task<List<SeccionesHome>> ObtenerSeccionesHome();
    }
}
