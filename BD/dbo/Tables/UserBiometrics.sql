CREATE TABLE [dbo].[UserBiometrics] (
	[Id]                     INT            IDENTITY (1, 1) NOT NULL,
	[UserId]                 INT            NOT NULL,
	[MeasurementDate]        DATE           NOT NULL,
	[WeightKg]               DECIMAL (5, 2) NOT NULL,
	[BodyFatPercentage]      DECIMAL (4, 2) NULL,
	[MuscleMassPercentage]   DECIMAL (4, 2) NULL,
	CONSTRAINT [PK_UserBiometrics] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_UserBiometrics_Usuario] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Usuario] ([idUsuario])
);
