CREATE TABLE [WDS].[roles](
	[role_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](32) NOT NULL,
	[description] [varchar](255) NOT NULL,
	[create_dt] [datetime] NULL,
	[mod_dt] [datetime] NULL,
	[expire_dt] [datetime] NULL,
	[is_active] [bit] NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [WDS].[roles] ADD  CONSTRAINT [DF_rolesmap_create_dt]  DEFAULT (getdate()) FOR [create_dt]
GO

ALTER TABLE [WDS].[roles] ADD  CONSTRAINT [DF_rolesmap_mod_dt]  DEFAULT (getdate()) FOR [mod_dt]
GO

ALTER TABLE [WDS].[roles] ADD  CONSTRAINT [DF_rolesmap_expire_dt]  DEFAULT (dateadd(year,(25),getdate())) FOR [expire_dt]
GO

ALTER TABLE [WDS].[roles] ADD  CONSTRAINT [DF_rolemap_is_active]  DEFAULT ((1)) FOR [is_active]
GO
