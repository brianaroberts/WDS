CREATE FUNCTION [wds].[get_UserClaims]
(
	@edipi int,
	@application_name varchar(32)
)
RETURNS @returntable TABLE
(
	claim_type varchar(128),
	claim_value varchar(256)
)
AS
BEGIN
	INSERT @returntable
	SELECT claims.claim_type, claims.claim_value
	FROM
		[wds].[Users] users, 
		[wds].[user_claims] claims,
		[wds].[Applications] app
	WHERE
		app.name = @application_name AND users.edipi = @edipi
		AND app.app_id = claims.app_id AND users.user_id = claims.user_id		
	RETURN
END
