CREATE TABLE [dbo].[MetodoPago] (
    [idMetodoPago] INT           IDENTITY (1, 1) NOT NULL,
    [nombreMetodo] NVARCHAR (50) NOT NULL,
    [estado]       BIT           DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([idMetodoPago] ASC),
    UNIQUE NONCLUSTERED ([nombreMetodo] ASC)
);

