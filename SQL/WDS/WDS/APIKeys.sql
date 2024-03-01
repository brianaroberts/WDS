-- Currently for latency issues, the API keys are loaded by the WDS at launch, this is why there is a string compare with the allowed applications 
--	vs. a relational lockdown. 
CREATE TABLE [WDS].[API_keys](
	[apikey_id] [int] IDENTITY(1,1) NOT NULL,
	[server_host_id] [int] NOT NULL, 
	[name] varchar(64) NOT NULL, 
	[encrypted_apiKey] [varchar](128) NOT NULL,	
	[host_mask] [varchar](128) NULL,
	[description] [varchar](512) NULL,
	[owner_email] [varchar](512) NULL,
	[create_dt] [datetime],
	[mod_dt] [datetime],
	[expire_date] [datetime],
	[is_active] [bit] NOT NULL,
 CONSTRAINT [PK_APIKeys] PRIMARY KEY CLUSTERED 
(
	[apikey_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_APIKeys_encrypted_apiKey] UNIQUE NONCLUSTERED 
(
	[encrypted_apiKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
	CONSTRAINT [fk_APIKeys_host_id] FOREIGN KEY ([server_host_id]) REFERENCES [wds].[hosts]([host_id])
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [uk_apikeys_name] ON [wds].[API_keys] ([server_host_id],[name])
GO
ALTER TABLE [WDS].[API_keys] ADD  CONSTRAINT [DF_APIKeys_create_dt]  DEFAULT (getdate()) FOR [create_dt]
GO
ALTER TABLE [WDS].[API_keys] ADD  CONSTRAINT [DF_APIKeys_mod_dt]  DEFAULT (getdate()) FOR [mod_dt]
GO
ALTER TABLE [WDS].[API_keys] ADD  CONSTRAINT [DF_APIKeys_expire_dt]  DEFAULT (getdate()+730) FOR [expire_date]
GO
ALTER TABLE [WDS].[API_keys] ADD  CONSTRAINT [DF_APIKeys_is_active]  DEFAULT ((0)) FOR [is_active]
GO
