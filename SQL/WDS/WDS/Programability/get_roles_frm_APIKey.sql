CREATE PROCEDURE [wds].[get_roles_frm_APIKey]
	@apiKeyId INT
AS
	SELECT [roles].[name] 
	FROM [wds].[application_apikey_roles]
		INNER JOIN [wds].[roles] ON [application_apikey_roles].role_id = [roles].[role_id]
	WHERE [apikey_id] = @apiKeyId;
