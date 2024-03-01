CREATE TABLE [wds].[callType_map]
(
	[callType_id] INT NOT NULL PRIMARY KEY, 
    [name] varchar(64) NOT NULL, 
    [description] varchar(255) NOT NULL, 
    [is_active] BIT NOT NULL
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [UK_calltype_map_Name] ON [wds].[callType_map] ([name])
GO
