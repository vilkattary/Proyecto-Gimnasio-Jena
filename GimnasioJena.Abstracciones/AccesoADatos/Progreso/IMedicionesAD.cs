using System.Collections.Generic;
using System.Threading.Tasks;
using GimnasioJena.Abstracciones.Modelos.Progreso;

namespace GimnasioJena.Abstracciones.AccesoADatos.Progreso
{
    public interface IMedicionesAD
    {
        Task<List<MedicionDinamicaDto>> ObtenerMedicionesUsuarioAD(int idUsuario);

        Task<string> ObtenerNombreClienteAD(int idUsuario);

        Task GuardarMedicionesAD(int idUsuario, List<ValorMedicionDto> valores);
    }
}
