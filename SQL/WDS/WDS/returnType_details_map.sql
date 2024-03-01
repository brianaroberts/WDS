CREATE TABLE [wds].[returnType_details_map]
(
	[details_Id] INT NOT NULL PRIMARY KEY, 
    [name] varchar(64) NOT NULL, 
    [description] varchar(255) NOT NULL, 
    [is_active] BIT NOT NULL
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [UK_rtd_map_Name] ON [wds].[returnType_details_map] ([name])
GO
