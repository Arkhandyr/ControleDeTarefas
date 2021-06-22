CREATE TABLE [dbo].[TBTarefas]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY,
	[Titulo] VARCHAR(64) NOT NULL,
	[Data_Inicial] DATETIME NOT NULL,
	[Data_Conclusao] DATETIME,
	[Porcentagem] VARCHAR(64),
	[Prioridade] INT NOT NULL
)

insert into TBTarefas
	(
		[Titulo], 
		[Data_Inicial], 
		[Data_Conclusao],
		[Porcentagem],
		[Prioridade]
	) 
	values 
	(
		'Atividade 01', 
		'06/01/2000',
		'',
		'0%',
		'1'
	)

	SELECT SCOPE_IDENTITY();

insert into TBTarefas
	(
		[Titulo], 
		[Data_Inicial], 
		[Prioridade]
	) 
	values 
	(
		'Atividade 04',
		'07/01/2000',
		1
)

update TBFuncionario 
	set	
		[Titulo] = 'Atividade 02 atualizada',
		[Data_Inicial] = '06/07/2000',
		[Prioridade] = 3
	where 
		[ID] = 2

Delete from TBFuncionario 
	where 
		[ID] = 1

select [ID], [Titulo], [Data_Inicial], [Prioridade] from TBTarefas

select [ID], [Titulo], [DataNascimento], [Salario] from TBTarefas
	where 
		[ID] = 1

select * from TBTarefas
TRUNCATE TABLE TBTarefas
DBCC CHECKIDENT ('TBTarefas', RESEED, 0);  
DBCC CHECKIDENT ('TBTarefas', RESEED)