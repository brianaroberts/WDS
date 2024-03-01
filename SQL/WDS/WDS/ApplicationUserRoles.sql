CREATE TABLE [wds].[application_user_roles]
(
	[app_id] INT NOT NULL, 
    [role_id] INT NOT NULL, 
    [user_id] INT NOT NULL,
    [create_dt] [datetime] CONSTRAINT [DF_aur_create_dt]  DEFAULT getdate(),
	[mod_dt] [datetime] CONSTRAINT [DF_aur_mod_dt]  DEFAULT getdate(),
	[expire_dt] [datetime] CONSTRAINT [DF_aur_expire_dt]  DEFAULT DateAdd(yy, +3, getdate()),
	[is_active] [bit] NOT NULL CONSTRAINT [DF_aur_is_active]  DEFAULT 1,
	CONSTRAINT [PK_ApplicationUserRoles] PRIMARY KEY CLUSTERED 
(
	[app_id] ASC, [role_id] ASC, [user_id] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [wds].[application_user_roles] ADD  CONSTRAINT [FK_ApplicationUserRoles_Roles] FOREIGN KEY ([role_id]) REFERENCES [wds].[roles]([role_id])
GO
ALTER TABLE [wds].[application_user_roles] ADD  CONSTRAINT [FK_ApplicationUserRoles_Applications] FOREIGN KEY ([app_id]) REFERENCES [wds].[Applications]([app_id])
GO
ALTER TABLE [wds].[application_user_roles] ADD  CONSTRAINT [FK_ApplicationUserRoles_Users] FOREIGN KEY ([user_id]) REFERENCES [wds].[Users]([user_id])
GO
