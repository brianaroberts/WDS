/*  We decided to require EDIPI for now until the need arises for an alternate id. 
	We will add
*/
CREATE TABLE [wds].[users](
	[user_id] [int] identity (1,1) NOT NULL,
	[edipi] [int] NOT NULL,
	[ptc] [char](1) NOT NULL,
	[add_dt] [datetime], 
	[mod_dt] DATETIME NULL, 
	[is_active] [bit] NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [wds].[users] ADD  CONSTRAINT [DF_Users_is_retired]  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [wds].[users] ADD  CONSTRAINT [df_users_add_dt]  DEFAULT (getdate()) FOR [add_dt]
GO
ALTER TABLE [wds].[users] ADD  CONSTRAINT [df_users_mod_dt]  DEFAULT (getdate()) FOR [mod_dt]
GO


