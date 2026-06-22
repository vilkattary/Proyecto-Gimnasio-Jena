using GimnasioJena.Abstracciones.AccesoADatos.Contacto;
using GimnasioJena.Abstracciones.General.Email;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Contacto;
using GimnasioJena.Abstracciones.Modelos.Contacto;
using GimnasioJena.AccesoADatos.Contacto;
using GimnasioJena.LogicaDeNegocio.General.Email;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Contacto
{
    public class ContactoLN : IContactoLN
    {
        private readonly IContactoAD    _repositorio;
        private readonly IServicioEmail _email;

        public ContactoLN()
        {
            _repositorio = new ContactoAD();
            _email       = new ServicioEmail();
        }

        public async Task<ContactoInfoDto> ObtenerInfoContactoAsync()
        {
            return await _repositorio.ObtenerInfoContactoAsync();
        }

        public async Task ProcesarNuevoMensajeAsync(ContactoFormDto dto)
        {
            // 1. Guardar en BD - si falla no bloquea el envio del correo
            try
            {
                await _repositorio.RegistrarMensajeAsync(
                    dto.Nombre,
                    dto.Correo,
                    dto.Telefono,
                    dto.Asunto,
                    dto.Mensaje);
            }
            catch (Exception) { }

            // 2. Obtener email del admin desde AspNetUsers (rol ADMINISTRADOR)
            //    Fallback a SmtpUser si no hay admin registrado en la BD
            string correoAdmin;
            try
            {
                correoAdmin = await _repositorio.ObtenerEmailAdminAsync();
            }
            catch (Exception)
            {
                correoAdmin = null;
            }

            if (string.IsNullOrWhiteSpace(correoAdmin))
                correoAdmin = ConfigurationManager.AppSettings["SmtpUser"];

            // 3. Correo al administrador con los datos del formulario
            var cuerpoAdmin =
                "<h2>Nuevo mensaje de contacto</h2>"                       +
                $"<p><strong>Nombre:</strong>         {dto.Nombre}</p>"    +
                $"<p><strong>Correo:</strong>         {dto.Correo}</p>"    +
                $"<p><strong>Tel&eacute;fono:</strong>{dto.Telefono}</p>"  +
                $"<p><strong>Asunto:</strong>         {dto.Asunto}</p>"    +
                $"<p><strong>Mensaje:</strong>        {dto.Mensaje}</p>";

            await _email.EnviarAsync(
                correoAdmin,
                "Nuevo contacto: " + dto.Asunto,
                cuerpoAdmin);

            // 4. Confirmacion de recepcion al usuario
            var cuerpoUsuario =
                "<p>Hemos recibido tu mensaje correctamente.</p>"                  +
                "<p>Nos pondremos en contacto contigo a la brevedad posible.</p>" +
                "<p><strong>Gimnasio Jena</strong></p>";

            await _email.EnviarAsync(
                dto.Correo,
                "Confirmacion de contacto - Gimnasio Jena",
                cuerpoUsuario);
        }
    }
}
