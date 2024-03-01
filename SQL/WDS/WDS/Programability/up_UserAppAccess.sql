CREATE PROCEDURE [wds].[add_UserAppAccess]
(
	@edipi int,
	@ptc char(1), 
	@application_name varchar(64)
)
AS
BEGIN
	INSERT INTO [wds].ApplicationAccessLog 
	VALUES ((SELECT TOP 1 app_id FROM Applications WHERE name = @application_name), @edipi, @ptc, GetDate())
END
