-- ============================================================================
-- SCRIPT DE LIMPIEZA TOTAL Y SEED COMPLETO - Gimnasio Jena
-- ============================================================================
-- Contraseña para TODOS los usuarios: Password!123
-- Hash generado con el algoritmo real de ASP.NET Identity v2
-- (PBKDF2-HMAC-SHA1, 1000 iteraciones, 16 bytes salt, 32 bytes subkey)
-- ============================================================================

USE [GimnasioJena];
GO

BEGIN TRANSACTION;

BEGIN TRY

    -- ============================================================
    -- FASE 1: LIMPIEZA TOTAL (orden inverso de dependencias FK)
    -- ============================================================
    PRINT '=== FASE 1: Limpiando datos existentes... ===';

    -- Nivel 1: Tablas hoja (sin hijos)
    DELETE FROM [dbo].[Pago];
    DELETE FROM [dbo].[Asistencia];
    DELETE FROM [dbo].[ProgresoCliente];
    DELETE FROM [dbo].[RegistroMedicion];
    DELETE FROM [dbo].[Mensaje];
    DELETE FROM [dbo].[Bitacora];
    DELETE FROM [dbo].[AspNetUserRoles];
    DELETE FROM [dbo].[AspNetUserClaims];
    DELETE FROM [dbo].[AspNetUserLogins];

    -- Nivel 2: Dependientes de clases y membresías
    DELETE FROM [dbo].[Reserva];
    DELETE FROM [dbo].[MembresiaCliente];

    -- Nivel 3: Clases programadas
    DELETE FROM [dbo].[ClaseProgramada];

    -- Nivel 4: Horarios y Entrenadores
    DELETE FROM [dbo].[HorarioSemanal];
    DELETE FROM [dbo].[Entrenador];

    -- Nivel 5: Progreso (catálogos)
    DELETE FROM [dbo].[Entrenamiento];
    DELETE FROM [dbo].[CampoMedicion];

    -- Nivel 6: Usuarios del sistema
    DELETE FROM [dbo].[Usuario];

    -- Nivel 7: Cuentas de autenticación
    DELETE FROM [dbo].[AspNetUsers];

    -- Resetear IDENTITY
    DBCC CHECKIDENT ('[dbo].[Usuario]', RESEED, 2000);
    DBCC CHECKIDENT ('[dbo].[Entrenador]', RESEED, 0);
    DBCC CHECKIDENT ('[dbo].[ClaseProgramada]', RESEED, 0);
    DBCC CHECKIDENT ('[dbo].[Reserva]', RESEED, 0);
    DBCC CHECKIDENT ('[dbo].[MembresiaCliente]', RESEED, 0);
    DBCC CHECKIDENT ('[dbo].[Pago]', RESEED, 0);
    DBCC CHECKIDENT ('[dbo].[Asistencia]', RESEED, 0);
    DBCC CHECKIDENT ('[dbo].[Entrenamiento]', RESEED, 0);
    DBCC CHECKIDENT ('[dbo].[CampoMedicion]', RESEED, 0);
    DBCC CHECKIDENT ('[dbo].[ProgresoCliente]', RESEED, 0);
    DBCC CHECKIDENT ('[dbo].[RegistroMedicion]', RESEED, 0);
    DBCC CHECKIDENT ('[dbo].[Mensaje]', RESEED, 0);
    DBCC CHECKIDENT ('[dbo].[Bitacora]', RESEED, 0);
    DBCC CHECKIDENT ('[dbo].[HorarioSemanal]', RESEED, 0);

    PRINT 'Limpieza completada.';

    -- ============================================================
    -- FASE 2: VARIABLES Y HASH DE CONTRASEÑA VÁLIDO
    -- ============================================================
    PRINT '=== FASE 2: Preparando datos... ===';

    -- Hash REAL generado con ASP.NET Identity v2
    -- Contraseña: Password!123
    DECLARE @PasswordHash NVARCHAR(MAX) = N'AErbCb7hUlY383n4yDwal/wbHiHz+OXHS9dtTW2MtQtu5eQ7EfL8/bnq0YrRmidBkQ==';

    -- GUIDs para las cuentas de Identity
    DECLARE @IdAdminGuid      NVARCHAR(128) = LOWER(NEWID());
    DECLARE @IdClienteGuid    NVARCHAR(128) = LOWER(NEWID());
    DECLARE @IdEntrenadorGuid NVARCHAR(128) = LOWER(NEWID());
    DECLARE @IdCliente2Guid   NVARCHAR(128) = LOWER(NEWID());

    -- IDs de roles existentes
    DECLARE @RolAdminId      NVARCHAR(128);
    DECLARE @RolClienteId    NVARCHAR(128);
    DECLARE @RolEntrenadorId NVARCHAR(128);

    SELECT TOP 1 @RolAdminId      = [Id] FROM [dbo].[AspNetRoles] WHERE UPPER([Name]) = 'ADMINISTRADOR';
    SELECT TOP 1 @RolClienteId    = [Id] FROM [dbo].[AspNetRoles] WHERE UPPER([Name]) = 'CLIENTE';
    SELECT TOP 1 @RolEntrenadorId = [Id] FROM [dbo].[AspNetRoles] WHERE UPPER([Name]) = 'ENTRENADOR';

    IF @RolClienteId IS NULL OR @RolEntrenadorId IS NULL
    BEGIN
        RAISERROR('ERROR: Los roles CLIENTE y/o ENTRENADOR no existen en AspNetRoles. Créalos primero.', 16, 1);
    END

    -- ============================================================
    -- FASE 3: CREACIÓN DE CUENTAS ASP.NET IDENTITY
    -- ============================================================
    PRINT '=== FASE 3: Creando cuentas de Identity... ===';

    INSERT INTO [dbo].[AspNetUsers]
        ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp],
         [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled],
         [LockoutEnabled], [AccessFailedCount], [UserName])
    VALUES
        -- Admin: bmdiana
        (@IdAdminGuid, N'bmdiana.1509@gmail.com', 1, @PasswordHash, LOWER(NEWID()),
         N'88667915', 1, 0, 1, 0, N'bmdiana.1509@gmail.com'),
        -- Cliente: Diana
        (@IdClienteGuid, N'diana.brenes1509@gmail.com', 1, @PasswordHash, LOWER(NEWID()),
         N'88667915', 1, 0, 1, 0, N'diana.brenes1509@gmail.com'),
        -- Entrenador: Vil
        (@IdEntrenadorGuid, N'vilkattary@gmail.com', 1, @PasswordHash, LOWER(NEWID()),
         N'87056805', 1, 0, 1, 0, N'vilkattary@gmail.com'),
        -- Cliente 2: diabrepers
        (@IdCliente2Guid, N'diabrepers1@gmail.com', 1, @PasswordHash, LOWER(NEWID()),
         N'88667915', 1, 0, 1, 0, N'diabrepers1@gmail.com');

    -- Asignar roles
    IF @RolAdminId IS NOT NULL
        INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (@IdAdminGuid, @RolAdminId);

    INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (@IdClienteGuid, @RolClienteId);
    INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (@IdEntrenadorGuid, @RolEntrenadorId);
    INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (@IdCliente2Guid, @RolClienteId);

    PRINT 'Cuentas de Identity creadas.';

    -- ============================================================
    -- FASE 4: CREACIÓN EN TABLA USUARIO (perfil de negocio)
    -- ============================================================
    PRINT '=== FASE 4: Creando perfiles de usuario... ===';

    DECLARE @IdUsuarioAdmin INT;
    DECLARE @IdUsuarioCliente INT;
    DECLARE @IdUsuarioEntrenador INT;
    DECLARE @IdUsuarioCliente2 INT;

    -- Admin
    INSERT INTO [dbo].[Usuario]
        ([identityUserId], [nombre], [apellido1], [apellido2], [identificacion],
         [correo], [telefono], [fechaRegistro], [estado])
    VALUES
        (@IdAdminGuid, N'Diana', N'Brenes', N'Muñoz', N'118220812',
         N'bmdiana.1509@gmail.com', N'88667915', SYSUTCDATETIME(), 1);
    SET @IdUsuarioAdmin = SCOPE_IDENTITY();

    -- Cliente Diana
    INSERT INTO [dbo].[Usuario]
        ([identityUserId], [nombre], [apellido1], [apellido2], [identificacion],
         [correo], [telefono], [fechaRegistro], [estado])
    VALUES
        (@IdClienteGuid, N'Diana', N'Brenes', N'P', N'111111111',
         N'diana.brenes1509@gmail.com', N'88667915', SYSUTCDATETIME(), 1);
    SET @IdUsuarioCliente = SCOPE_IDENTITY();

    -- Entrenador Vil
    INSERT INTO [dbo].[Usuario]
        ([identityUserId], [nombre], [apellido1], [apellido2], [identificacion],
         [correo], [telefono], [fechaRegistro], [estado])
    VALUES
        (@IdEntrenadorGuid, N'Vil', N'Kattary', N'E', N'222222222',
         N'vilkattary@gmail.com', N'87056805', SYSUTCDATETIME(), 1);
    SET @IdUsuarioEntrenador = SCOPE_IDENTITY();

    -- Cliente 2
    INSERT INTO [dbo].[Usuario]
        ([identityUserId], [nombre], [apellido1], [apellido2], [identificacion],
         [correo], [telefono], [fechaRegistro], [estado])
    VALUES
        (@IdCliente2Guid, N'Diana', N'Brenes', NULL, N'333333333',
         N'diabrepers1@gmail.com', N'88667915', SYSUTCDATETIME(), 1);
    SET @IdUsuarioCliente2 = SCOPE_IDENTITY();

    PRINT 'Perfiles de usuario creados.';

    -- ============================================================
    -- FASE 5: REGISTRAR ENTRENADOR
    -- ============================================================
    PRINT '=== FASE 5: Registrando entrenador... ===';

    INSERT INTO [dbo].[Entrenador]
        ([idUsuario], [especialidad], [descripcion], [fechaContratacion], [estado])
    VALUES
        (@IdUsuarioEntrenador, N'Fuerza y Acondicionamiento',
         N'Entrenador especializado asignado para pruebas completas.',
         CAST(GETDATE() AS DATE), 1);

    PRINT 'Entrenador registrado.';

    -- ============================================================
    -- FASE 6: MEMBRESÍA ACTIVA PARA EL CLIENTE
    -- ============================================================
    PRINT '=== FASE 6: Creando membresías... ===';

    INSERT INTO [dbo].[MembresiaCliente]
        ([idUsuario], [idPlanMembresia], [idEstadoMembresia],
         [fechaInicio], [fechaFin], [clasesDisponibles], [observaciones], [fechaCreacion])
    VALUES
        (@IdUsuarioCliente, 1, 1,
         DATEADD(DAY, -15, CAST(GETDATE() AS DATE)),
         DATEADD(DAY, 15, CAST(GETDATE() AS DATE)),
         25, N'Membresía activa de prueba.', SYSUTCDATETIME());

    PRINT 'Membresías creadas.';

    -- ============================================================
    -- FASE 7: CLASES PROGRAMADAS Y RESERVAS
    -- ============================================================
    PRINT '=== FASE 7: Creando clases, reservas... ===';

    DECLARE @IdClase1 INT, @IdClase2 INT;

    INSERT INTO [dbo].[ClaseProgramada]
        ([idTipoClase], [idUsuarioEntrenador], [idEstadoClase],
         [fechaClase], [horaInicio], [horaFin], [cupoMaximo],
         [ubicacion], [observaciones], [fechaCreacion])
    VALUES
        (4, @IdUsuarioEntrenador, 3,
         CAST(DATEADD(DAY, -10, GETDATE()) AS DATE), '08:00', '09:00', 15,
         N'Sala Principal', N'Prueba de asistencia a clase de Fuerza', SYSUTCDATETIME());
    SET @IdClase1 = SCOPE_IDENTITY();

    INSERT INTO [dbo].[ClaseProgramada]
        ([idTipoClase], [idUsuarioEntrenador], [idEstadoClase],
         [fechaClase], [horaInicio], [horaFin], [cupoMaximo],
         [ubicacion], [observaciones], [fechaCreacion])
    VALUES
        (1, @IdUsuarioEntrenador, 3,
         CAST(DATEADD(DAY, -3, GETDATE()) AS DATE), '18:00', '19:00', 20,
         N'Sala Funcional', N'Prueba de asistencia a clase de Acondicionamiento', SYSUTCDATETIME());
    SET @IdClase2 = SCOPE_IDENTITY();

    -- Reservas: Diana asiste a ambas clases (idEstadoReserva = 3 -> Asistió)
    INSERT INTO [dbo].[Reserva]
        ([idUsuario], [idClaseProgramada], [idEstadoReserva], [fechaReserva], [observaciones])
    VALUES
        (@IdUsuarioCliente, @IdClase1, 3,
         DATEADD(DAY, -12, SYSUTCDATETIME()), N'Reserva probada con asistencia confirmada.');

    INSERT INTO [dbo].[Reserva]
        ([idUsuario], [idClaseProgramada], [idEstadoReserva], [fechaReserva], [observaciones])
    VALUES
        (@IdUsuarioCliente, @IdClase2, 3,
         DATEADD(DAY, -5, SYSUTCDATETIME()), N'Reserva probada con asistencia confirmada.');

    PRINT 'Clases y reservas creadas.';

    -- ============================================================
    -- FASE 8: ENTRENAMIENTOS Y PROGRESO (RUTINAS)
    -- ============================================================
    PRINT '=== FASE 8: Creando entrenamientos y progreso... ===';

    DECLARE @IdRutina1 INT, @IdRutina2 INT;

    INSERT INTO [dbo].[Entrenamiento] ([DiaSemana], [NombreRutina], [EjerciciosDetalle])
    VALUES (N'Lunes', N'Día de Pierna (Fuerza)', N'Sentadillas 4x10, Prensa 4x12, Desplantes 3x15');
    SET @IdRutina1 = SCOPE_IDENTITY();

    INSERT INTO [dbo].[Entrenamiento] ([DiaSemana], [NombreRutina], [EjerciciosDetalle])
    VALUES (N'Miércoles', N'Acondicionamiento Full Body', N'Burpees 3x15, Kettlebell Swings 4x20, Box Jumps 3x10');
    SET @IdRutina2 = SCOPE_IDENTITY();

    INSERT INTO [dbo].[ProgresoCliente]
        ([idUsuario], [IdEntrenamiento], [PesoAlcanzado], [Repeticiones], [Notas], [FechaRegistro])
    VALUES
        (@IdUsuarioCliente, @IdRutina1, 45.00, 10,
         N'Vil me corrigió la postura en las sentadillas, me sentí mucho mejor.',
         DATEADD(DAY, -10, SYSUTCDATETIME())),
        (@IdUsuarioCliente, @IdRutina2, 12.00, 20,
         N'Terminé exhausta pero completé los swings.',
         DATEADD(DAY, -3, SYSUTCDATETIME()));

    PRINT 'Entrenamientos y progreso creados.';

    -- ============================================================
    -- FASE 9: CAMPOS DE MEDICIÓN Y REGISTROS (EVOLUCIÓN)
    -- ============================================================
    PRINT '=== FASE 9: Creando datos de mediciones... ===';

    DECLARE @IdCampoPeso INT, @IdCampoGrasa INT, @IdCampoMusculo INT;

    INSERT INTO [dbo].[CampoMedicion] ([NombreCampo], [Activo]) VALUES (N'Peso Corporal (kg)', 1);
    SET @IdCampoPeso = SCOPE_IDENTITY();

    INSERT INTO [dbo].[CampoMedicion] ([NombreCampo], [Activo]) VALUES (N'Porcentaje de Grasa (%)', 1);
    SET @IdCampoGrasa = SCOPE_IDENTITY();

    INSERT INTO [dbo].[CampoMedicion] ([NombreCampo], [Activo]) VALUES (N'Porcentaje de Músculo (%)', 1);
    SET @IdCampoMusculo = SCOPE_IDENTITY();

    INSERT INTO [dbo].[RegistroMedicion] ([idUsuario], [IdCampo], [Valor], [FechaRegistro])
    VALUES
        -- Hace 2 meses (Inicio)
        (@IdUsuarioCliente, @IdCampoPeso,    69.50, DATEADD(MONTH, -2, SYSUTCDATETIME())),
        (@IdUsuarioCliente, @IdCampoGrasa,   32.50, DATEADD(MONTH, -2, SYSUTCDATETIME())),
        (@IdUsuarioCliente, @IdCampoMusculo, 31.00, DATEADD(MONTH, -2, SYSUTCDATETIME())),
        -- Hace 1 mes
        (@IdUsuarioCliente, @IdCampoPeso,    67.20, DATEADD(MONTH, -1, SYSUTCDATETIME())),
        (@IdUsuarioCliente, @IdCampoGrasa,   30.10, DATEADD(MONTH, -1, SYSUTCDATETIME())),
        (@IdUsuarioCliente, @IdCampoMusculo, 32.80, DATEADD(MONTH, -1, SYSUTCDATETIME())),
        -- Actualidad
        (@IdUsuarioCliente, @IdCampoPeso,    65.30, SYSUTCDATETIME()),
        (@IdUsuarioCliente, @IdCampoGrasa,   27.80, SYSUTCDATETIME()),
        (@IdUsuarioCliente, @IdCampoMusculo, 34.50, SYSUTCDATETIME());

    PRINT 'Mediciones creadas.';

    -- ============================================================
    -- COMMIT
    -- ============================================================
    COMMIT TRANSACTION;

    PRINT '';
    PRINT '=====================================================';
    PRINT '  SEED COMPLETADO EXITOSAMENTE';
    PRINT '=====================================================';
    PRINT '';
    PRINT '  Usuarios creados:';
    PRINT '  +---------------------------------+----------------+';
    PRINT '  | Email                           | Rol            |';
    PRINT '  +---------------------------------+----------------+';
    PRINT '  | bmdiana.1509@gmail.com          | ADMINISTRADOR  |';
    PRINT '  | diana.brenes1509@gmail.com      | CLIENTE        |';
    PRINT '  | vilkattary@gmail.com            | ENTRENADOR     |';
    PRINT '  | diabrepers1@gmail.com           | CLIENTE        |';
    PRINT '  +---------------------------------+----------------+';
    PRINT '';
    PRINT '  Contrasena para TODOS: Password!123';
    PRINT '';
    PRINT '=====================================================';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    PRINT '';
    PRINT '!!! ERROR - Se revirtieron TODOS los cambios !!!';
    PRINT 'Mensaje: ' + ERROR_MESSAGE();
    PRINT 'Linea:   ' + CAST(ERROR_LINE() AS NVARCHAR(10));
    PRINT '';

    THROW;
END CATCH;
GO
