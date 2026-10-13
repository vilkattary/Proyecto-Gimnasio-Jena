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
            string nombreContacto = System.Net.WebUtility.HtmlEncode(dto.Nombre);
            string correoContacto = System.Net.WebUtility.HtmlEncode(dto.Correo);
            string telefonoContacto = System.Net.WebUtility.HtmlEncode(dto.Telefono);
            string asuntoContacto = System.Net.WebUtility.HtmlEncode(dto.Asunto);
            string mensajeContacto = System.Net.WebUtility.HtmlEncode(dto.Mensaje)
                .Replace("\r\n", "\n")
                .Replace("\n", "<br>");

            var cuerpoAdmin = $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
</head>
<body style='margin:0; padding:0; background-color:#F3F5F7;
             font-family:Arial,Helvetica,sans-serif;'>

    <table role='presentation' width='100%' cellpadding='0'
           cellspacing='0' style='background-color:#F3F5F7;'>
        <tr>
            <td align='center' style='padding:35px 15px;'>

                <table role='presentation' width='100%' cellpadding='0'
                       cellspacing='0'
                       style='max-width:600px; background-color:#FFFFFF;
                              border-radius:12px; overflow:hidden;'>

                    <tr>
                        <td align='center'
                            style='background-color:#1C2636;
                                   padding:35px 25px;'>
                            <h1 style='margin:0; color:#FFFFFF;
                                       font-size:27px; letter-spacing:2px;'>
                                JÉNA
                            </h1>
                            <p style='margin:8px 0 0; color:#FF6335;
                                      font-size:12px; letter-spacing:3px;
                                      font-weight:bold;'>
                                TRAINING METHODOLOGY
                            </p>
                        </td>
                    </tr>

                    <tr>
                        <td style='height:5px; background-color:#FF6335;'></td>
                    </tr>

                    <tr>
                        <td style='padding:38px 35px;'>

                            <h2 style='margin:0 0 12px; color:#1C2636;
                                       font-size:23px;'>
                                Nuevo mensaje de contacto
                            </h2>

                            <p style='margin:0 0 28px; color:#555555;
                                      font-size:15px; line-height:1.7;'>
                                Se ha recibido una nueva consulta desde
                                el formulario de contacto de JÉNA.
                            </p>

                            <table role='presentation' width='100%'
                                   cellpadding='0' cellspacing='0'
                                   style='border-collapse:collapse;
                                          background-color:#F5F7FA;'>

                                <tr>
                                    <td style='padding:14px 18px;
                                               border-bottom:1px solid #E1E5EA;'>
                                        <p style='margin:0 0 6px;
                                                  color:#FF6335;
                                                  font-size:11px;
                                                  font-weight:bold;
                                                  letter-spacing:1px;'>
                                            NOMBRE
                                        </p>
                                        <p style='margin:0; color:#1C2636;
                                                  font-size:15px;'>
                                            {nombreContacto}
                                        </p>
                                    </td>
                                </tr>

                                <tr>
                                    <td style='padding:14px 18px;
                                               border-bottom:1px solid #E1E5EA;'>
                                        <p style='margin:0 0 6px;
                                                  color:#FF6335;
                                                  font-size:11px;
                                                  font-weight:bold;
                                                  letter-spacing:1px;'>
                                            CORREO ELECTRÓNICO
                                        </p>
                                        <p style='margin:0; color:#1C2636;
                                                  font-size:15px;'>
                                            {correoContacto}
                                        </p>
                                    </td>
                                </tr>

                                <tr>
                                    <td style='padding:14px 18px;
                                               border-bottom:1px solid #E1E5EA;'>
                                        <p style='margin:0 0 6px;
                                                  color:#FF6335;
                                                  font-size:11px;
                                                  font-weight:bold;
                                                  letter-spacing:1px;'>
                                            TELÉFONO
                                        </p>
                                        <p style='margin:0; color:#1C2636;
                                                  font-size:15px;'>
                                            {telefonoContacto}
                                        </p>
                                    </td>
                                </tr>

                                <tr>
                                    <td style='padding:14px 18px;'>
                                        <p style='margin:0 0 6px;
                                                  color:#FF6335;
                                                  font-size:11px;
                                                  font-weight:bold;
                                                  letter-spacing:1px;'>
                                            ASUNTO
                                        </p>
                                        <p style='margin:0; color:#1C2636;
                                                  font-size:15px;'>
                                            {asuntoContacto}
                                        </p>
                                    </td>
                                </tr>

                            </table>

                            <table role='presentation' width='100%'
                                   cellpadding='0' cellspacing='0'
                                   style='margin-top:24px;
                                          border-left:4px solid #FF6335;
                                          background-color:#F5F7FA;'>
                                <tr>
                                    <td style='padding:20px;'>
                                        <p style='margin:0 0 12px;
                                                  color:#1C2636;
                                                  font-size:13px;
                                                  font-weight:bold;'>
                                            MENSAJE RECIBIDO
                                        </p>
                                        <p style='margin:0; color:#444444;
                                                  font-size:15px;
                                                  line-height:1.8;
                                                  overflow-wrap:break-word;'>
                                            {mensajeContacto}
                                        </p>
                                    </td>
                                </tr>
                            </table>

                            <p style='margin:28px 0 0; color:#666666;
                                      font-size:13px; line-height:1.7;'>
                                Podés utilizar el correo electrónico
                                proporcionado para responder directamente
                                a esta consulta.
                            </p>

                        </td>
                    </tr>

                    <tr>
                        <td align='center'
                            style='background-color:#1C2636;
                                   padding:24px 20px;'>
                            <p style='margin:0; color:#FFFFFF;
                                      font-size:12px;'>
                                JÉNA TRAINING METHODOLOGY
                            </p>
                            <p style='margin:10px 0 0; color:#B9C0C9;
                                      font-size:11px;'>
                                Notificación automática del formulario de contacto.
                            </p>
                        </td>
                    </tr>

                </table>

            </td>
        </tr>
    </table>

</body>
</html>";

            await _email.EnviarAsync(
                correoAdmin,
                "Nuevo contacto: " + dto.Asunto,
                cuerpoAdmin);

            // 4. Confirmación de recepción al usuario
            string nombreCliente = System.Net.WebUtility.HtmlEncode(dto.Nombre);

            var cuerpoUsuario = $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
</head>
<body style='margin:0; padding:0; background-color:#F3F5F7;
             font-family:Arial,Helvetica,sans-serif;'>

    <table role='presentation' width='100%' cellpadding='0'
           cellspacing='0' style='background-color:#F3F5F7;'>
        <tr>
            <td align='center' style='padding:35px 15px;'>

                <table role='presentation' width='100%' cellpadding='0'
                       cellspacing='0'
                       style='max-width:600px; background-color:#FFFFFF;
                              border-radius:12px; overflow:hidden;'>

                    <tr>
                        <td align='center'
                            style='background-color:#1C2636;
                                   padding:35px 25px;'>
                            <h1 style='margin:0; color:#FFFFFF;
                                       font-size:27px; letter-spacing:2px;'>
                                JÉNA
                            </h1>
                            <p style='margin:8px 0 0; color:#FF6335;
                                      font-size:12px; letter-spacing:3px;
                                      font-weight:bold;'>
                                TRAINING METHODOLOGY
                            </p>
                        </td>
                    </tr>

                    <tr>
                        <td style='height:5px; background-color:#FF6335;'></td>
                    </tr>

                    <tr>
                        <td style='padding:38px 35px;'>

                            <h2 style='margin:0 0 22px; color:#1C2636;
                                       font-size:23px;'>
                                ¡Mensaje recibido!
                            </h2>

                            <p style='color:#333333; font-size:16px;
                                      line-height:1.7;'>
                                Hola, <strong>{nombreCliente}</strong>.
                            </p>

                            <p style='color:#555555; font-size:15px;
                                      line-height:1.8;'>
                                Gracias por ponerte en contacto con
                                <strong>JÉNA Training Methodology</strong>.
                                Hemos recibido tu mensaje correctamente.
                            </p>

                            <table role='presentation' width='100%'
                                   cellpadding='0' cellspacing='0'
                                   style='margin:25px 0;
                                          background-color:#F5F7FA;
                                          border-left:4px solid #FF6335;'>
                                <tr>
                                    <td style='padding:20px;'>
                                        <p style='margin:0; color:#1C2636;
                                                  font-size:15px;
                                                  line-height:1.7;'>
                                            Nuestro equipo revisará tu consulta
                                            y se pondrá en contacto contigo
                                            a la brevedad posible.
                                        </p>
                                    </td>
                                </tr>
                            </table>

                            <p style='color:#555555; font-size:15px;
                                      line-height:1.8;'>
                                Agradecemos tu interés y la confianza
                                en nuestro equipo.
                            </p>

                            <p style='margin-top:30px; color:#1C2636;
                                      font-size:15px; line-height:1.7;'>
                                ¡Nos vemos pronto!<br>
                                <strong>Equipo JÉNA</strong>
                            </p>

                        </td>
                    </tr>

                    <tr>
                        <td align='center'
                            style='background-color:#1C2636;
                                   padding:24px 20px;'>
                            <p style='margin:0; color:#FFFFFF;
                                      font-size:12px;'>
                                JÉNA TRAINING METHODOLOGY
                            </p>
                            <p style='margin:10px 0 0; color:#B9C0C9;
                                      font-size:11px;'>
                                Este es un correo automático de confirmación.
                            </p>
                        </td>
                    </tr>

                </table>

            </td>
        </tr>
    </table>

</body>
</html>";

            await _email.EnviarAsync(
                dto.Correo,
                "Confirmacion de contacto - Gimnasio Jena",
                cuerpoUsuario);
        }
    }
}
