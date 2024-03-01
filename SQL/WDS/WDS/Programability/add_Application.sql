CREATE PROCEDURE [WDS].[add_application]
	@hostName varchar(64), 
	@name varchar(128),
	@description varchar(128),
	@owner varchar(128)
AS
	DECLARE @appId INT
	DECLARE @hostId INT

	SELECT @hostId = [host_id] FROM [wds].[hosts] WHERE [host_name] = @hostName
	IF (@hostId = null)
		EXEC add_host @hostName, 1, @hostId OUTPUT
	
	BEGIN TRAN
	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [wds].[applications] WHERE [name] = @name)
			INSERT INTO [wds].[applications] VALUES (@name, @description, @owner, default, default, 1, 0, 1)
		ELSE
			UPDATE [wds].[applications] SET [is_active] = 1, [mod_dt] = getdate() WHERE [name] = @name

		SELECT @appId = [app_id] FROM [wds].[applications] WHERE [name] = @name
	
		IF NOT EXISTS (SELECT * FROM [wds].[application_roles] WHERE app_id = @appId)
		BEGIN
			INSERT INTO [wds].[roles] VALUES (@name + '.Admin', 'Administrator', default, default, default, 1)
			INSERT INTO [wds].[application_roles] VALUES (SCOPE_IDENTITY(), @appId, default, default, 1)
			INSERT INTO [wds].[roles] VALUES (@name + '.Write', 'Allowed to write data', default, default, default, 1)
			INSERT INTO [wds].[application_roles] VALUES (SCOPE_IDENTITY(), @appId, default, default, 1)
			INSERT INTO [wds].[roles] VALUES (@name + '.Read', 'Allowed to read data', default, default, default, 1)
			INSERT INTO [wds].[application_roles] VALUES (SCOPE_IDENTITY(), @appId, default, default, 1)
			
		END
		ELSE
		BEGIN
			UPDATE [wds].[roles] SET [is_active] = 1, [mod_dt] = getdate() WHERE [name] like @name + '%'
			UPDATE [wds].[application_roles] SET [is_active] = 1, [mod_dt] = getdate() WHERE [app_id] = @appId
		END
			

		DECLARE @roleList varchar(max) = @name + '.Admin,' + @name + '.Write,' + @name + '.Read';
		EXEC [wds].[add_APIKey_application_role] 'Admin', @name, @roleList

		IF NOT EXISTS (SELECT * FROM [wds].[application_hosts] WHERE [app_id] = @appId AND [host_id] = @hostId)
			INSERT INTO [wds].[application_hosts] VALUES (@appId, @hostId, default, 1)
		ELSE 
			UPDATE [wds].[application_hosts] SET [is_active] = 1, [mod_dt] = getdate() WHERE [app_id] = @appId AND [host_id] = @hostId

		COMMIT TRAN
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
	END CATCH
RETURN 0
