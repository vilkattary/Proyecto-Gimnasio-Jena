CREATE TABLE [dbo].[EstadoClase] (
    [idEstadoClase] INT           IDENTITY (1, 1) NOT NULL,
    [nombreEstado]  NVARCHAR (30) NOT NULL,
    [estado]        BIT           DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([idEstadoClase] ASC),
    UNIQUE NONCLUSTERED ([nombreEstado] ASC)
);

