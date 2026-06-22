using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.General.Email
{
    public interface IServicioEmail
    {
        Task EnviarAsync(string destinatario, string asunto, string cuerpoHtml);
    }
}
