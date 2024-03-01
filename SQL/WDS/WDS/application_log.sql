CREATE TABLE [WDS].[application_log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[client_host] [varchar](64) NOT NULL,
	[server_host] [varchar](64) NOT NULL,
	[application] [varchar](64) NOT NULL,
	[log_area] [varchar](64) NOT NULL,
	[log_level] [int] NOT NULL,	
	[message] [varchar](4000) NULL,
	[create_date] datetime NOT NULL,
	CONSTRAINT [fk_applog_lm] FOREIGN KEY ([log_level]) REFERENCES [wds].[log_level_map]([log_level])
) ON [PRIMARY]
GO


ALTER TABLE [wds].[application_log] ADD  CONSTRAINT [df_appLog_create_dt]  DEFAULT (getdate()) FOR [create_date]
GO