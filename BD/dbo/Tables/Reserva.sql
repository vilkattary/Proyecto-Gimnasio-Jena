CREATE TABLE [dbo].[Reserva] (
    [idReserva]         INT            IDENTITY (1, 1) NOT NULL,
    [idUsuario]         INT            NOT NULL,
    [idClaseProgramada] INT            NOT NULL,
    [idEstadoReserva]   INT            NOT NULL,
    [fechaReserva]      DATETIME2 (7)  DEFAULT (sysutcdatetime()) NOT NULL,
    [observaciones]     NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([idReserva] ASC),
    CONSTRAINT [FK_Reserva_Clase] FOREIGN KEY ([idClaseProgramada]) REFERENCES [dbo].[ClaseProgramada] ([idClaseProgramada]),
    CONSTRAINT [FK_Reserva_Estado] FOREIGN KEY ([idEstadoReserva]) REFERENCES [dbo].[EstadoReserva] ([idEstadoReserva]),
    CONSTRAINT [FK_Reserva_Usuario] FOREIGN KEY ([idUsuario]) REFERENCES [dbo].[Usuario] ([idUsuario]),
    CONSTRAINT [UQ_Reserva_Usuario_Clase] UNIQUE NONCLUSTERED ([idUsuario] ASC, [idClaseProgramada] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Reserva_Usuario]
    ON [dbo].[Reserva]([idUsuario] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Reserva_Clase]
    ON [dbo].[Reserva]([idClaseProgramada] ASC);

