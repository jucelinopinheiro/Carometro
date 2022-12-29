--- Base de dados para carômetro em sua versão 6.0

USE [master];

DECLARE @kill varchar(8000) = '';  
SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'  
FROM sys.dm_exec_sessions
WHERE database_id  = db_id('Curso')

EXEC(@kill);

DROP DATABASE [DbCarometro]

USE [master]
GO

CREATE DATABASE [DbCarometro]
GO

USE [DbCarometro]
GO

--Tabelas

--Id usuário é o nif
CREATE TABLE [Usuarios]
(
    [Id] INT NOT NULL,
    [Nome] VARCHAR(80) NOT NULL,
    [Email] VARCHAR(80),
    [SenhaHash] VARCHAR(255) NOT NULL,
    [Perfil] TINYINT NOT NULL,
    [Notificar] BIT NOT NULL DEFAULT(0),
    [CreateAt] DATE NOT NULL,
    [UpdateAt] DATE,
    
    CONSTRAINT [PK_Usuarios] PRIMARY KEY([Id])
)
GO

CREATE TABLE [Cursos]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [Descricao] VARCHAR(50) NOT NULL, 
    [Tipo] TINYINT NOT NULL,
    [Cor] VARCHAR(7),
    [Ativo] BIT NOT NULL DEFAULT(1),
    [CreateAt] DATE NOT NULL,
    [UpdateAt] DATE,
    
    CONSTRAINT [PK_Cursos] PRIMARY KEY([Id])
)
GO

CREATE TABLE [Turmas]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [Descricao] VARCHAR(80) NOT NULL, 
    [Sigla] VARCHAR(50)NOT NULL,
    [TurmaSgset] VARCHAR(20)NOT NULL,
    [Classroom] VARCHAR(80),
    [DataInicio] DATE NOT NULL,
    [DataFim] DATE NOT NULL,
    [Ativo] BIT NOT NULL DEFAULT(1),
    [CreateAt] DATE NOT NULL,
    [UpdateAt] DATE,
    [CursoId] INT NOT NULL,

    CONSTRAINT [Pk_Turmas] PRIMARY KEY([Id]),
    CONSTRAINT [FK_Turmas_Cursos] FOREIGN KEY([CursoId]) REFERENCES [Cursos]([Id])
)
GO

CREATE TABLE [Alunos]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [Nome] VARCHAR(80) NOT NULL,
    [CelAluno]VARCHAR(15),
    [EmailAluno]VARCHAR(80),
    [Nascimento] DATE NOT NULL,
    [Rg]VARCHAR(14) NOT NULL,
    [Cpf]VARCHAR(14) NOT NULL,
    [Pai] VARCHAR(80),
    [CelPai] VARCHAR(15),
    [Mae] VARCHAR(80),
    [CelMae]VARCHAR(15),
    [Foto]VARCHAR(255),
    [Pne]BIT NOT NULL DEFAULT(0),
    [ObsAluno]VARCHAR(255),
    [CreateAt] DATE NOT NULL,
    [UpdateAt] DATE,

    CONSTRAINT [Pk_Alunos] PRIMARY KEY([Id])
)
GO

CREATE TABLE[Matriculas]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [MatriculaSgset] INT NOT NULL,
    [DataMatricula] DATE,
    [AlunoId]INT NOT NULL,
    [TurmaId]INT NOT NULL,
    [CreateAt] DATE NOT NULL,
    [UpdateAt] DATE,
    
    CONSTRAINT [Pk_Matriculas] PRIMARY KEY([Id]),
    CONSTRAINT [FK_Matriculas_Alunos] FOREIGN KEY([AlunoId]) REFERENCES [Alunos]([Id]),
    CONSTRAINT [FK_Matriculas_Turmas] FOREIGN KEY([TurmaId]) REFERENCES [Turmas]([Id])
    
)
GO

CREATE TABLE[Ocorrencias]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [UsuarioId] INT NOT NULL,
    [AlunoId] INT NOT NULL,
    [TurmaId] INT NOT NULL,
    [Nome] VARCHAR(20),
    [Descricao] TEXT,
    [CreateAt] DATE NOT NULL,
    [UpdateAt] DATE,
    
    CONSTRAINT [Pk_Ocorrencias] PRIMARY KEY([Id]),
    CONSTRAINT [FK_Ocorrencias_Usuarios] FOREIGN KEY([UsuarioId]) REFERENCES [Usuarios]([Id]),
    CONSTRAINT [FK_Ocorrencias_Alunos] FOREIGN KEY([AlunoId]) REFERENCES [Alunos]([Id]),
    CONSTRAINT [FK_Ocorrencias_Turmas] FOREIGN KEY([TurmaID]) REFERENCES [Turmas]([Id])
    
)
GO

CREATE TABLE[Anexos]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [OcorrenciaId]INT NOT NULL,
    [Descricao] VARCHAR(20),
    [UrlAnexo]VARCHAR(255) NOT NULL,
    [CreateAt] DATE NOT NULL,
    [UpdateAt] DATE,
    
    CONSTRAINT [Pk_Anexos] PRIMARY KEY([Id]),
    CONSTRAINT [FK_Anexos_Ocorrencias] FOREIGN KEY([OcorrenciaId]) REFERENCES [Ocorrencias]([Id])
)
GO

INSERT INTO [Usuarios]([Id],[Nome],[SenhaHash],[Perfil], [CreateAt], [UpdateAt]) VALUES (12345, 'Administrador', '10000.qP6NldYeIkUilgv2kC+GHA==.GIx2sUlVpcFjx7e2I1yEtD6Z1f4qOvRZ4imV/bYPsJw=', 1, GETDATE(), GETDATE())

INSERT INTO [Cursos]([Descricao], [Tipo],[Cor],[CreateAt], [UpdateAt])VALUES('Técnico em Cybersegurança', 1, '#016974',GETDATE(), GETDATE())

INSERT INTO [Turmas]([Descricao], [Sigla], [TurmaSgset], [DataInicio], [DataFim], [CursoId], [CreateAt], [UpdateAt])
VALUES('Primeiro Termo Técnido em Cybersegurança - Noite','2023-1S-1CN','1CN','2023-01-26', '2023-06-16', 1, GETDATE(), GETDATE())