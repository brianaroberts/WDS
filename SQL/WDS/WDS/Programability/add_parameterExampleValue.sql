CREATE PROCEDURE [WDS].[add_parameter_example_value]
	@appName varchar(128),
	@recordsetName varchar(128),
	@setName varchar(64), 
	@parameterName varchar(64), 
	@parameterValue varchar(64)
AS
	DECLARE @recordsetId INT
	DECLARE @paramId INT

	SELECT @recordsetId = [recordsets].[recordset_id] 
	FROM [wds].[recordsets] 		
	WHERE [recordsets].[name] = @recordsetName AND [recordsets].[app_id] IN (SELECT app_id from wds.applications WHERE name = @appName)

	SELECT @paramId = parameter_id FROM [wds].[parameters] WHERE [name] = @parameterName

	IF (@paramId Is Not null AND @recordsetId IS NOT null AND @setName IS NOT null)
	BEGIN
		IF NOT EXISTS (SELECT * FROM [wds].[parameter_example_values] WHERE recordset_id = @recordsetId AND set_name = @setName AND parameter_id = @paramId)
		BEGIN
			INSERT INTO [wds].[parameter_example_values] VALUES (@recordsetId, @setName, @paramId, @parameterValue)
		END
	END
RETURN 0