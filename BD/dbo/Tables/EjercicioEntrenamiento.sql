CREATE TABLE [dbo].[EjercicioEntrenamiento] (
	[idEjercicio]     INT            IDENTITY (1, 1) NOT NULL,
	[idPlantillaDia]  INT            NOT NULL,
	[NombreEjercicio] NVARCHAR (150) NOT NULL,
	[TipoMetrica]     INT            DEFAULT ((1)) NOT NULL,
	[OrdenIndice]     INT            DEFAULT ((0)) NOT NULL,
	[Notas]           NVARCHAR (500) NULL,
	CONSTRAINT [PK_EjercicioEntrenamiento] PRIMARY KEY CLUSTERED ([idEjercicio] ASC),
	CONSTRAINT [FK_Ejercicio_PlantillaDia] FOREIGN KEY ([idPlantillaDia]) REFERENCES [dbo].[PlantillaDiaEntrenamiento] ([idPlantillaDia]) ON DELETE CASCADE
);
