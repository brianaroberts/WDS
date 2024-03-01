CREATE TABLE [wds].[recordsets]
(
	[recordset_id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[app_id] INT NOT NULL, 
    [name] VARCHAR(64) NOT NULL, 
    [substitute] VARCHAR(64) NULL, 
    [access_role_id] INT NOT NULL,  
    [connection_id] INT NOT NULL, 
    [callType_id] INT NOT NULL, 
    [returnType_id] INT NOT NULL, 
    [returnType_details_id] INT NOT NULL, 
    [statement] VARCHAR(4000) NOT NULL, 
    [cache] BIT NOT NULL, 
    [cache_timeout] INT NOT NULL,    
    [endpoint_required] BIT NOT NULL, 
    [log_call] BIT NOT NULL, 
    [create_dt] [datetime],
	[mod_dt] [datetime],    
    [is_active] BIT NOT NULL, 
    CONSTRAINT [fk_recordsets_plugins] FOREIGN KEY ([app_id]) REFERENCES [wds].[applications]([app_id]), 
    CONSTRAINT [FK_recordsets_connections] FOREIGN KEY ([connection_id]) REFERENCES [wds].[connections]([connection_id]), 
    CONSTRAINT [FK_recordsets_application_roles] FOREIGN KEY ([access_role_id]) REFERENCES [wds].[roles]([role_id]),
    CONSTRAINT [FK_recordsets_callType] FOREIGN KEY ([callType_id]) REFERENCES [wds].[callType_map]([callType_id]),
    CONSTRAINT [FK_recordsets_returnType] FOREIGN KEY ([returnType_id]) REFERENCES [wds].[returnType_map]([returnType_id]),
    CONSTRAINT [FK_recordsets_returnType_Details] FOREIGN KEY ([returnType_details_id]) REFERENCES [wds].[returnType_details_map]([details_id])
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [uk_recordset_name] ON [wds].[recordsets] ([name],[is_active])
GO
ALTER TABLE [wds].[recordsets] ADD  CONSTRAINT [df_recordsets_name]  DEFAULT ('default') FOR [name]
GO
ALTER TABLE [wds].[recordsets] ADD  CONSTRAINT [df_recordsets_is_active]  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [wds].[recordsets] ADD  CONSTRAINT [df_recordset_create_dt]  DEFAULT (getdate()) FOR [create_dt]
GO
ALTER TABLE [wds].[recordsets] ADD  CONSTRAINT [df_recordset_mod_dt]  DEFAULT (getdate()) FOR [mod_dt]
GO

