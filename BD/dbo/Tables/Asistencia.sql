CREATE TABLE [dbo].[Asistencia] (
    [idAsistencia]           INT            IDENTITY (1, 1) NOT NULL,
    [idReserva]              INT            NOT NULL,
    [idUsuarioRecepcionista] INT            NULL,
    [fechaRegistro]          DATETIME2 (7)  DEFAULT (sysutcdatetime()) NOT NULL,
    [asistio]                BIT            NOT NULL,
    [observaciones]          NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([idAsistencia] ASC),
    CONSTRAINT [FK_Asistencia_Recepcionista] FOREIGN KEY ([idUsuarioRecepcionista]) REFERENCES [dbo].[Usuario] ([idUsuario]),
    CONSTRAINT [FK_Asistencia_Reserva] FOREIGN KEY ([idReserva]) REFERENCES [dbo].[Reserva] ([idReserva]),
    UNIQUE NONCLUSTERED ([idReserva] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Asistencia_Reserva]
    ON [dbo].[Asistencia]([idReserva] ASC);

