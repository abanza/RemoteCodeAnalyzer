CREATE TABLE [dbo].[Files]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [FolderId] INT NOT NULL, 
    [Name] NVARCHAR(1000) NOT NULL, 
    [FileData] VARBINARY(MAX) NULL, 
    [CheckedOutBy] INT NULL, 
    [AnalyzedXml] XML NULL
)
