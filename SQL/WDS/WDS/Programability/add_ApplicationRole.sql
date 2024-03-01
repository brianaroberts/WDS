CREATE PROCEDURE [WDS].[add_application_role]
	@appId INT,
	@roleName varchar(32),
	@roleDescription varchar(255),
	@roleId INT OUTPUT
AS
	SELECT @roleId = role_id FROM [wds].[roles] WHERE [name] = @roleName 
	IF (@roleId IS NULL)
	BEGIN
		INSERT INTO [wds].[roles] values (@roleName, @roleDescription, default, default, default, 1)
		SET @roleId = SCOPE_IDENTITY();
	END
	ELSE
		UPDATE [wds].[roles] SET [is_active] = 1, [mod_dt] = getdate() WHERE [name] = @roleName

	IF NOT EXISTS (SELECT * FROM [wds].[application_roles] WHERE [role_id] = @roleId AND [app_id] = @appId)
		INSERT INTO [wds].[application_roles] VALUES (@roleId, @appID, default, default, 1)	
	ELSE
		UPDATE [wds].[application_roles] SET [is_active] = 1, [mod_dt] = getdate() WHERE [role_id] = @roleId AND [app_id] = @appId
	
RETURN 0