CREATE PROCEDURE [WDS].[add_ApplicationAccessLog]
	@clientHost varchar(64),
	@serverHost varchar(64),
	@appName varchar(64), 
	@recordsetName varchar(64), 
	@apiKey varchar(64), 
	@edipi INT, 
	@ptc char(1),
	@parameters varchar(max)
AS
	DECLARE @appID INT
	DECLARE @recordsetID INT

	SELECT @appID = app_id FROM [wds].[applications] WHERE [name] = @appName
	SELECT @recordsetID = [recordset_id] FROM [wds].[recordsets] WHERE [name] = @recordsetName 

	INSERT INTO [wds].[application_access_log] VALUES (@appID, @recordsetID, @clientHost, @serverHost, @apiKey, @edipi, @ptc, @parameters, GETDATE())
RETURN 0