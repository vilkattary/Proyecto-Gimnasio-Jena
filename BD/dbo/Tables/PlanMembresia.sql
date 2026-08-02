CREATE TABLE [dbo].[PlanMembresia] (
    [idPlanMembresia]    INT             IDENTITY (1, 1) NOT NULL,
    [nombrePlan]         NVARCHAR (100)  NOT NULL,
    [descripcion]        NVARCHAR (500)  NULL,
    [precio]             DECIMAL (10, 2) NOT NULL,
    [cantidadClases]     INT             NULL,
    [duracionDias]       INT             NOT NULL,
    [incluyeClasePrueba] BIT             DEFAULT ((0)) NOT NULL,
    [estado]             BIT             DEFAULT ((1)) NOT NULL,
    [fechaCreacion]      DATETIME2 (7)   DEFAULT (sysutcdatetime()) NOT NULL,
    PRIMARY KEY CLUSTERED ([idPlanMembresia] ASC),
    CHECK ([cantidadClases]>=(0)),
    CHECK ([duracionDias]>(0)),
    CHECK ([precio]>=(0))
);

