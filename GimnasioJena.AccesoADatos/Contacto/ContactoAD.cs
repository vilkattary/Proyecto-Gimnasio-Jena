using GimnasioJena.Abstracciones.AccesoADatos.Contacto;
using GimnasioJena.Abstracciones.Modelos.Contacto;
using GimnasioJena.AccesoADatos.Entidades.Comunicacion;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Contacto
{
    public class ContactoAD : IContactoAD
    {
        public async Task<ContactoInfoDto> ObtenerInfoContactoAsync()
        {
            using (var contexto = new Contexto())
            {
                var registros = await contexto.ContenidoWeb
                    .Where(x => x.Pagina == "Contacto" && x.Estado)
                    .ToListAsync();

                var dto = new ContactoInfoDto();

                foreach (var registro in registros)
                {
                    switch (registro.Clave)
                    {
                        case "Direccion":
                            dto.Direccion = registro.TextoPrincipal;
                            break;
                        case "Telefono":
                            dto.Telefono = registro.TextoPrincipal;
                            break;
                        case "Correo":
                            dto.Correo = registro.TextoPrincipal;
                            break;
                    }
                }

                return dto;
            }
        }

        public async Task<string> ObtenerEmailAdminAsync()
        {
            using (var contexto = new Contexto())
            {
                var emails = await contexto.Database
                    .SqlQuery<string>(
                        "SELECT TOP 1 u.Email " +
                        "FROM AspNetUsers u " +
                        "INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId " +
                        "INNER JOIN AspNetRoles r ON ur.RoleId = r.Id " +
                        "WHERE r.Name = 'ADMINISTRADOR' AND u.Email IS NOT NULL")
                    .ToListAsync();

                return emails.FirstOrDefault();
            }
        }

        public async Task RegistrarMensajeAsync(string nombre, string correo, string telefono, string asunto, string mensaje)
        {
            using (var contexto = new Contexto())
            {
                var entidad = new MensajeEntidad
                {
                    nombre     = nombre,
                    correo     = correo,
                    telefono   = telefono,
                    asunto     = asunto,
                    mensaje    = mensaje,
                    estado     = "Pendiente",
                    fechaEnvio = DateTime.Now
                };

                contexto.Mensajes.Add(entidad);
                await contexto.SaveChangesAsync();
            }
        }
    }
}
