CREATE TABLE [dbo].[ProgresionEjercicio] (
	[idProgresion]         INT            IDENTITY (1, 1) NOT NULL,
	[idEjercicio]          INT            NOT NULL,
	[NumeroSemana]         INT            NOT NULL,
	[Series]               INT            NULL,
	[RepeticionesObjetivo] NVARCHAR (50)  NULL,
	[DuracionSegundos]     INT            NULL,
	[CargaOrpeSugerido]    NVARCHAR (100) NULL,
	CONSTRAINT [PK_ProgresionEjercicio] PRIMARY KEY CLUSTERED ([idProgresion] ASC),
	CONSTRAINT [FK_Progresion_Ejercicio] FOREIGN KEY ([idEjercicio]) REFERENCES [dbo].[EjercicioEntrenamiento] ([idEjercicio]) ON DELETE CASCADE,
	CONSTRAINT [CK_Progresion_Semana] CHECK ([NumeroSemana]>=(1) AND [NumeroSemana]<=(4))
);
