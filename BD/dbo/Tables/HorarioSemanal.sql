CREATE TABLE [dbo].[HorarioSemanal] (
    [idHorario]           INT           IDENTITY (1, 1) NOT NULL,
    [idTipoClase]         INT           NOT NULL,
    [idUsuarioEntrenador] INT           NOT NULL,
    [diaSemana]           TINYINT       NOT NULL,
    [horaInicio]          TIME (7)      NOT NULL,
    [horaFin]             TIME (7)      NOT NULL,
    [cupoMaximo]          INT           NOT NULL,
    [ubicacion]           VARCHAR (100) NOT NULL,
    [estado]              BIT           DEFAULT ((1)) NOT NULL,
    [fechaCreacion]       DATETIME      NOT NULL,
    [fechaModificacion]   DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([idHorario] ASC),
    CONSTRAINT [FK_Horario_Entrenador] FOREIGN KEY ([idUsuarioEntrenador]) REFERENCES [dbo].[Usuario] ([idUsuario]),
    CONSTRAINT [FK_Horario_TipoClase] FOREIGN KEY ([idTipoClase]) REFERENCES [dbo].[TipoClase] ([idTipoClase]),
    CONSTRAINT [UQ_HorarioSemanal] UNIQUE NONCLUSTERED ([diaSemana] ASC, [horaInicio] ASC, [idTipoClase] ASC, [idUsuarioEntrenador] ASC),
    CONSTRAINT [UQ_HorarioSemanal_Entrenador] UNIQUE NONCLUSTERED ([diaSemana] ASC, [horaInicio] ASC, [idUsuarioEntrenador] ASC)
);

