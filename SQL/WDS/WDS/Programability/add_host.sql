CREATE PROCEDURE [wds].[add_host]
	@hostName varchar(64),
	@isActive bit = 1,
	@hostId INT OUTPUT
AS
	

	IF NOT EXISTS (SELECT * FROM [wds].[hosts] WHERE [host_name] = @hostName)
		INSERT INTO [wds].[hosts] values (@hostName, 'Please provide a description', @isActive, default)
	ELSE
		UPDATE [wds].[hosts] SET [is_active] = 1, [mod_dt] = getdate() WHERE [host_name] = @hostName

	SELECT @hostId = [host_id] FROM [wds].[hosts] WHERE [host_name] = @hostName
RETURN 0
