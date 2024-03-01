CREATE VIEW [wds].[v_application_log]
AS
SELECT WDS.application_log.client_host, WDS.application_log.server_host, WDS.application_log.application, WDS.application_log.log_area, WDS.log_level_map.log_description AS log_level, WDS.application_log.message, 
        WDS.application_log.create_date
FROM WDS.application_log INNER JOIN
        WDS.log_level_map ON WDS.application_log.log_level = WDS.log_level_map.log_level
GO