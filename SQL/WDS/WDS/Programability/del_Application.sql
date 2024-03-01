CREATE PROCEDURE [WDS].[del_Application]
	@app_id int = 0, 
	@app_name varchar(256) = ''
	
AS
	IF LEN(@app_name) > 2 
		SELECT @app_id = app_id FROM [wds].[applications] WHERE [name] = @app_name 
	ELSE
		SELECT @app_name = [name] FROM [wds].[applications] WHERE [app_id] = @app_id
	
	IF @app_id > 0
	BEGIN
		DECLARE @recordset_id INT
		-- Gather all the parameter ids before disassociation. 
		DECLARE curs CURSOR LOCAL FAST_FORWARD FOR
			SELECT recordset_id FROM [wds].[recordsets] WHERE app_id = @app_id

		OPEN curs

		FETCH NEXT FROM curs INTO @recordset_id

		WHILE @@FETCH_STATUS = 0 BEGIN
			EXEC [wds].[del_recordset]  @recordset_id
			FETCH NEXT FROM curs INTO @recordset_id
		END

		CLOSE curs
		DEALLOCATE curs

		UPDATE [wds].[application_roles] SET [is_active] = 0, [mod_dt] = getdate() WHERE app_id = @app_id
		UPDATE [wds].[application_ad_roles] SET [is_active] = 0, [mod_dt] = getdate() WHERE app_id = @app_id
		UPDATE [wds].[application_user_roles] SET [is_active] = 0, [mod_dt] = getdate() WHERE app_id = @app_id
		UPDATE [wds].[application_apikey_roles] SET [is_active] = 0, [mod_dt] = getdate() WHERE app_id = @app_id
		UPDATE [wds].[application_hosts] SET [is_active] = 0, [mod_dt] = getdate() WHERE app_id = @app_id
		
		-- Cleanup the user table
		UPDATE [wds].[users] SET [is_active] = 0, [mod_dt] = getdate() WHERE [user_id] NOT IN (SELECT DISTINCT USER_ID FROM [wds].[application_user_roles])
		UPDATE [wds].[API_keys] SET [is_active] = 0, [mod_dt] = getdate() WHERE [apikey_id] NOT IN (SELECT DISTINCT apikey_id FROM [wds].[application_apikey_roles])

		-- Finally remove the application
		UPDATE [wds].[applications] SET [is_active] = 0, [mod_dt] = getdate() WHERE app_id = @app_id
		UPDATE [wds].[roles] SET [is_active] = 0, [mod_dt] = getdate() WHERE [name] LIKE (@app_name + '%')

		-- Clean up Connection
		UPDATE [wds].[connections] SET [is_active] = 0, [mod_dt] = getdate() WHERE connection_id NOT IN (select distinct connection_id from recordsets)
	END; 
RETURN 0