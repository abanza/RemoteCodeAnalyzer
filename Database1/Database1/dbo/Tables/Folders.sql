﻿CREATE TABLE [dbo].[Folders]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [UserId] INT NOT NULL, 
    [Name] NVARCHAR(1000) NOT NULL
)
