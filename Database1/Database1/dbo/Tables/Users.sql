CREATE TABLE [dbo].[Users] (
    [Id]       INT         IDENTITY (1, 1) NOT NULL,
    [Role]     NVARCHAR(200) NOT NULL,
    [Username] NVARCHAR(500) NOT NULL, 
    [Password] NVARCHAR(1000) NOT NULL
);

