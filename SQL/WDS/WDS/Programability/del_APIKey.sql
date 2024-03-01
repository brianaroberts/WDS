CREATE PROCEDURE [wds].[del_APIKey]
	@apiKeyId INT,
	@apiKeyName varchar(64)
AS
	If (@apiKeyId IS Null AND @apiKeyName IS NOT NULL)
		SELECT @apiKeyId = [apikey_id] FROM [wds].[API_keys] WHERE [name] = @apiKeyName
	IF (@apiKeyId IS NOT null) 
	BEGIN
		UPDATE [wds].[application_apikey_roles] SET [is_active] = 0, [mod_dt] = getdate() WHERE apikey_id = @apiKeyId; 
		UPDATE [wds].[API_keys] SET [is_active] = 0, [mod_dt] = getdate() WHERE apikey_id = @apiKeyId; 
	END; 
RETURN 0
