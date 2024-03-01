CREATE TABLE [wds].[applications](
	[app_id] [int] identity (1,1) NOT NULL,
	[name] [varchar](128) NOT NULL,
	[description] [varchar](128) NOT NULL,
	[owner] [varchar](128) NOT NULL,
	[create_dt] [datetime] NULL,
	[mod_dt] [datetime],
	[user_auth] [bit] NOT NULL, 
	[ad_auth] [bit] NOT NULL, 
	[is_active] [bit] NOT NULL,
 CONSTRAINT [PK_Configurations] PRIMARY KEY CLUSTERED 
(
	[app_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [Applications] ON [wds].[Applications] ([name],[is_active])
GO
ALTER TABLE [wds].[Applications] ADD  CONSTRAINT [DF_Applications_create_dt]  DEFAULT (getdate()) FOR [create_dt]
GO
ALTER TABLE [wds].[Applications] ADD  CONSTRAINT [DF_Applications_mod_dt]  DEFAULT (getdate()) FOR [mod_dt]
GO
ALTER TABLE [wds].[Applications] ADD  CONSTRAINT [DF_Applications_is_retired]  DEFAULT ((1)) FOR [is_active]
GO