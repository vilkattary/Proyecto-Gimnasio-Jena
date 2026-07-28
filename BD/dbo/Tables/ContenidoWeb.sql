CREATE TABLE [dbo].[ContenidoWeb] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [Pagina]            VARCHAR (50)   NOT NULL,
    [Seccion]           VARCHAR (50)   NOT NULL,
    [Clave]             VARCHAR (50)   NOT NULL,
    [TextoPrincipal]    NVARCHAR (MAX) NULL,
    [TextoSecundario]   NVARCHAR (MAX) NULL,
    [UrlImagen]         VARCHAR (2048) NULL,
    [Orden]             INT            DEFAULT ((0)) NOT NULL,
    [FechaModificacion] DATETIME       DEFAULT (getdate()) NULL,
    [Estado]            BIT            DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

