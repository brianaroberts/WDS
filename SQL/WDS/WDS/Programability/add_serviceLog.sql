CREATE PROCEDURE [WDS].[add_serviceLog]
	@instanceId varchar(128),
	@logLevel varchar(128),
	@host varchar(64),
	@message varchar(64)
AS
	INSERT INTO [wds].[service_log] VALUES(@instanceId, @logLevel, @host, @message, default);
RETURN 0
