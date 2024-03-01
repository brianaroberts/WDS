CREATE FUNCTION [wds].[get_UserRoles]
(
	@edipi int,
	@application_name varchar(64)
)
RETURNS @returntable TABLE
(
	role_name varchar(32)
)
AS
BEGIN
	INSERT @returntable
	SELECT roles.name
	FROM
		[wds].[Users] users, 
		[wds].[ApplicationUserRoles] aur,
		[wds].[Applications] app, 
		[wds].[application_roles] roles,
		[wds].[roles] 
	WHERE
		app.name = @application_name AND users.edipi = @edipi
		AND app.app_id = roles.app_id AND app.app_id = roles.app_id
		AND users.user_id = aur.user_id AND roles.role_id = aur.role_id
		AND roles.role_id = aur.role_id
	RETURN
END
