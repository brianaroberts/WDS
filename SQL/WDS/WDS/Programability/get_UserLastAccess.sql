CREATE FUNCTION [wds].[get_UserLastAccess]
(
	@edipi int,
	@ptc char(1), 
	@application_name varchar(64)
)
RETURNS datetime
AS
BEGIN
	DECLARE @value DATETIME

	SET @value = (SELECT access_dt
	FROM
		[wds].[ApplicationAccessLog]
	WHERE
		app_id IN (SELECT app_id FROM Applications WHERE name = @application_name) AND edipi = @edipi AND ptc = @ptc)
		
	RETURN @value
END
