CREATE TABLE [wds].[parameters]
(
	[parameter_id] [int] identity (1,1) PRIMARY KEY NOT NULL,
	[name] varchar(64) NOT NULL, 
	[call_parameter_name] varchar(64) NOT NULL, 
	[required] bit not null, 
	[reg_ex_mask] varchar(255), 
	[default_value] varchar(255), 
	[replace_with] varchar(255), 
	[create_dt] [datetime],
	[mod_dt] [datetime],    
    [is_active] BIT NOT NULL
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [uk_recordset_name] ON [wds].[parameters] ([name],[is_active])
GO
ALTER TABLE [wds].[parameters] ADD  CONSTRAINT [df_parameters_is_active]  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [wds].[parameters] ADD  CONSTRAINT [df_parameters_required]  DEFAULT ((0)) FOR [required]
GO
ALTER TABLE [wds].[parameters] ADD  CONSTRAINT [df_parameters_create_dt]  DEFAULT (getdate()) FOR [create_dt]
GO
ALTER TABLE [wds].[parameters] ADD  CONSTRAINT [df_parameters_mod_dt]  DEFAULT (getdate()) FOR [mod_dt]
GO

