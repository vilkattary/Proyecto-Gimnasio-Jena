CREATE TABLE [dbo].[TipoClase] (
    [idTipoClase]     INT            IDENTITY (1, 1) NOT NULL,
    [nombreClase]     NVARCHAR (100) NOT NULL,
    [descripcion]     NVARCHAR (500) NULL,
    [duracionMinutos] INT            NOT NULL,
    [estado]          BIT            DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([idTipoClase] ASC),
    CHECK ([duracionMinutos]>(0)),
    UNIQUE NONCLUSTERED ([nombreClase] ASC)
);

