CREATE TABLE [wds].[recordset_parameter_map]
(
	[recordset_id] INT NOT NULL,
	[parameter_id] INT NOT NULL,
	[create_dt] [datetime],
	[mod_dt] [datetime],    
    [is_active] BIT NOT NULL, 
	PRIMARY KEY (recordset_id, parameter_id),
    CONSTRAINT [FK_recordset_parameter_map_parameters] FOREIGN KEY ([parameter_id]) REFERENCES [wds].[parameters]([parameter_id]),
	CONSTRAINT [FK_recordset_parameter_map_recordset] FOREIGN KEY ([recordset_id]) REFERENCES [wds].[recordsets]([recordset_id])
)
GO 
ALTER TABLE [wds].[recordset_parameter_map] ADD  CONSTRAINT [df_recordsetparametermap_is_active]  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [wds].[recordset_parameter_map] ADD  CONSTRAINT [df_recordsetparametermap_create_dt]  DEFAULT (getdate()) FOR [create_dt]
GO
ALTER TABLE [wds].[recordset_parameter_map] ADD  CONSTRAINT [df_recordsetparametermap_mod_dt]  DEFAULT (getdate()) FOR [mod_dt]
GO
