CREATE PROCEDURE [WDS].[add_parameter]	
	@parameterName varchar(64), 
	@callParameterName varchar(64), 
	@required bit, 
	@regExMask varchar(255) = '', 
	@defaultValue varchar(255) = '', 
	@replaceWith varchar(255) = '', 
	@isActive bit = 1, 
	@appName varchar(64) = '',
	@recordsetName varchar(64) = ''
AS
	DECLARE @parameterID INT;
	SELECT @parameterID = parameter_id FROM [wds].[parameters] WHERE [name] = @parameterName

	IF (@parameterID IS NULL)
	BEGIN	
		INSERT INTO [wds].[parameters] VALUES (@parameterName, @callParameterName, @required, @regExMask, @defaultValue, @replaceWith, default, default, @isActive)
		SET @parameterID = SCOPE_IDENTITY()		
	END
	ELSE 
		UPDATE [wds].[parameters] SET [is_active] = 1, [mod_dt] = getdate() WHERE [name] = @parameterName
	
	IF (@appName <> '' AND @recordsetName <> '')
	BEGIN
		DECLARE @appID INT
		DECLARE @recordsetID INT
		
		SELECT @appID = app_id FROM [wds].[Applications] WHERE NAME = @appName
		IF @appId IS NOT NULL
		BEGIN
			SELECT @recordsetID = recordset_id FROM [wds].[recordsets] WHERE app_id = @appID AND name = @recordsetName
			IF @recordsetID IS NOT NULL
			BEGIN
				IF NOT EXISTS (SELECT * FROM [wds].[recordset_parameter_map] WHERE recordset_id = @recordsetID and @parameterID = @parameterID)
					INSERT INTO [wds].[recordset_parameter_map] VALUES (@recordsetID, @parameterID, default, default, @isActive)
				ELSE 
					UPDATE [wds].[recordset_parameter_map] SET [is_active] = 1, [mod_dt] = getdate() WHERE recordset_id = @recordsetID and @parameterID = @parameterID
			END
		END
	END
RETURN 0