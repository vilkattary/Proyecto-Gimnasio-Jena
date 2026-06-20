using GimnasioJena.Abstracciones.AccesoADatos.Usuarios.CambiarRolUsuario;
using GimnasioJena.Abstracciones.Modelos.Usuarios;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Usuarios.CambiarRolUsuario
{
    public class CambiarRolUsuarioAD : ICambiarRolUsuarioAD
    {
        private readonly Contexto _contexto;

        public CambiarRolUsuarioAD()
        {
            _contexto = new Contexto();
        }

        public bool CambiarRolUsuario(UsuarioCambiarRolDto usuarioCambiarRol)
        {
            using (var transaccion = _contexto.Database.BeginTransaction())
            {
                try
                {
                    string validarRol = @"
                        SELECT COUNT(1)
                        FROM AspNetRoles
                        WHERE Name = @rolNuevo;
                    ";

                    int existeRol = _contexto.Database.SqlQuery<int>(
                        validarRol,
                        new SqlParameter("@rolNuevo", usuarioCambiarRol.rolNuevo)
                    ).FirstOrDefault();

                    if (existeRol == 0)
                    {
                        return false;
                    }

                    string eliminarRolesActuales = @"
                        DELETE FROM AspNetUserRoles
                        WHERE UserId = @identityUserId;
                    ";

                    _contexto.Database.ExecuteSqlCommand(
                        eliminarRolesActuales,
                        new SqlParameter("@identityUserId", usuarioCambiarRol.identityUserId)
                    );

                    string insertarNuevoRol = @"
                        INSERT INTO AspNetUserRoles (UserId, RoleId)
                        SELECT @identityUserId, Id
                        FROM AspNetRoles
                        WHERE Name = @rolNuevo;
                    ";

                    _contexto.Database.ExecuteSqlCommand(
                        insertarNuevoRol,
                        new SqlParameter("@identityUserId", usuarioCambiarRol.identityUserId),
                        new SqlParameter("@rolNuevo", usuarioCambiarRol.rolNuevo)
                    );

                    if (usuarioCambiarRol.rolNuevo == "ENTRENADOR")
                    {
                        string insertarEntrenadorSiNoExiste = @"
                            IF NOT EXISTS (
                                SELECT 1
                                FROM Entrenador
                                WHERE idUsuario = @idUsuario
                            )
                            BEGIN
                                INSERT INTO Entrenador
                                (
                                    idUsuario,
                                    especialidad,
                                    descripcion,
                                    fechaContratacion,
                                    estado
                                )
                                VALUES
                                (
                                    @idUsuario,
                                    'Pendiente de completar',
                                    'Pendiente de completar',
                                    GETDATE(),
                                    1
                                );
                            END;
                        ";

                        _contexto.Database.ExecuteSqlCommand(
                            insertarEntrenadorSiNoExiste,
                            new SqlParameter("@idUsuario", usuarioCambiarRol.idUsuario)
                        );
                    }

                    string insertarBitacora = @"
                        INSERT INTO Bitacora
                        (
                            idUsuario,
                            tablaAfectada,
                            accionRealizada,
                            idRegistroAfectado,
                            fechaAccion,
                            detalle,
                            ipUsuario
                        )
                        VALUES
                        (
                            @idUsuarioAdministrador,
                            'AspNetUserRoles',
                            'UPDATE',
                            @idUsuarioAfectado,
                            SYSUTCDATETIME(),
                            @detalle,
                            NULL
                        );
                    ";

                    string detalle = "Cambio de rol. Rol anterior: "
                                     + (usuarioCambiarRol.rolActual ?? "SIN ROL")
                                     + ". Rol nuevo: "
                                     + usuarioCambiarRol.rolNuevo;

                    _contexto.Database.ExecuteSqlCommand(
                        insertarBitacora,
                        new SqlParameter("@idUsuarioAdministrador", (object)usuarioCambiarRol.idUsuarioAdministrador ?? DBNull.Value),
                        new SqlParameter("@idUsuarioAfectado", usuarioCambiarRol.idUsuario),
                        new SqlParameter("@detalle", detalle)
                    );

                    transaccion.Commit();
                    return true;
                }
                catch
                {
                    transaccion.Rollback();
                    return false;
                }
            }
        }
    }
}