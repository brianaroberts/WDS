CREATE TABLE [wds].[service_log]
(
	[log_id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[instance_id] [varchar](64) NOT NULL, 
	[log_level] [int] NOT NULL,	
	[host] [varchar](64) NOT NULL, 
	[message] varchar(max), 
	[create_dt] [datetime],
	CONSTRAINT [fk_servicelog_lm] FOREIGN KEY ([log_level]) REFERENCES [wds].[log_level_map]([log_level])
)
GO
ALTER TABLE [wds].[service_log] ADD  CONSTRAINT [df_servicelog_create_dt]  DEFAULT (getdate()) FOR [create_dt]
GO

