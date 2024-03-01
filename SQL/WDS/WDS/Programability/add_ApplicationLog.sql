CREATE PROCEDURE [WDS].[add_ApplicationLog]
	@clientHost varchar(64),
	@serverHost varchar(64),
	@application varchar(64),
	@logArea varchar(64), 
	@logLevel varchar(1),	
	@message varchar(4000)
AS
	INSERT INTO [wds].[application_log] VALUES (@clientHost, @serverHost, @application, @logArea, CONVERT(INT,@logLevel), @message, GETDATE())
RETURN 0