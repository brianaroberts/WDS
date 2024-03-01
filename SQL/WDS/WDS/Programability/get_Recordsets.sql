CREATE PROCEDURE [wds].[get_Recordsets]
	@appName varchar(64)
AS
SELECT 
	[recordset_id],[applications].[name] application,[recordsets].[name] ,[roles].[name] as role,[connection_id],[callType_id],[returnType_id],[returnType_details_id],[statement],[cache],[cache_timeout],[endpoint_required],[recordsets].[create_dt],[recordsets].[mod_dt],[recordsets].[is_active]  
FROM [WDS].[WDS].[recordsets] 
	INNER JOIN [wds].[application_roles] ON [recordsets].access_role_id = [application_roles].role_id and [application_roles].[is_active] = 1
	INNER JOIN [wds].[roles] ON [application_roles].[role_id] = [roles].[role_id] and [roles].[is_active] = 1
	INNER JOIN [wds].[applications] ON [applications].[app_id] = [recordsets].[app_id] AND [applications].[is_active] = 1
WHERE [recordsets].[app_id] IN (SELECT app_id FROM [wds].[applications] WHERE [name]=@appName AND [is_active] = 1)