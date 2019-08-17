CREATE TABLE [dbo].[SharedFolders]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [OwnerId] INT NOT NULL, 
    [UserId] INT NOT NULL,
    [FolderId] INT NOT NULL
)
