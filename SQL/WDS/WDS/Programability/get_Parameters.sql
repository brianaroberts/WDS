CREATE PROCEDURE [wds].[get_parameters]
	@appName varchar(64),
	@recordsetName varchar(64)
AS
SELECT [parameters].[parameter_id], [parameters].name, [parameters].call_parameter_name, [parameters].required, [parameters].reg_ex_mask, [parameters].default_value, [parameters].replace_with
FROM [WDS].[parameters] 
	INNER JOIN [wds].[recordset_parameter_map] ON [recordset_parameter_map].[parameter_id] = [parameters].[parameter_id] AND [recordset_parameter_map].[is_active] = 1
	INNER JOIN [wds].[recordsets] ON [recordsets].[recordset_id] = [recordset_parameter_map].[recordset_id] AND [recordsets].[is_active] = 1
WHERE [parameters].[is_active] = 1  AND [recordsets].[name] = @recordsetName AND [recordsets].app_id IN (SELECT app_id from [wds].[applications] WHERE [name] = @appName);