using GimnasioJena.Abstracciones.AccesoADatos.Usuarios.ObtenerTodosLosUsuarios;
using GimnasioJena.Abstracciones.Modelos.Usuarios;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Usuarios.ObtenerTodosLosUsuarios
{
    public class ObtenerTodosLosUsuariosAD : IObtenerTodosLosUsuariosAD
    {
        private readonly Contexto _contexto;

        public ObtenerTodosLosUsuariosAD()
        {
            _contexto = new Contexto();
        }

        public List<UsuarioListadoDto> ObtenerTodosLosUsuarios()
        {
            string consulta = @"
                SELECT 
                    ISNULL(u.idUsuario, 0) AS idUsuario,
                    au.Id AS identityUserId,
                    LTRIM(RTRIM(ISNULL(u.nombre, '') + ' ' + ISNULL(u.apellido1, '') + ' ' + ISNULL(u.apellido2, ''))) AS nombreCompleto,
                    ISNULL(u.identificacion, '') AS identificacion,
                    ISNULL(au.Email, '') AS correo,
                    ISNULL(u.telefono, '') AS telefono,
                    ISNULL(ar.Name, 'SIN ROL') AS rol,
                    ISNULL(u.fechaRegistro, GETDATE()) AS fechaRegistro,
                    ISNULL(u.estado, 1) AS estado
                FROM AspNetUsers au
                LEFT JOIN Usuario u 
                    ON u.identityUserId = au.Id
                LEFT JOIN AspNetUserRoles aur 
                    ON aur.UserId = au.Id
                LEFT JOIN AspNetRoles ar 
                    ON ar.Id = aur.RoleId
                ORDER BY au.Email;
            ";

            return _contexto.Database.SqlQuery<UsuarioListadoDto>(consulta).ToList();
        }
    }
}