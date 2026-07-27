CREATE TABLE [dbo].[Entrenador] (
    [idEntrenador]      INT            IDENTITY (1, 1) NOT NULL,
    [idUsuario]         INT            NOT NULL,
    [especialidad]      NVARCHAR (100) NULL,
    [descripcion]       NVARCHAR (500) NULL,
    [fechaContratacion] DATE           DEFAULT (CONVERT([date],sysutcdatetime())) NOT NULL,
    [estado]            BIT            DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([idEntrenador] ASC),
    CONSTRAINT [FK_Entrenador_Usuario] FOREIGN KEY ([idUsuario]) REFERENCES [dbo].[Usuario] ([idUsuario]),
    CONSTRAINT [UQ_Entrenador_Usuario] UNIQUE NONCLUSTERED ([idUsuario] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Entrenador_Usuario]
    ON [dbo].[Entrenador]([idUsuario] ASC);

