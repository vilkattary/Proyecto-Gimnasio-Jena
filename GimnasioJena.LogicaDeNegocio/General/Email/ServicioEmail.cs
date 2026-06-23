using GimnasioJena.Abstracciones.General.Email;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.General.Email
{
    public class ServicioEmail : IServicioEmail
    {
        public async Task EnviarAsync(string destinatario, string asunto, string cuerpoHtml)
        {
            var host    = ConfigurationManager.AppSettings["SmtpHost"];
            var port    = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            var usuario = ConfigurationManager.AppSettings["SmtpUser"];
            var clave   = (ConfigurationManager.AppSettings["SmtpPassword"] ?? "").Replace(" ", "");
            var remite  = ConfigurationManager.AppSettings["SmtpFrom"];

            using (var cliente = new SmtpClient(host, port))
            {
                cliente.EnableSsl   = true;
                cliente.Credentials = new NetworkCredential(usuario, clave);

                var correo = new MailMessage
                {
                    From       = new MailAddress(remite, "Gimnasio Jena"),
                    Subject    = asunto,
                    Body       = cuerpoHtml,
                    IsBodyHtml = true
                };

                correo.To.Add(destinatario);

                await cliente.SendMailAsync(correo).ConfigureAwait(false);
            }
        }
    }
}
