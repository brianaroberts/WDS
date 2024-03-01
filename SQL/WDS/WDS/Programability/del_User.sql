CREATE PROCEDURE [wds].[del_User]
	@userId int
AS
	IF (@userId <> 0)
	BEGIN
		UPDATE [wds].[application_user_roles] SET [is_active] = 0, [mod_dt] = getdate() WHERE user_id = @userId; 
		UPDATE [wds].[user_claims] SET [is_active] = 0, [mod_dt] = getdate() WHERE user_id = @userId;
		UPDATE [wds].[users] SET [is_active] = 0, [mod_dt] = getdate() WHERE user_id = @userId;
	END; 

RETURN 0
