CREATE PROCEDURE [WDS].[get_Connections]
	@hostName varchar(64) = null,
	@connectionName varchar(64) = null
AS
SELECT * from [wds].[Connections] 
WHERE is_active = 1 AND host_id IN (SELECT host_id 
									FROM [wds].[HOSTS] 
									WHERE (@hostName is null OR host_name in (@hostName, '*')) AND (@connectionName is NULL OR [name] = @connectionName) and is_active = 1)