CREATE TABLE [dbo].[Rol] (
    [idRol]         INT            IDENTITY (1, 1) NOT NULL,
    [nombreRol]     NVARCHAR (50)  NOT NULL,
    [descripcion]   NVARCHAR (200) NULL,
    [estado]        BIT            DEFAULT ((1)) NOT NULL,
    [fechaCreacion] DATETIME2 (7)  DEFAULT (sysutcdatetime()) NOT NULL,
    PRIMARY KEY CLUSTERED ([idRol] ASC),
    UNIQUE NONCLUSTERED ([nombreRol] ASC)
);

