CREATE TABLE [dbo].[UserWorkoutLog] (
	[Id]                     INT           IDENTITY (1, 1) NOT NULL,
	[UserId]                 INT           NOT NULL,
	[ClassInstanceId]        INT           NOT NULL,
	[WorkoutDayTemplateId]   INT           NOT NULL,
	[LoggedDate]             DATETIME2 (7) CONSTRAINT [DF_UserWorkoutLog_LoggedDate] DEFAULT (sysutcdatetime()) NOT NULL,
	[Notes]                  NVARCHAR (500) NULL,
	CONSTRAINT [PK_UserWorkoutLog] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_UserWorkoutLog_Usuario] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Usuario] ([idUsuario]),
	CONSTRAINT [FK_UserWorkoutLog_Clase] FOREIGN KEY ([ClassInstanceId]) REFERENCES [dbo].[ClaseProgramada] ([idClaseProgramada]),
	CONSTRAINT [FK_UserWorkoutLog_Plantilla] FOREIGN KEY ([WorkoutDayTemplateId]) REFERENCES [dbo].[PlantillaDiaEntrenamiento] ([idPlantillaDia])
);
