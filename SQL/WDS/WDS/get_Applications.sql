CREATE PROCEDURE [WDS].[get_Applications]
	@hostName varchar(64)
AS
SELECT * 
FROM 
	[wds].[Applications] 
WHERE is_active = 1 AND app_id IN (SELECT app_id FROM [wds].[application_hosts] 
									WHERE host_id IN (SELECT host_id 
														FROM [wds].[hosts] 
														WHERE host_name = @hostName))
