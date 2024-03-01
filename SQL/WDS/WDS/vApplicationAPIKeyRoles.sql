CREATE VIEW [wds].[vapplication_apikey_roles]
AS
SELECT        WDS.applications.name AS app_name, WDS.recordsets.name AS recordset_name, WDS.API_keys.name AS apiKey, WDS.hosts.host_name AS server_host, WDS.API_keys.host_mask, WDS.API_keys.owner_email, 
                         WDS.application_apikey_roles.create_dt, WDS.application_apikey_roles.mod_dt, WDS.application_apikey_roles.expire_dt, WDS.application_apikey_roles.is_active
FROM            WDS.application_apikey_roles INNER JOIN
                         WDS.applications ON WDS.application_apikey_roles.app_id = WDS.applications.app_id INNER JOIN
                         WDS.recordsets ON WDS.applications.app_id = WDS.recordsets.app_id INNER JOIN
                         WDS.API_keys ON WDS.application_apikey_roles.apikey_id = WDS.API_keys.apikey_id INNER JOIN
                         WDS.hosts ON WDS.API_keys.server_host_id = WDS.hosts.host_id
GO