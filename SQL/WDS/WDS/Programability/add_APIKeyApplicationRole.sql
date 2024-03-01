CREATE PROCEDURE [WDS].[add_APIKey_application_role]
	@apiKeyName varchar(64),
	@appName varchar(64), 
	@roles varchar(max)

AS
	DECLARE @roleTable TABLE (role_id INT, role varchar(64))
	DECLARE @APIKeyId INT
	DECLARE @AppId INT
	SELECT @APIKeyId = apikey_id FROM [wds].[API_keys] WHERE [name] = @apiKeyName
	SELECT @AppId = app_id FROM [wds].[applications] WHERE [name] = @appName

	INSERT INTO @roleTable 
		SELECT [role_id], [value] role FROM STRING_SPLIT(@roles, ',')  
			INNER JOIN [wds].[roles]  ON [name] = [value]
		
	IF (@APIKeyId > 0 AND @AppId > 0) 
	BEGIN
		INSERT INTO [wds].[application_apikey_roles] 
			SELECT @AppId, role_id, @APIKeyId, getdate(), getdate(), getdate()+730, 1 
			FROM @roleTable
			WHERE (CONVERT(varchar(10), @AppId) + '-' + CONVERT(varchar(10), role_id)) NOT IN (SELECT (CONVERT(varchar(10), app_id) + '-' + CONVERT(varchar(10), role_id)) FROM [wds].[application_apikey_roles] WHERE apikey_id = @APIKeyId)
	END
		UPDATE [wds].[application_apikey_roles] SET [is_active] = 1, [mod_dt] = getdate() WHERE [app_id] = @AppId AND [apikey_id] = @APIKeyId
RETURN 0