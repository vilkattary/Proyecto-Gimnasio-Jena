CREATE TABLE [dbo].[Pago] (
    [idPago]             INT             IDENTITY (1, 1) NOT NULL,
    [idMembresiaCliente] INT             NOT NULL,
    [idMetodoPago]       INT             NOT NULL,
    [idEstadoPago]       INT             NOT NULL,
    [monto]              DECIMAL (10, 2) NOT NULL,
    [fechaPago]          DATETIME2 (7)   DEFAULT (sysutcdatetime()) NOT NULL,
    [referenciaPago]     NVARCHAR (100)  NULL,
    [observaciones]      NVARCHAR (300)  NULL,
    PRIMARY KEY CLUSTERED ([idPago] ASC),
    CHECK ([monto]>(0)),
    CONSTRAINT [FK_Pago_Estado] FOREIGN KEY ([idEstadoPago]) REFERENCES [dbo].[EstadoPago] ([idEstadoPago]),
    CONSTRAINT [FK_Pago_Membresia] FOREIGN KEY ([idMembresiaCliente]) REFERENCES [dbo].[MembresiaCliente] ([idMembresiaCliente]),
    CONSTRAINT [FK_Pago_Metodo] FOREIGN KEY ([idMetodoPago]) REFERENCES [dbo].[MetodoPago] ([idMetodoPago])
);


GO
CREATE NONCLUSTERED INDEX [IX_Pago_Fecha]
    ON [dbo].[Pago]([fechaPago] ASC);

