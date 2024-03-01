CREATE TABLE [WDS].[application_roles](
	[role_id] [int] NOT NULL,
	[app_id] [int] NOT NULL,	
	[create_dt] [datetime] NULL,
	[mod_dt] [datetime] NULL, 
	[is_active] [bit] NOT NULL,
 CONSTRAINT [PK_ApplicationRoles] PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [WDS].[application_roles] ADD  CONSTRAINT [DF_ApplicationRoles_create_dt]  DEFAULT (getdate()) FOR [create_dt]
GO
ALTER TABLE [WDS].[application_roles] ADD  CONSTRAINT [DF_ApplicationRoles_mod_dt]  DEFAULT (getdate()) FOR [mod_dt]
GO

ALTER TABLE [WDS].[application_roles] ADD  CONSTRAINT [DF_Applicationrole_is_active]  DEFAULT ((1)) FOR [is_active]
GO

ALTER TABLE [WDS].[application_roles]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationRoles_ToRoles] FOREIGN KEY([app_id])
REFERENCES [WDS].[applications] ([app_id])
GO

ALTER TABLE [WDS].[application_roles]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationRole_ToRoles] FOREIGN KEY([role_id])
REFERENCES [WDS].[roles] ([role_id])
GO