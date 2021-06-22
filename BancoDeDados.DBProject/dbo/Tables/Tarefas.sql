CREATE TABLE [dbo].[Tarefas] (
    [ID]             INT          NOT NULL,
    [Titulo]         VARCHAR (64) NOT NULL,
    [Data_Inicial]   DATETIME     NOT NULL,
    [Data_Conclusao] DATETIME     NULL,
    [Porcentagem]    VARCHAR (64) NULL,
    [Prioridade]     INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

