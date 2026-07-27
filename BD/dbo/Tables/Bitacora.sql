CREATE TABLE [dbo].[Bitacora] (
    [idBitacora]         INT            IDENTITY (1, 1) NOT NULL,
    [idUsuario]          INT            NULL,
    [tablaAfectada]      NVARCHAR (100) NOT NULL,
    [accionRealizada]    NVARCHAR (30)  NOT NULL,
    [idRegistroAfectado] INT            NULL,
    [fechaAccion]        DATETIME2 (7)  DEFAULT (sysutcdatetime()) NOT NULL,
    [detalle]            NVARCHAR (MAX) NULL,
    [ipUsuario]          NVARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([idBitacora] ASC),
    CONSTRAINT [CK_Bitacora_Accion] CHECK ([accionRealizada]='LOGOUT' OR [accionRealizada]='LOGIN' OR [accionRealizada]='CAMBIO_ESTADO' OR [accionRealizada]='UPDATE' OR [accionRealizada]='INSERT'),
    CONSTRAINT [FK_Bitacora_Usuario] FOREIGN KEY ([idUsuario]) REFERENCES [dbo].[Usuario] ([idUsuario])
);

