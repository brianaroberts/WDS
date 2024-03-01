CREATE PROCEDURE [WDS].[add_APIKey]
	@serverHost varchar(64),
	@name varchar(64),
	@encryptedAPIKey varchar(128),
	@hostMask varchar(256) = '',
	@description varchar(256) = '',
	@ownerEmail varchar(256) = '',
	@isActive bit = 1
AS
BEGIN
	DECLARE @hostId INT; 
	EXEC [wds].[add_host] @serverHost, @isActive, @hostId OUTPUT

	IF NOT EXISTS (SELECT * FROM [wds].[API_keys] WHERE [name] = @name)
		INSERT INTO [wds].[API_keys] values (@hostId, @name, @encryptedAPIKey, @hostMask, @description, @ownerEmail, default, default, default, @isActive)
	ELSE 
		UPDATE [wds].[API_Keys] SET [is_active] = 1, [mod_dt] = getdate() WHERE [name] = @name
END;