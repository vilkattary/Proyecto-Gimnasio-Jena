CREATE TABLE [dbo].[ClaseProgramada] (
    [idClaseProgramada]   INT            IDENTITY (1, 1) NOT NULL,
    [idTipoClase]         INT            NOT NULL,
    [idUsuarioEntrenador] INT            NOT NULL,
    [idEstadoClase]       INT            NOT NULL,
    [fechaClase]          DATE           NOT NULL,
    [horaInicio]          TIME (7)       NOT NULL,
    [horaFin]             TIME (7)       NOT NULL,
    [cupoMaximo]          INT            NOT NULL,
    [ubicacion]           NVARCHAR (100) NULL,
    [observaciones]       NVARCHAR (500) NULL,
    [fechaCreacion]       DATETIME2 (7)  DEFAULT (sysutcdatetime()) NOT NULL,
    [fechaModificacion]   DATETIME2 (7)  NULL,
    [idHorario]           INT            NULL,
    PRIMARY KEY CLUSTERED ([idClaseProgramada] ASC),
    CHECK ([cupoMaximo]>(0)),
    CONSTRAINT [CK_Clase_Horas] CHECK ([horaFin]>[horaInicio]),
    CONSTRAINT [FK_ClaseProgramada_Entrenador] FOREIGN KEY ([idUsuarioEntrenador]) REFERENCES [dbo].[Usuario] ([idUsuario]),
    CONSTRAINT [FK_ClaseProgramada_Estado] FOREIGN KEY ([idEstadoClase]) REFERENCES [dbo].[EstadoClase] ([idEstadoClase]),
    CONSTRAINT [FK_ClaseProgramada_HorarioSemanal] FOREIGN KEY ([idHorario]) REFERENCES [dbo].[HorarioSemanal] ([idHorario]),
    CONSTRAINT [FK_ClaseProgramada_TipoClase] FOREIGN KEY ([idTipoClase]) REFERENCES [dbo].[TipoClase] ([idTipoClase])
);


GO
CREATE NONCLUSTERED INDEX [IX_ClaseProgramada_IdHorario]
    ON [dbo].[ClaseProgramada]([idHorario] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ClaseProgramada_Fecha]
    ON [dbo].[ClaseProgramada]([fechaClase] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ClaseProgramada_Entrenador]
    ON [dbo].[ClaseProgramada]([idUsuarioEntrenador] ASC);

