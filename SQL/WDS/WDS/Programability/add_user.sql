CREATE PROCEDURE [wds].[add_user]
	@edipi int,
	@ptc char(1),
	@app_name varchar(256) = '',
	@role_name varchar(256) = ''
AS
	DECLARE @user_id INT
	IF NOT EXISTS (SELECT * FROM [wds].[users] WHERE edipi = @edipi)
	BEGIN
		INSERT INTO [wds].[users] VALUES (@edipi, @ptc, default, default, 1)
		-- First Time seeing user, add default roles. 
		EXEC [wds].[add_user] @edipi, @ptc, 'WDS', 'WDS.Read'
	END; 
	ELSE
		UPDATE [wds].[users] SET [is_active] = 1, [mod_dt] = getdate() WHERE edipi = @edipi

	SET @user_id = SCOPE_IDENTITY();

	if (@app_name <> '' AND @role_name <> '')
	BEGIN
		DECLARE @app_id INT 
		DECLARE @role_id INT
		SELECT @app_id = app_id FROM [wds].[applications] WHERE [name] = @app_name
		SELECT @role_id = role_id FROM [wds].[roles] WHERE [name] = @role_name
				
		IF NOT EXISTS (SELECT * FROM [wds].[application_user_roles] WHERE app_id = @app_id AND role_id = @role_id AND user_id = @user_id)
		BEGIN
			INSERT INTO [wds].[application_user_roles] VALUES (@app_id, @role_id, @user_id, default, default, default, 1)
		END;
		ELSE
			UPDATE [wds].[application_user_roles] SET [is_active] = 1, [mod_dt] = getdate() WHERE app_id = @app_id AND role_id = @role_id AND user_id = @user_id
	END; 
RETURN 0
