using GimnasioJena.Abstracciones.Modelos.Contacto;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Contacto
{
    public interface IContactoLN
    {
        Task<ContactoInfoDto> ObtenerInfoContactoAsync();
        Task ProcesarNuevoMensajeAsync(ContactoFormDto dto);
    }
}
