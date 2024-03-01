CREATE VIEW [wds].[vapplication_access_log]
AS
SELECT        WDS.application_access_log.log_id, WDS.applications.name AS application, WDS.recordsets.name AS recordset, WDS.application_access_log.client_host, WDS.application_access_log.server_host, 
                         WDS.application_access_log.api_key, WDS.application_access_log.edipi, WDS.application_access_log.ptc, WDS.application_access_log.parameters, WDS.application_access_log.access_dt
FROM            WDS.application_access_log INNER JOIN
                         WDS.recordsets ON WDS.application_access_log.recordset_id = WDS.recordsets.recordset_id INNER JOIN
                         WDS.applications ON WDS.application_access_log.app_id = WDS.applications.app_id AND WDS.recordsets.app_id = WDS.applications.app_id
GO