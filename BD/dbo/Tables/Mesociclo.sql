CREATE TABLE [dbo].[Mesociclo] (
	[idMesociclo] INT            IDENTITY (1, 1) NOT NULL,
	[Nombre]      NVARCHAR (150) NOT NULL,
	[FechaInicio] DATE           NOT NULL,
	[FechaFin]    DATE           NOT NULL,
	[EsActivo]    BIT            DEFAULT ((1)) NOT NULL,
	CONSTRAINT [PK_Mesociclo] PRIMARY KEY CLUSTERED ([idMesociclo] ASC)
);
