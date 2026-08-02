CREATE PROCEDURE [dbo].[LimpiarTodasLasClases]
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [dbo].[Asistencia]
    WHERE [idReserva] IN (
        SELECT r.[idReserva]
        FROM [dbo].[Reserva] r
        INNER JOIN [dbo].[ClaseProgramada] c
            ON r.[idClaseProgramada] = c.[idClaseProgramada]
    );

    DELETE FROM [dbo].[Reserva]
    WHERE [idClaseProgramada] IN (
        SELECT [idClaseProgramada]
        FROM [dbo].[ClaseProgramada]
    );

    DELETE FROM [dbo].[ClaseProgramada];
END
