CREATE TABLE [wds].[returnType_map]
(
	[returnType_Id] INT NOT NULL PRIMARY KEY, 
    [name] varchar(64) NOT NULL, 
    [description] varchar(255) NOT NULL, 
    [is_active] BIT NOT NULL
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [UK_returnType_map_Name] ON [wds].[returnType_map] ([name])
GO
