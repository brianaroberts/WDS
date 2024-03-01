CREATE TABLE [wds].[application_apikey_roles]
(
	[app_id] INT NOT NULL,
	[role_id] INT NOT NULL, 
    [apikey_id] INT NOT NULL,
    [create_dt] [datetime] CONSTRAINT [DF_aapir_create_dt]  DEFAULT getdate(),
	[mod_dt] [datetime] CONSTRAINT [DF_aapir_mod_dt]  DEFAULT getdate(),
	[expire_dt] [datetime] CONSTRAINT [DF_aapir_expire_dt]  DEFAULT DateAdd(yy, +3, getdate()),
	[is_active] [bit] NOT NULL CONSTRAINT [DF_aapir_is_active]  DEFAULT 1,
	CONSTRAINT [PK_ApplicationApikeyRoles] PRIMARY KEY CLUSTERED 
(
	[role_id] ASC, [apikey_id] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [wds].[application_apikey_roles] ADD  CONSTRAINT [FK_ApplicationApiKeyRoles_Roles] FOREIGN KEY ([role_id]) REFERENCES [wds].[roles]([role_id])
GO
ALTER TABLE [wds].[application_apikey_roles] ADD  CONSTRAINT [FK_ApplicationApiKeyRoles_Applications] FOREIGN KEY ([app_id]) REFERENCES [wds].[Applications]([app_id])
GO
ALTER TABLE [wds].[application_apikey_roles] ADD  CONSTRAINT [FK_ApplicationApiKeyRoles_Users] FOREIGN KEY ([apikey_id]) REFERENCES [wds].[API_Keys]([apikey_id])
GO
