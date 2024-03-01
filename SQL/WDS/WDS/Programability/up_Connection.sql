CREATE PROCEDURE [WDS].[up_connection]
	@hostName varchar(64),
	@name varchar(64),
	@connectionType varchar(32) = 'Database',
	@production varchar(256) = '',
	@staging varchar(256) = '',
	@development varchar(256) = '',
	@isActive bit = 1,
	@idOutput int OUTPUT

AS
BEGIN
	DECLARE @hostId INT; 

	EXEC [wds].[add_host] @hostName, @isActive, @hostId OUTPUT

	SELECT @idOutput = connection_id FROM [wds].[connections] WHERE [name] = @name
	
	IF @idOutput IS NULL
	BEGIN
		INSERT INTO [wds].[connections] values (@hostId, @name, @connectionType, @production, @staging, @development, default, default, @isActive)
		SET @idOutput = SCOPE_IDENTITY();
	END;
	ELSE
		
		UPDATE [wds].[connections] 
		SET [connection_type] = @connectionType, 
			[production] = @production, 
			[staging] = @staging, 
			[development] = @development,
			[is_active] = @isActive, 
			[mod_dt] = GETDATE()
		WHERE [name] = @name
END;