/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) a.[apikey_id], d.name app_name, e.name role_name
      , b.host_name
      ,a.[name]
      ,a.[encrypted_apiKey]
      ,a.[host_mask]
      ,a.[description]
      ,a.[owner_email]
      ,a.[create_dt]
      ,a.[mod_dt]
      ,a.[expire_date]
      ,a.[is_active]
  FROM [wds].[WDS].[API_keys] a 
		INNER JOIN [WDS].[Hosts] b ON a.server_host_id = b.host_id
		INNER JOIN [WDS].[application_apikey_roles] c ON a.apikey_id = c.apikey_id
		INNER JOIN [WDS].applications d ON c.app_id = d.app_id
		INNER JOIN [WDS].roles e ON c.role_id = e.role_id
  WHERE a.[name] = 'SCKey'