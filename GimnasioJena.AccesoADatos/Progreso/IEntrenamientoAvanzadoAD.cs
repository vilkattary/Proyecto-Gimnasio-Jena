using System.Collections.Generic;
using System.Threading.Tasks;
using GimnasioJena.AccesoADatos.Entidades.Progreso;

namespace GimnasioJena.AccesoADatos.Progreso
{
    public interface IEntrenamientoAvanzadoAD
    {
        Task<List<EntrenamientoEntidad>> ObtenerPlantillasDisponiblesAD();

        Task<EntrenamientoEntidad> ObtenerEntrenamientoPorIdAD(int idEntrenamiento);

        Task<int> InsertarEntrenamientoFlexibleAD(EntrenamientoEntidad entidad);

        Task GuardarEntrenamientoAD(EntrenamientoEntidad entidad);

        Task<EntrenamientoEntidad> ObtenerEntrenamientoPorClaseAD(int idClaseProgramada);
    }
}
