-- ============================================================================
-- SCRIPT COMPLEMENTARIO: DATOS HISTÓRICOS DE PRUEBA
-- Para: diana.brenes1509@gmail.com (CLIENTE)
-- Período: Noviembre 2025 → Septiembre 2026 (hoy)
-- ============================================================================
-- REQUISITO: Ejecutar DESPUÉS de Script_Limpieza_Y_Seed_Completo.sql
-- ============================================================================

USE [GimnasioJena];
GO

BEGIN TRANSACTION;

BEGIN TRY

    -- ============================================================
    -- PASO 0: OBTENER IDs EXISTENTES
    -- ============================================================
    DECLARE @IdUsuarioCliente INT;
    DECLARE @IdUsuarioEntrenador INT;

    SELECT @IdUsuarioCliente = idUsuario
    FROM [dbo].[Usuario]
    WHERE correo = N'diana.brenes1509@gmail.com';

    SELECT @IdUsuarioEntrenador = idUsuario
    FROM [dbo].[Usuario]
    WHERE correo = N'vilkattary@gmail.com';

    IF @IdUsuarioCliente IS NULL OR @IdUsuarioEntrenador IS NULL
    BEGIN
        RAISERROR('ERROR: No se encontraron los usuarios diana.brenes1509@gmail.com y/o vilkattary@gmail.com. Ejecuta primero Script_Limpieza_Y_Seed_Completo.sql', 16, 1);
    END

    PRINT 'Usuario cliente ID: ' + CAST(@IdUsuarioCliente AS NVARCHAR(10));
    PRINT 'Usuario entrenador ID: ' + CAST(@IdUsuarioEntrenador AS NVARCHAR(10));

    -- ============================================================
    -- PASO 1: MEMBRESÍAS HISTÓRICAS (Nov 2025 → Sep 2026)
    -- ============================================================
    -- Eliminar la membresía de prueba del seed base para reemplazarla
    -- con el historial completo
    DELETE FROM [dbo].[Pago] WHERE [idMembresiaCliente] IN (
        SELECT idMembresiaCliente FROM [dbo].[MembresiaCliente]
        WHERE idUsuario = @IdUsuarioCliente
    );
    DELETE FROM [dbo].[MembresiaCliente] WHERE idUsuario = @IdUsuarioCliente;

    PRINT '=== Creando membresías históricas... ===';

    -- idPlanMembresia = 1, idEstadoMembresia = 1 (Activa para la vigente)
    -- Las vencidas se marcan con estado 1 pero fechaFin < hoy
    -- (la app detecta "Vencida" comparando fechaFin < GETDATE())

    DECLARE @IdMemb1 INT, @IdMemb2 INT, @IdMemb3 INT, @IdMemb4 INT, @IdMemb5 INT, @IdMemb6 INT;

    -- Membresía 1: Nov 2025 - Dic 2025
    INSERT INTO [dbo].[MembresiaCliente]
        ([idUsuario],[idPlanMembresia],[idEstadoMembresia],[fechaInicio],[fechaFin],[clasesDisponibles],[observaciones],[fechaCreacion])
    VALUES (@IdUsuarioCliente, 1, 1, '2025-11-01', '2025-12-01', 0, N'Primera membresía - Inicio del viaje fitness.', '2025-11-01T08:00:00');
    SET @IdMemb1 = SCOPE_IDENTITY();

    -- Membresía 2: Dic 2025 - Ene 2026
    INSERT INTO [dbo].[MembresiaCliente]
        ([idUsuario],[idPlanMembresia],[idEstadoMembresia],[fechaInicio],[fechaFin],[clasesDisponibles],[observaciones],[fechaCreacion])
    VALUES (@IdUsuarioCliente, 1, 1, '2025-12-01', '2026-01-01', 0, N'Renovación diciembre.', '2025-12-01T08:00:00');
    SET @IdMemb2 = SCOPE_IDENTITY();

    -- Membresía 3: Ene 2026 - Mar 2026
    INSERT INTO [dbo].[MembresiaCliente]
        ([idUsuario],[idPlanMembresia],[idEstadoMembresia],[fechaInicio],[fechaFin],[clasesDisponibles],[observaciones],[fechaCreacion])
    VALUES (@IdUsuarioCliente, 1, 1, '2026-01-01', '2026-03-01', 0, N'Plan extendido de 2 meses.', '2026-01-01T08:00:00');
    SET @IdMemb3 = SCOPE_IDENTITY();

    -- Membresía 4: Mar 2026 - May 2026
    INSERT INTO [dbo].[MembresiaCliente]
        ([idUsuario],[idPlanMembresia],[idEstadoMembresia],[fechaInicio],[fechaFin],[clasesDisponibles],[observaciones],[fechaCreacion])
    VALUES (@IdUsuarioCliente, 1, 1, '2026-03-01', '2026-05-01', 0, N'Renovación marzo.', '2026-03-01T08:00:00');
    SET @IdMemb4 = SCOPE_IDENTITY();

    -- Membresía 5: May 2026 - Jul 2026
    INSERT INTO [dbo].[MembresiaCliente]
        ([idUsuario],[idPlanMembresia],[idEstadoMembresia],[fechaInicio],[fechaFin],[clasesDisponibles],[observaciones],[fechaCreacion])
    VALUES (@IdUsuarioCliente, 1, 1, '2026-05-01', '2026-07-01', 0, N'Renovación mayo.', '2026-05-01T08:00:00');
    SET @IdMemb5 = SCOPE_IDENTITY();

    -- Membresía 6: Jul 2026 - Oct 2026 (ACTIVA actual)
    INSERT INTO [dbo].[MembresiaCliente]
        ([idUsuario],[idPlanMembresia],[idEstadoMembresia],[fechaInicio],[fechaFin],[clasesDisponibles],[observaciones],[fechaCreacion])
    VALUES (@IdUsuarioCliente, 1, 1, '2026-07-01', '2026-10-01', 20, N'Membresía vigente actual.', '2026-07-01T08:00:00');
    SET @IdMemb6 = SCOPE_IDENTITY();

    PRINT 'Membresías creadas: 6 períodos (Nov 2025 → Oct 2026).';

    -- ============================================================
    -- PASO 2: PAGOS POR CADA MEMBRESÍA
    -- ============================================================
    PRINT '=== Creando pagos... ===';

    -- idEstadoPago = 2 (Pagado), idMetodoPago = 1
    INSERT INTO [dbo].[Pago]
        ([idMembresiaCliente],[idMetodoPago],[idEstadoPago],[monto],[fechaPago],[referenciaPago],[observaciones])
    VALUES
        (@IdMemb1, 1, 2, 25000.00, '2025-11-01T09:00:00', N'PAG-2025-001', N'Pago membresía noviembre 2025'),
        (@IdMemb2, 1, 2, 25000.00, '2025-12-01T09:00:00', N'PAG-2025-002', N'Pago membresía diciembre 2025'),
        (@IdMemb3, 1, 2, 45000.00, '2026-01-02T10:00:00', N'PAG-2026-001', N'Pago membresía ene-feb 2026'),
        (@IdMemb4, 1, 2, 45000.00, '2026-03-01T09:30:00', N'PAG-2026-002', N'Pago membresía mar-abr 2026'),
        (@IdMemb5, 1, 2, 45000.00, '2026-05-01T08:45:00', N'PAG-2026-003', N'Pago membresía may-jun 2026'),
        (@IdMemb6, 1, 2, 60000.00, '2026-07-01T09:00:00', N'PAG-2026-004', N'Pago membresía jul-sep 2026');

    PRINT 'Pagos creados: 6 pagos históricos.';

    -- ============================================================
    -- PASO 3: ENTRENAMIENTOS / RUTINAS (catálogo ampliado)
    -- ============================================================
    PRINT '=== Creando rutinas de entrenamiento... ===';

    -- Eliminar los del seed base para tener un catálogo completo
    DELETE FROM [dbo].[ProgresoCliente] WHERE idUsuario = @IdUsuarioCliente;
    DELETE FROM [dbo].[Entrenamiento];

    DECLARE @RutLunes INT, @RutMartes INT, @RutMiercoles INT, @RutJueves INT, @RutViernes INT, @RutSabado INT;

    INSERT INTO [dbo].[Entrenamiento] ([DiaSemana],[NombreRutina],[EjerciciosDetalle])
    VALUES (N'Lunes', N'Pierna y Glúteos', N'Sentadillas 4x12, Hip Thrust 4x10, Prensa 3x15, Desplantes búlgaros 3x12, Extensión cuádriceps 3x15');
    SET @RutLunes = SCOPE_IDENTITY();

    INSERT INTO [dbo].[Entrenamiento] ([DiaSemana],[NombreRutina],[EjerciciosDetalle])
    VALUES (N'Martes', N'Espalda y Bíceps', N'Remo con barra 4x10, Jalón al pecho 4x12, Remo con mancuerna 3x12, Curl bíceps 3x15, Face pulls 3x15');
    SET @RutMartes = SCOPE_IDENTITY();

    INSERT INTO [dbo].[Entrenamiento] ([DiaSemana],[NombreRutina],[EjerciciosDetalle])
    VALUES (N'Miércoles', N'Full Body Funcional', N'Burpees 3x15, Kettlebell Swings 4x20, Box Jumps 3x10, TRX Rows 3x12, Plancha 3x45s');
    SET @RutMiercoles = SCOPE_IDENTITY();

    INSERT INTO [dbo].[Entrenamiento] ([DiaSemana],[NombreRutina],[EjerciciosDetalle])
    VALUES (N'Jueves', N'Pecho y Tríceps', N'Press banca 4x10, Press inclinado mancuernas 4x12, Aperturas 3x15, Fondos 3x12, Extensión tríceps 3x15');
    SET @RutJueves = SCOPE_IDENTITY();

    INSERT INTO [dbo].[Entrenamiento] ([DiaSemana],[NombreRutina],[EjerciciosDetalle])
    VALUES (N'Viernes', N'Pierna Fuerza', N'Sentadilla profunda 5x5, Peso muerto rumano 4x8, Prensa 4x12, Curl femoral 3x15, Elevación de talones 4x20');
    SET @RutViernes = SCOPE_IDENTITY();

    INSERT INTO [dbo].[Entrenamiento] ([DiaSemana],[NombreRutina],[EjerciciosDetalle])
    VALUES (N'Sábado', N'HIIT + Core', N'Mountain climbers 4x30s, Jumping lunges 3x20, Burpees 3x12, Russian twist 3x20, Plancha lateral 3x30s, Crunch bicicleta 3x20');
    SET @RutSabado = SCOPE_IDENTITY();

    PRINT 'Rutinas creadas: 6 rutinas semanales.';

    -- ============================================================
    -- PASO 4: CLASES PROGRAMADAS (Nov 2025 → Sep 2026)
    -- ============================================================
    -- Generar ~2 clases por semana durante ~46 semanas
    -- Usando bucle para no escribir 90+ inserts manuales
    -- ============================================================
    PRINT '=== Generando clases programadas históricas... ===';

    -- Eliminar las clases del seed base
    DELETE FROM [dbo].[Asistencia];
    DELETE FROM [dbo].[Reserva] WHERE idUsuario = @IdUsuarioCliente;
    DELETE FROM [dbo].[ClaseProgramada] WHERE idUsuarioEntrenador = @IdUsuarioEntrenador;

    -- Tabla temporal para almacenar IDs de clases generadas
    CREATE TABLE #ClasesGeneradas (
        idClase INT,
        fechaClase DATE,
        idTipoClase INT,
        diaSemana NVARCHAR(20)
    );

    DECLARE @FechaInicio DATE = '2025-11-03'; -- Primer lunes de noviembre 2025
    DECLARE @FechaFin DATE = '2026-09-16';    -- Hoy
    DECLARE @FechaActual DATE = @FechaInicio;
    DECLARE @DiaSemana INT;
    DECLARE @IdClaseTemp INT;
    DECLARE @TipoClase INT;
    DECLARE @HoraInicio TIME;
    DECLARE @HoraFin TIME;
    DECLARE @Ubicacion NVARCHAR(100);
    DECLARE @Observacion NVARCHAR(500);
    DECLARE @ContadorClases INT = 0;

    WHILE @FechaActual <= @FechaFin
    BEGIN
        SET @DiaSemana = DATEPART(WEEKDAY, @FechaActual); -- 1=Dom, 2=Lun, 3=Mar, 4=Mie, 5=Jue, 6=Vie, 7=Sab

        -- Lunes: Clase de Fuerza (TipoClase 4), 08:00-09:00
        IF @DiaSemana = 2
        BEGIN
            SET @TipoClase = 4; SET @HoraInicio = '08:00'; SET @HoraFin = '09:00';
            SET @Ubicacion = N'Sala de Pesas';
            SET @Observacion = N'Pierna y Glúteos - Fuerza';

            INSERT INTO [dbo].[ClaseProgramada]
                ([idTipoClase],[idUsuarioEntrenador],[idEstadoClase],[fechaClase],[horaInicio],[horaFin],[cupoMaximo],[ubicacion],[observaciones],[fechaCreacion])
            VALUES (@TipoClase, @IdUsuarioEntrenador, 3, @FechaActual, @HoraInicio, @HoraFin, 15, @Ubicacion, @Observacion, DATEADD(DAY, -2, CAST(@FechaActual AS DATETIME2)));
            SET @IdClaseTemp = SCOPE_IDENTITY();
            INSERT INTO #ClasesGeneradas VALUES (@IdClaseTemp, @FechaActual, @TipoClase, N'Lunes');
            SET @ContadorClases += 1;
        END

        -- Miércoles: Clase Funcional (TipoClase 1), 18:00-19:00
        IF @DiaSemana = 4
        BEGIN
            SET @TipoClase = 1; SET @HoraInicio = '18:00'; SET @HoraFin = '19:00';
            SET @Ubicacion = N'Sala Funcional';
            SET @Observacion = N'Full Body Funcional - Acondicionamiento';

            INSERT INTO [dbo].[ClaseProgramada]
                ([idTipoClase],[idUsuarioEntrenador],[idEstadoClase],[fechaClase],[horaInicio],[horaFin],[cupoMaximo],[ubicacion],[observaciones],[fechaCreacion])
            VALUES (@TipoClase, @IdUsuarioEntrenador, 3, @FechaActual, @HoraInicio, @HoraFin, 20, @Ubicacion, @Observacion, DATEADD(DAY, -2, CAST(@FechaActual AS DATETIME2)));
            SET @IdClaseTemp = SCOPE_IDENTITY();
            INSERT INTO #ClasesGeneradas VALUES (@IdClaseTemp, @FechaActual, @TipoClase, N'Miércoles');
            SET @ContadorClases += 1;
        END

        -- Viernes: Clase de Fuerza (TipoClase 4), 07:00-08:00
        IF @DiaSemana = 6
        BEGIN
            SET @TipoClase = 4; SET @HoraInicio = '07:00'; SET @HoraFin = '08:00';
            SET @Ubicacion = N'Sala de Pesas';
            SET @Observacion = N'Pierna Fuerza - Sentadilla pesada';

            INSERT INTO [dbo].[ClaseProgramada]
                ([idTipoClase],[idUsuarioEntrenador],[idEstadoClase],[fechaClase],[horaInicio],[horaFin],[cupoMaximo],[ubicacion],[observaciones],[fechaCreacion])
            VALUES (@TipoClase, @IdUsuarioEntrenador, 3, @FechaActual, @HoraInicio, @HoraFin, 12, @Ubicacion, @Observacion, DATEADD(DAY, -2, CAST(@FechaActual AS DATETIME2)));
            SET @IdClaseTemp = SCOPE_IDENTITY();
            INSERT INTO #ClasesGeneradas VALUES (@IdClaseTemp, @FechaActual, @TipoClase, N'Viernes');
            SET @ContadorClases += 1;
        END

        -- Sábado (quincenal): HIIT (TipoClase 1), 09:00-10:00
        IF @DiaSemana = 7 AND DATEPART(WEEK, @FechaActual) % 2 = 0
        BEGIN
            SET @TipoClase = 1; SET @HoraInicio = '09:00'; SET @HoraFin = '10:00';
            SET @Ubicacion = N'Sala Funcional';
            SET @Observacion = N'HIIT + Core - Sesión quincenal';

            INSERT INTO [dbo].[ClaseProgramada]
                ([idTipoClase],[idUsuarioEntrenador],[idEstadoClase],[fechaClase],[horaInicio],[horaFin],[cupoMaximo],[ubicacion],[observaciones],[fechaCreacion])
            VALUES (@TipoClase, @IdUsuarioEntrenador, 3, @FechaActual, @HoraInicio, @HoraFin, 20, @Ubicacion, @Observacion, DATEADD(DAY, -2, CAST(@FechaActual AS DATETIME2)));
            SET @IdClaseTemp = SCOPE_IDENTITY();
            INSERT INTO #ClasesGeneradas VALUES (@IdClaseTemp, @FechaActual, @TipoClase, N'Sábado');
            SET @ContadorClases += 1;
        END

        SET @FechaActual = DATEADD(DAY, 1, @FechaActual);
    END

    PRINT 'Clases generadas: ' + CAST(@ContadorClases AS NVARCHAR(10));

    -- ============================================================
    -- PASO 5: RESERVAS (Diana asiste ~85% de las clases)
    -- ============================================================
    PRINT '=== Generando reservas y asistencia... ===';

    -- Diana reserva el 85% de las clases (simulación realista: a veces falta)
    -- Usamos un patrón determinístico: falta cada ~7 clases
    DECLARE @ContadorReservas INT = 0;
    DECLARE @Secuencia INT = 0;

    DECLARE clase_cursor CURSOR FOR
        SELECT idClase, fechaClase FROM #ClasesGeneradas ORDER BY fechaClase;

    DECLARE @CurIdClase INT, @CurFechaClase DATE;

    OPEN clase_cursor;
    FETCH NEXT FROM clase_cursor INTO @CurIdClase, @CurFechaClase;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @Secuencia += 1;

        -- Asiste a todas excepto cada 7ª clase (simulando ausencias realistas)
        IF @Secuencia % 7 != 0
        BEGIN
            INSERT INTO [dbo].[Reserva]
                ([idUsuario],[idClaseProgramada],[idEstadoReserva],[fechaReserva],[observaciones])
            VALUES
                (@IdUsuarioCliente, @CurIdClase, 1,
                 DATEADD(DAY, -1, CAST(@CurFechaClase AS DATETIME2)),
                 N'Reserva automática de prueba');
            SET @ContadorReservas += 1;
        END

        FETCH NEXT FROM clase_cursor INTO @CurIdClase, @CurFechaClase;
    END

    CLOSE clase_cursor;
    DEALLOCATE clase_cursor;

    PRINT 'Reservas creadas: ' + CAST(@ContadorReservas AS NVARCHAR(10));

    -- ============================================================
    -- PASO 6: PROGRESO DEL CLIENTE (registros mensuales)
    -- ============================================================
    PRINT '=== Generando progreso de entrenamiento... ===';

    -- Eliminar el progreso del seed base
    DELETE FROM [dbo].[ProgresoCliente] WHERE idUsuario = @IdUsuarioCliente;

    -- Progreso mensual: Diana va subiendo pesos gradualmente
    -- Rutinas de referencia: @RutLunes (Pierna), @RutMiercoles (Funcional), @RutViernes (Fuerza)

    INSERT INTO [dbo].[ProgresoCliente]
        ([idUsuario],[IdEntrenamiento],[PesoAlcanzado],[Repeticiones],[Notas],[FechaRegistro])
    VALUES
        -- Noviembre 2025 (inicio, pesos bajos)
        (@IdUsuarioCliente, @RutLunes,     20.00, 12, N'Primera vez haciendo sentadillas con barra. Forma básica.', '2025-11-10T08:30:00'),
        (@IdUsuarioCliente, @RutMiercoles,  8.00, 15, N'Kettlebell swings con 8kg. Cansada pero motivada.', '2025-11-12T18:30:00'),
        (@IdUsuarioCliente, @RutViernes,   25.00,  8, N'Sentadilla con barra sola. Aprendiendo la técnica.', '2025-11-14T07:30:00'),

        -- Diciembre 2025
        (@IdUsuarioCliente, @RutLunes,     25.00, 12, N'Subí 5kg en sentadillas. Vil corrigió mi postura.', '2025-12-08T08:30:00'),
        (@IdUsuarioCliente, @RutMiercoles, 10.00, 15, N'Kettlebell 10kg. Mejor cardio.', '2025-12-10T18:30:00'),
        (@IdUsuarioCliente, @RutViernes,   30.00, 8,  N'Peso muerto rumano con 30kg. Sentí bien los femorales.', '2025-12-19T07:30:00'),

        -- Enero 2026
        (@IdUsuarioCliente, @RutLunes,     30.00, 12, N'Hip thrust con 30kg. Glúteos trabajados.', '2026-01-12T08:30:00'),
        (@IdUsuarioCliente, @RutMiercoles, 12.00, 18, N'Más repeticiones con kettlebell 12kg. Resistencia mejoró.', '2026-01-14T18:30:00'),
        (@IdUsuarioCliente, @RutViernes,   35.00, 8,  N'Sentadilla 35kg x8. ¡Nuevo récord personal!', '2026-01-24T07:30:00'),

        -- Febrero 2026
        (@IdUsuarioCliente, @RutLunes,     35.00, 12, N'Sentadillas más profundas. Mejor movilidad.', '2026-02-09T08:30:00'),
        (@IdUsuarioCliente, @RutSabado,    10.00, 15, N'HIIT con Vil. Burpees + box jumps. Exhausta.', '2026-02-14T09:30:00'),
        (@IdUsuarioCliente, @RutViernes,   40.00, 6,  N'Peso muerto 40kg. Vil supervisó la técnica de agarre.', '2026-02-21T07:30:00'),

        -- Marzo 2026
        (@IdUsuarioCliente, @RutLunes,     40.00, 10, N'Prensa 40kg x10. Piernas más fuertes cada mes.', '2026-03-09T08:30:00'),
        (@IdUsuarioCliente, @RutMiercoles, 14.00, 20, N'Kettlebell 14kg x20. Swings explosivos.', '2026-03-11T18:30:00'),
        (@IdUsuarioCliente, @RutViernes,   45.00, 6,  N'Sentadilla 45kg. Siento la fuerza en las piernas.', '2026-03-20T07:30:00'),

        -- Abril 2026
        (@IdUsuarioCliente, @RutLunes,     45.00, 10, N'Hip thrust 45kg. Progreso constante.', '2026-04-06T08:30:00'),
        (@IdUsuarioCliente, @RutSabado,    12.00, 15, N'HIIT más intenso. Russian twist con disco 12kg.', '2026-04-11T09:30:00'),
        (@IdUsuarioCliente, @RutViernes,   50.00, 5,  N'¡SENTADILLA 50KG! Medio plato por lado. Feliz.', '2026-04-24T07:30:00'),

        -- Mayo 2026
        (@IdUsuarioCliente, @RutLunes,     50.00, 8,  N'Prensa 50kg ya es cómoda. Desplantes búlgaros mejorados.', '2026-05-11T08:30:00'),
        (@IdUsuarioCliente, @RutMiercoles, 16.00, 20, N'Kettlebell 16kg. Cardio y fuerza combinados.', '2026-05-13T18:30:00'),
        (@IdUsuarioCliente, @RutViernes,   55.00, 5,  N'Peso muerto 55kg. Agarre firme, espalda recta.', '2026-05-23T07:30:00'),

        -- Junio 2026
        (@IdUsuarioCliente, @RutLunes,     55.00, 8,  N'Sentadillas 55kg x8. Vil dice que mi técnica está excelente.', '2026-06-08T08:30:00'),
        (@IdUsuarioCliente, @RutSabado,    14.00, 15, N'HIIT con ejercicios compuestos. Mejor resistencia.', '2026-06-13T09:30:00'),
        (@IdUsuarioCliente, @RutViernes,   60.00, 5,  N'¡PESO MUERTO 60KG! Récord personal absoluto.', '2026-06-20T07:30:00'),

        -- Julio 2026
        (@IdUsuarioCliente, @RutLunes,     55.00, 10, N'Volumen alto: sentadillas 55kg x10. Enfoque en repeticiones.', '2026-07-07T08:30:00'),
        (@IdUsuarioCliente, @RutMiercoles, 16.00, 22, N'Kettlebell 16kg x22. Resistencia cardiovascular impresionante.', '2026-07-09T18:30:00'),
        (@IdUsuarioCliente, @RutViernes,   60.00, 6,  N'Sentadilla 60kg x6. Consistencia en la fuerza.', '2026-07-25T07:30:00'),

        -- Agosto 2026
        (@IdUsuarioCliente, @RutLunes,     60.00, 8,  N'Hip thrust 60kg x8. Glúteos de acero.', '2026-08-04T08:30:00'),
        (@IdUsuarioCliente, @RutSabado,    16.00, 15, N'HIIT con jumping lunges pesados. Explosividad.', '2026-08-09T09:30:00'),
        (@IdUsuarioCliente, @RutViernes,   65.00, 5,  N'¡SENTADILLA 65KG! Más de la mitad de mi peso corporal.', '2026-08-22T07:30:00'),

        -- Septiembre 2026 (mes actual)
        (@IdUsuarioCliente, @RutLunes,     60.00, 10, N'Semana de deload. Sentadilla 60kg x10 controlada.', '2026-09-01T08:30:00'),
        (@IdUsuarioCliente, @RutMiercoles, 18.00, 20, N'Kettlebell 18kg. Nuevo peso, nueva meta.', '2026-09-03T18:30:00'),
        (@IdUsuarioCliente, @RutViernes,   65.00, 6,  N'Peso muerto 65kg x6. Consistente y fuerte.', '2026-09-12T07:30:00');

    PRINT 'Progreso creado: 33 registros mensuales (Nov 2025 → Sep 2026).';

    -- ============================================================
    -- PASO 7: MEDICIONES CORPORALES (evolución mensual)
    -- ============================================================
    PRINT '=== Generando mediciones corporales mensuales... ===';

    -- Obtener IDs de los CampoMedicion existentes
    DECLARE @CampoPeso INT, @CampoGrasa INT, @CampoMusculo INT;
    SELECT @CampoPeso    = IdCampo FROM [dbo].[CampoMedicion] WHERE NombreCampo = N'Peso Corporal (kg)';
    SELECT @CampoGrasa   = IdCampo FROM [dbo].[CampoMedicion] WHERE NombreCampo = N'Porcentaje de Grasa (%)';
    SELECT @CampoMusculo = IdCampo FROM [dbo].[CampoMedicion] WHERE NombreCampo = N'Porcentaje de Músculo (%)';

    -- Eliminar las mediciones del seed base
    DELETE FROM [dbo].[RegistroMedicion] WHERE idUsuario = @IdUsuarioCliente;

    -- Evolución realista de una mujer ~34 años en 11 meses de entrenamiento
    -- Peso: 72kg → 64kg (bajó 8kg)
    -- Grasa: 35% → 26% (bajó 9%)
    -- Músculo: 28% → 36% (subió 8%)
    INSERT INTO [dbo].[RegistroMedicion] ([idUsuario],[IdCampo],[Valor],[FechaRegistro])
    VALUES
        -- Noviembre 2025 (inicio)
        (@IdUsuarioCliente, @CampoPeso,    72.00, '2025-11-05T08:00:00'),
        (@IdUsuarioCliente, @CampoGrasa,   35.00, '2025-11-05T08:00:00'),
        (@IdUsuarioCliente, @CampoMusculo, 28.00, '2025-11-05T08:00:00'),

        -- Diciembre 2025
        (@IdUsuarioCliente, @CampoPeso,    71.20, '2025-12-03T08:00:00'),
        (@IdUsuarioCliente, @CampoGrasa,   34.10, '2025-12-03T08:00:00'),
        (@IdUsuarioCliente, @CampoMusculo, 28.80, '2025-12-03T08:00:00'),

        -- Enero 2026
        (@IdUsuarioCliente, @CampoPeso,    70.50, '2026-01-07T08:00:00'),
        (@IdUsuarioCliente, @CampoGrasa,   33.20, '2026-01-07T08:00:00'),
        (@IdUsuarioCliente, @CampoMusculo, 29.50, '2026-01-07T08:00:00'),

        -- Febrero 2026
        (@IdUsuarioCliente, @CampoPeso,    69.30, '2026-02-04T08:00:00'),
        (@IdUsuarioCliente, @CampoGrasa,   32.00, '2026-02-04T08:00:00'),
        (@IdUsuarioCliente, @CampoMusculo, 30.50, '2026-02-04T08:00:00'),

        -- Marzo 2026
        (@IdUsuarioCliente, @CampoPeso,    68.00, '2026-03-04T08:00:00'),
        (@IdUsuarioCliente, @CampoGrasa,   30.80, '2026-03-04T08:00:00'),
        (@IdUsuarioCliente, @CampoMusculo, 31.50, '2026-03-04T08:00:00'),

        -- Abril 2026
        (@IdUsuarioCliente, @CampoPeso,    67.10, '2026-04-01T08:00:00'),
        (@IdUsuarioCliente, @CampoGrasa,   29.50, '2026-04-01T08:00:00'),
        (@IdUsuarioCliente, @CampoMusculo, 32.50, '2026-04-01T08:00:00'),

        -- Mayo 2026
        (@IdUsuarioCliente, @CampoPeso,    66.50, '2026-05-06T08:00:00'),
        (@IdUsuarioCliente, @CampoGrasa,   28.50, '2026-05-06T08:00:00'),
        (@IdUsuarioCliente, @CampoMusculo, 33.50, '2026-05-06T08:00:00'),

        -- Junio 2026
        (@IdUsuarioCliente, @CampoPeso,    65.80, '2026-06-03T08:00:00'),
        (@IdUsuarioCliente, @CampoGrasa,   27.80, '2026-06-03T08:00:00'),
        (@IdUsuarioCliente, @CampoMusculo, 34.20, '2026-06-03T08:00:00'),

        -- Julio 2026
        (@IdUsuarioCliente, @CampoPeso,    65.20, '2026-07-02T08:00:00'),
        (@IdUsuarioCliente, @CampoGrasa,   27.00, '2026-07-02T08:00:00'),
        (@IdUsuarioCliente, @CampoMusculo, 35.00, '2026-07-02T08:00:00'),

        -- Agosto 2026
        (@IdUsuarioCliente, @CampoPeso,    64.50, '2026-08-06T08:00:00'),
        (@IdUsuarioCliente, @CampoGrasa,   26.30, '2026-08-06T08:00:00'),
        (@IdUsuarioCliente, @CampoMusculo, 35.50, '2026-08-06T08:00:00'),

        -- Septiembre 2026 (hoy)
        (@IdUsuarioCliente, @CampoPeso,    64.00, '2026-09-10T08:00:00'),
        (@IdUsuarioCliente, @CampoGrasa,   25.80, '2026-09-10T08:00:00'),
        (@IdUsuarioCliente, @CampoMusculo, 36.00, '2026-09-10T08:00:00');

    PRINT 'Mediciones creadas: 33 registros (11 meses × 3 métricas).';

    -- ============================================================
    -- LIMPIEZA
    -- ============================================================
    DROP TABLE #ClasesGeneradas;

    -- ============================================================
    -- COMMIT
    -- ============================================================
    COMMIT TRANSACTION;

    PRINT '';
    PRINT '=====================================================';
    PRINT '  DATOS HISTÓRICOS INSERTADOS EXITOSAMENTE';
    PRINT '=====================================================';
    PRINT '';
    PRINT '  Usuario: diana.brenes1509@gmail.com';
    PRINT '  Período: Noviembre 2025 → Septiembre 2026';
    PRINT '';
    PRINT '  Resumen de datos generados:';
    PRINT '  +-------------------------------+-----------+';
    PRINT '  | Tabla                         | Registros |';
    PRINT '  +-------------------------------+-----------+';
    PRINT '  | MembresiaCliente              | 6         |';
    PRINT '  | Pago                          | 6         |';
    PRINT '  | Entrenamiento (rutinas)       | 6         |';
    PRINT '  | ClaseProgramada               | ~160      |';
    PRINT '  | Reserva                       | ~137      |';
    PRINT '  | ProgresoCliente               | 33        |';
    PRINT '  | RegistroMedicion              | 33        |';
    PRINT '  +-------------------------------+-----------+';
    PRINT '';
    PRINT '=====================================================';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    IF OBJECT_ID('tempdb..#ClasesGeneradas') IS NOT NULL
        DROP TABLE #ClasesGeneradas;

    PRINT '';
    PRINT '!!! ERROR - Se revirtieron TODOS los cambios !!!';
    PRINT 'Mensaje: ' + ERROR_MESSAGE();
    PRINT 'Linea:   ' + CAST(ERROR_LINE() AS NVARCHAR(10));
    PRINT '';

    THROW;
END CATCH;
GO
