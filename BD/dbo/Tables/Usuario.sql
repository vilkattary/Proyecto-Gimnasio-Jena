CREATE TABLE [dbo].[Usuario] (
    [idUsuario]         INT            IDENTITY (1, 1) NOT NULL,
    [identityUserId]    NVARCHAR (128) NULL,
    [nombre]            NVARCHAR (50)  NOT NULL,
    [apellido1]         NVARCHAR (50)  NOT NULL,
    [apellido2]         NVARCHAR (50)  NULL,
    [identificacion]    NVARCHAR (50)  NOT NULL,
    [correo]            NVARCHAR (100) NOT NULL,
    [telefono]          NVARCHAR (20)  NULL,
    [fechaRegistro]     DATETIME2 (7)  DEFAULT (sysutcdatetime()) NOT NULL,
    [fechaModificacion] DATETIME2 (7)  NULL,
    [estado]            BIT            DEFAULT ((1)) NOT NULL,
    [direccion]         NVARCHAR (300) NULL,
    [fotoPerfil]        NVARCHAR (300) NULL,
    PRIMARY KEY CLUSTERED ([idUsuario] ASC),
    UNIQUE NONCLUSTERED ([correo] ASC),
    UNIQUE NONCLUSTERED ([identificacion] ASC),
    UNIQUE NONCLUSTERED ([identityUserId] ASC),
    CONSTRAINT [UQ_Usuario_Correo] UNIQUE NONCLUSTERED ([correo] ASC),
    CONSTRAINT [UQ_Usuario_Identificacion] UNIQUE NONCLUSTERED ([identificacion] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Usuario_IdentityUserId]
    ON [dbo].[Usuario]([identityUserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Usuario_Correo]
    ON [dbo].[Usuario]([correo] ASC);

