CREATE TABLE [dbo].[PlantillaDiaEntrenamiento] (
	[idPlantillaDia] INT            IDENTITY (1, 1) NOT NULL,
	[idMesociclo]    INT            NOT NULL,
	[DiaSemana]      TINYINT        NOT NULL,
	[AreaEnfoque]    NVARCHAR (100) NULL,
	[OrdenIndice]    INT            DEFAULT ((0)) NOT NULL,
	CONSTRAINT [PK_PlantillaDiaEntrenamiento] PRIMARY KEY CLUSTERED ([idPlantillaDia] ASC),
	CONSTRAINT [FK_PlantillaDia_Mesociclo] FOREIGN KEY ([idMesociclo]) REFERENCES [dbo].[Mesociclo] ([idMesociclo]) ON DELETE CASCADE
);
