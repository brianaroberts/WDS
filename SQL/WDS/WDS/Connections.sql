CREATE TABLE [wds].[connections](
	[connection_id] [int] IDENTITY(1,1) NOT NULL,
	[host_id] [int] NOT NULL, 
	[name] [varchar](64) NOT NULL,
	[connection_type] [varchar](32) NOT NULL,
	[production] [varchar](256) NOT NULL,
	[staging] [varchar](256) NOT NULL,
	[development] [varchar](256) NOT NULL,
	[create_dt] [datetime],
	[mod_dt] [datetime],
	[is_active] [bit] NOT NULL,
	CONSTRAINT [PK_Connections] PRIMARY KEY CLUSTERED 
	(
		[connection_id] ASC
	) 
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT [fk_connections_host_id] FOREIGN KEY ([host_id]) REFERENCES [wds].[hosts]([host_id])
) 
GO

CREATE UNIQUE NONCLUSTERED INDEX [UK_Connections_Name] ON [wds].[Connections] ([host_id],[name])
GO
ALTER TABLE [wds].[Connections] ADD  CONSTRAINT [CHK_Connections_type] CHECK (connection_type IN ('Database', 'Filesystem', 'WebService'))
GO
ALTER TABLE [wds].[Connections] ADD  CONSTRAINT [DF_Connections_create_dt]  DEFAULT (getdate()) FOR [create_dt]
GO
ALTER TABLE [wds].[Connections] ADD  CONSTRAINT [DF_Connections_mod_dt]  DEFAULT (getdate()) FOR [mod_dt]
GO
ALTER TABLE wds.[Connections] ADD  CONSTRAINT [DF_Connections_is_active]  DEFAULT ((1)) FOR [is_active]
GO