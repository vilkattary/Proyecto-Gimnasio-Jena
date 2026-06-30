using GimnasioJena.Abstracciones.Modelos.Contacto;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.AccesoADatos.Contacto
{
    public interface IContactoAD
    {
        Task<ContactoInfoDto> ObtenerInfoContactoAsync();
        Task RegistrarMensajeAsync(string nombre, string correo, string telefono, string asunto, string mensaje);
        Task<string> ObtenerEmailAdminAsync();
    }
}
