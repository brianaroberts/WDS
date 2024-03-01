CREATE TABLE [WDS].[application_access_log](
	[log_id] [int] IDENTITY(1,1) NOT NULL,
	[app_id] [int] NOT NULL,
	[recordset_id] [int] NOT NULL,
	[client_host] [varchar](64) NOT NULL,
	[server_host] [varchar](64) NOT NULL,
	[api_key] [varchar](64) NULL,
	[edipi] [int] NULL,
	[ptc] [char](1) NULL,
	[parameters] varchar(max) NULL, 
	[access_dt] [datetime] NOT NULL,
 CONSTRAINT [PK_ApplicationAccessLog] PRIMARY KEY CLUSTERED 
(
	[log_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [WDS].[application_access_log]  WITH CHECK ADD  CONSTRAINT [fk_aal_appId] FOREIGN KEY([app_id])
REFERENCES [WDS].[applications] ([app_id])
GO

ALTER TABLE [WDS].[application_access_log] CHECK CONSTRAINT [fk_aal_appId]
GO

ALTER TABLE [WDS].[application_access_log]  WITH CHECK ADD  CONSTRAINT [fk_aal_rsid] FOREIGN KEY([recordset_id])
REFERENCES [WDS].[recordsets] ([recordset_id])
GO

ALTER TABLE [WDS].[application_access_log] CHECK CONSTRAINT [fk_aal_rsid]
GO
