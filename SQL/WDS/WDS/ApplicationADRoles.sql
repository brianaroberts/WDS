CREATE TABLE [wds].[application_AD_roles]
(
	[app_id] INT NOT NULL, 
    [role_id] INT NOT NULL, 
    [ad_user_group] varchar(128) NOT NULL,
    [create_dt] [datetime] NULL,
	[mod_dt] [datetime] NULL,
	[expire_dt] [datetime] NULL,	
	[is_active] [bit] NOT NULL,
	CONSTRAINT [PK_ApplicationADRoles] PRIMARY KEY CLUSTERED 
(
	[app_id] ASC, [role_id] ASC, [ad_user_group] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [wds].[application_AD_roles] ADD  CONSTRAINT [FK_ApplicationADRoles_Roles] FOREIGN KEY ([role_id]) REFERENCES [wds].[roles]([role_id])
GO
ALTER TABLE [wds].[application_AD_roles] ADD  CONSTRAINT [FK_ApplicationUserAD_Applications] FOREIGN KEY ([app_id]) REFERENCES [wds].[Applications]([app_id])
GO


ALTER TABLE [wds].[application_AD_roles] ADD  CONSTRAINT [DF_aadr_is_active]  DEFAULT ((1)) FOR [is_active]
GO

ALTER TABLE [wds].[application_AD_roles] ADD  CONSTRAINT [DF_aadr_create_dt]  DEFAULT (getdate()) FOR [create_dt]
GO

ALTER TABLE [wds].[application_AD_roles] ADD  CONSTRAINT [DF_aadr_mod_dt]  DEFAULT (getdate()) FOR [mod_dt]
GO

ALTER TABLE [wds].[application_AD_roles] ADD  CONSTRAINT [DF_aadr_expire_dt]  DEFAULT (DateAdd(yy, +3, getdate())) FOR [expire_dt]
GO