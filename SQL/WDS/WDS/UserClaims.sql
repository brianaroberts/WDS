CREATE TABLE [wds].[user_claims](
	[user_claim_id] [int] identity (1,1) NOT NULL,
	[app_id] [int] NOT NULL,
	[user_id] [int] NOT NULL,
	[claim_type] [nvarchar](128) NOT NULL,
	[claim_value] [nvarchar](256) NOT NULL,
	[is_active] [bit] NOT NULL,
 [mod_dt] DATETIME NULL, 
    CONSTRAINT [PK_UserClaims] PRIMARY KEY CLUSTERED 
(
	[user_claim_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
GO

ALTER TABLE [wds].[user_claims] ADD  CONSTRAINT [FK_UserClaimes_Users] FOREIGN KEY ([user_id]) REFERENCES [wds].[users]([user_id])
GO
ALTER TABLE [wds].[user_claims] ADD  CONSTRAINT [FK_UserClaimes_AppID] FOREIGN KEY ([app_id]) REFERENCES [wds].[applications]([app_id])
GO

ALTER TABLE [wds].[user_claims] ADD  CONSTRAINT [DF_User_Claims_is_approved]  DEFAULT ((1)) FOR [is_active]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Row identifier' , @level0type=N'SCHEMA',@level0name=N'wds', @level1type=N'TABLE',@level1name=N'user_claims', @level2type=N'COLUMN',@level2name=N'user_claim_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User row identifier' , @level0type=N'SCHEMA',@level0name=N'wds', @level1type=N'TABLE',@level1name=N'user_claims', @level2type=N'COLUMN',@level2name=N'user_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Defined claim type' , @level0type=N'SCHEMA',@level0name=N'wds', @level1type=N'TABLE',@level1name=N'user_claims', @level2type=N'COLUMN',@level2name=N'claim_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Value of claim type as pertains to user' , @level0type=N'SCHEMA',@level0name=N'wds', @level1type=N'TABLE',@level1name=N'user_claims', @level2type=N'COLUMN',@level2name=N'claim_value'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User claims managment; specified by the application to manage roles, permissions, and any other application determined objectives to an individual' , @level0type=N'SCHEMA',@level0name=N'wds', @level1type=N'TABLE',@level1name=N'user_claims'
GO