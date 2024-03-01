CREATE TABLE [wds].[hosts]
(
	[host_id] INT IDENTITY(1,1)  NOT NULL PRIMARY KEY, 
    [host_name] VARCHAR(64) NOT NULL,
	[description] VARCHAR(64) NULL, 
    [is_active] BIT NOT NULL, 
    [mod_dt] DATETIME NOT NULL
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [UK_Hosts_Name] ON [wds].[Hosts] ([host_name])
GO
ALTER TABLE [wds].[Hosts] ADD  CONSTRAINT [DF_Hosts_mod_dt]  DEFAULT (getdate()) FOR [mod_dt]
GO
ALTER TABLE [wds].[Hosts] ADD  CONSTRAINT [DF_Hosts_is_active]  DEFAULT ((1)) FOR [is_active]
GO