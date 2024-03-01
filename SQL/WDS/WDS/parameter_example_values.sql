CREATE TABLE [wds].[parameter_example_values]
(
	[example_set_id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [recordset_id] INT NOT NULL, 
    [set_name] varchar(64) NOT NULL, 
    [parameter_id] INT NOT NULL,     
    [value] VARCHAR(255) NULL, 
    CONSTRAINT [fk_parameter_example_values_rs] FOREIGN KEY ([recordset_id]) REFERENCES [wds].[recordsets]([recordset_id]), 
    CONSTRAINT [fk_parameter_example_values_p] FOREIGN KEY ([parameter_id]) REFERENCES [wds].[parameters]([parameter_id])
)
