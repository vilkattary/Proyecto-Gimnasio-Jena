CREATE PROCEDURE [dbo].[LimpiarClasesRecurrentes]
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [dbo].[Asistencia]
    WHERE [idReserva] IN (
        SELECT r.[idReserva]
        FROM [dbo].[Reserva] r
        INNER JOIN [dbo].[ClaseProgramada] c
            ON r.[idClaseProgramada] = c.[idClaseProgramada]
        WHERE c.[idHorario] IS NOT NULL
    );

    DELETE FROM [dbo].[Reserva]
    WHERE [idClaseProgramada] IN (
        SELECT [idClaseProgramada]
        FROM [dbo].[ClaseProgramada]
        WHERE [idHorario] IS NOT NULL
    );

    DELETE FROM [dbo].[ClaseProgramada]
    WHERE [idHorario] IS NOT NULL;
END
