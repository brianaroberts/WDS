CREATE PROCEDURE [WDS].[add_Recordset]
	@hostName varchar(128),
	@appName varchar(128),
	@recordsetName varchar(64),
	@substituteName varchar(64),
	@connectionName varchar(64), 
	@roleName varchar(64),
	@call varchar(4000) = '',
	@callTypeId INT,
	@returnTypeId INT,
	@returnTypeDetailsId INT,
	@cache bit, 
	@cacheTimeOut INT, 
	@endPointRequired bit,
	@logCall bit = 0
AS
	DECLARE @appID INT
	
	DECLARE @connectionId INT
	DECLARE @accessRoleId INT
	
	SELECT @appID = app_id FROM [wds].[Applications] WHERE [name] = @appName 	
	SELECT @connectionId = connection_id from [wds].[Connections] WHERE [name] = @connectionName

	IF @connectionId Is null
	BEGIN
		EXEC [wds].[up_Connection] @hostName, @connectionName, 'Database', default, default, default, default, @idOutput = @connectionId Output
	END;
	
	EXEC [wds].[add_application_role] @appID, @roleName, 'NEEDS DESCRIPTION', @accessRoleId OUTPUT

	IF NOT EXISTS (SELECT * FROM [wds].[recordsets] WHERE [name] = @recordsetName AND [is_active] = 1) AND (@accessRoleId > 0 AND @accessRoleId IS NOT NULL)
	BEGIN
		IF NOT EXISTS  (SELECT * FROM [wds].[recordsets] WHERE [name] = @recordsetName)
			INSERT INTO [wds].[recordsets] VALUES (@appID, @recordsetName, @substituteName, @accessRoleId, @connectionId, @callTypeId, @returnTypeId, @returnTypeDetailsId, @call, @cache, @cacheTimeOut, @endPointRequired, @logCall, default, default, 1)
		ELSE
			UPDATE [wds].[recordsets] SET [is_active] = 1, [mod_dt] = getdate() WHERE [name] = @recordsetName
	END
		
		
RETURN 0