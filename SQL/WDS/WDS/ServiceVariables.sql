-- Currently for latency issues, the API keys are loaded by the WDS at launch, this is why there is a string compare with the allowed applications 
--	vs. a relational lockdown. 
CREATE TABLE [wds].[service_variables] (
	[server_host_id] INT NOT NULL,
	[var_name] varchar(64) NOT NULL,
	[var_value] varchar(max), 		
	[description] [varchar](512) NULL,
	[create_dt] [datetime],
	[mod_dt] [datetime],
	[expire_date] [datetime],
	[is_active] [bit] NOT NULL,
	CONSTRAINT [fk_service_variables_hostId] FOREIGN KEY ([server_host_id]) REFERENCES [wds].[hosts]([host_id]),
 CONSTRAINT [PK_service_variables] PRIMARY KEY CLUSTERED ([server_host_id],[var_name] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY])
GO

ALTER TABLE [wds].[service_variables] ADD  CONSTRAINT [DF_sv_create_dt]  DEFAULT (getdate()) FOR [create_dt]
GO
ALTER TABLE [wds].[service_variables] ADD  CONSTRAINT [DF_sv_mod_dt]  DEFAULT (getdate()) FOR [mod_dt]
GO
ALTER TABLE [wds].[service_variables] ADD  CONSTRAINT [DF_sv_expire_dt]  DEFAULT (getdate()+3650) FOR [expire_date]
GO
ALTER TABLE [wds].[service_variables] ADD  CONSTRAINT [DF_sv_is_active]  DEFAULT ((1)) FOR [is_active]
GO
