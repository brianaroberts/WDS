USE WDS;
-- Call settings
SELECT * FROM [wds].[vconnections]
SELECT * FROM [wds].[applications]
SELECT * FROM [wds].[recordsets]
SELECT * FROM [wds].[application_roles]
SELECT * FROM [wds].[parameters]

-- Permissions
SELECT * FROM [wds].[vapplication_hosts]
SELECT * FROM [wds].[vapplication_apikey_roles]
SELECT * FROM [wds].[vapplication_user_roles]
SELECT * FROM [wds].[vapplication_AD_roles]

-- Logging
SELECT * FROM [WDS].[application_log]
SELECT * FROM [wds].[vapplication_access_log]

