CREATE TABLE [dbo].[UserSetLog] (
	[Id]                 INT             IDENTITY (1, 1) NOT NULL,
	[UserWorkoutLogId]   INT             NOT NULL,
	[WorkoutExerciseId]  INT             NOT NULL,
	[SetNumber]          INT             NOT NULL,
	[RepsCompleted]      INT             NULL,
	[WeightUsedKg]       DECIMAL (5, 2)  NULL,
	[DurationSeconds]    INT             NULL,
	[DistanceMeters]     DECIMAL (6, 2)  NULL,
	[Estimated1RM]       DECIMAL (5, 2)  NULL,
	[VolumeLoad]         DECIMAL (8, 2)  NULL,
	CONSTRAINT [PK_UserSetLog] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_UserSetLog_WorkoutLog] FOREIGN KEY ([UserWorkoutLogId]) REFERENCES [dbo].[UserWorkoutLog] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_UserSetLog_Ejercicio] FOREIGN KEY ([WorkoutExerciseId]) REFERENCES [dbo].[EjercicioEntrenamiento] ([idEjercicio])
);
