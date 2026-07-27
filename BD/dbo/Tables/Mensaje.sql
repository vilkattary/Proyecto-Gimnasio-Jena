CREATE TABLE [dbo].[Mensaje] (
    [idMensaje]      INT             IDENTITY (1, 1) NOT NULL,
    [idUsuario]      INT             NULL,
    [nombre]         NVARCHAR (100)  NOT NULL,
    [correo]         NVARCHAR (100)  NOT NULL,
    [telefono]       NVARCHAR (20)   NULL,
    [asunto]         NVARCHAR (150)  NOT NULL,
    [mensaje]        NVARCHAR (1000) NOT NULL,
    [respuesta]      NVARCHAR (1000) NULL,
    [fechaEnvio]     DATETIME2 (7)   DEFAULT (sysutcdatetime()) NOT NULL,
    [fechaRespuesta] DATETIME2 (7)   NULL,
    [estado]         NVARCHAR (30)   DEFAULT ('Pendiente') NOT NULL,
    PRIMARY KEY CLUSTERED ([idMensaje] ASC),
    CONSTRAINT [CK_Mensaje_Estado] CHECK ([estado]='Cerrado' OR [estado]='Respondido' OR [estado]='Pendiente'),
    CONSTRAINT [FK_Mensaje_Usuario] FOREIGN KEY ([idUsuario]) REFERENCES [dbo].[Usuario] ([idUsuario])
);


GO
CREATE NONCLUSTERED INDEX [IX_Mensaje_Estado]
    ON [dbo].[Mensaje]([estado] ASC);

