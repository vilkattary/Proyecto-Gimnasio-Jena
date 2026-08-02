CREATE TABLE [dbo].[MembresiaCliente] (
    [idMembresiaCliente] INT            IDENTITY (1, 1) NOT NULL,
    [idUsuario]          INT            NOT NULL,
    [idPlanMembresia]    INT            NOT NULL,
    [idEstadoMembresia]  INT            NOT NULL,
    [fechaInicio]        DATE           NOT NULL,
    [fechaFin]           DATE           NOT NULL,
    [clasesDisponibles]  INT            NULL,
    [observaciones]      NVARCHAR (500) NULL,
    [fechaCreacion]      DATETIME2 (7)  DEFAULT (sysutcdatetime()) NOT NULL,
    PRIMARY KEY CLUSTERED ([idMembresiaCliente] ASC),
    CHECK ([clasesDisponibles]>=(0)),
    CONSTRAINT [CK_Membresia_Fechas] CHECK ([fechaFin]>[fechaInicio]),
    CONSTRAINT [FK_MembresiaCliente_Estado] FOREIGN KEY ([idEstadoMembresia]) REFERENCES [dbo].[EstadoMembresia] ([idEstadoMembresia]),
    CONSTRAINT [FK_MembresiaCliente_Plan] FOREIGN KEY ([idPlanMembresia]) REFERENCES [dbo].[PlanMembresia] ([idPlanMembresia]),
    CONSTRAINT [FK_MembresiaCliente_Usuario] FOREIGN KEY ([idUsuario]) REFERENCES [dbo].[Usuario] ([idUsuario])
);


GO
CREATE NONCLUSTERED INDEX [IX_MembresiaCliente_Usuario]
    ON [dbo].[MembresiaCliente]([idUsuario] ASC);

