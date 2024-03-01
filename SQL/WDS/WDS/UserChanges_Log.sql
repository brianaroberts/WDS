CREATE TABLE [wds].[userchanges_Log]
(
	[change_id] [int] identity (1,1) NOT NULL,
	[edipi] [int] NOT NULL,
	[ptc] [char](1) NOT NULL,
	[change_dt] [datetime], 
	[change_log] varchar(MAX),
	 CONSTRAINT [PK_UserChanges_Log] PRIMARY KEY CLUSTERED 
(
	[change_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [wds].[UserChanges_Log] ADD  CONSTRAINT [DF_UserChanges_Log_change_dt]  DEFAULT (getdate()) FOR [change_dt]
GO