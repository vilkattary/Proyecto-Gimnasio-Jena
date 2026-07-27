CREATE TABLE [dbo].[EstadoPago] (
    [idEstadoPago] INT           IDENTITY (1, 1) NOT NULL,
    [nombreEstado] NVARCHAR (30) NOT NULL,
    [estado]       BIT           DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([idEstadoPago] ASC),
    UNIQUE NONCLUSTERED ([nombreEstado] ASC)
);

