CREATE TABLE [wds].[application_hosts]
(
	[app_id] INT NOT NULL, 
    [host_id] INT NOT NULL,
	[mod_dt] DATETIME NULL, 
    [is_active] BIT NULL, 
    CONSTRAINT [fk_ApplicationHosts_app_id] FOREIGN KEY ([app_id]) REFERENCES [wds].[applications]([app_id]),
	CONSTRAINT [fk_ApplicationHosts_host_id] FOREIGN KEY ([host_id]) REFERENCES [wds].[hosts]([host_id])
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [uk_ApplicationHosts_ids] ON [wds].[application_hosts] ([app_id],[host_id])
GO
