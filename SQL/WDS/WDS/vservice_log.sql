CREATE VIEW [wds].[vservice_log]
AS
SELECT        WDS.service_log.instance_id, WDS.log_level_map.log_description, WDS.service_log.host, WDS.service_log.message, WDS.service_log.create_dt
FROM            WDS.log_level_map INNER JOIN
                         WDS.service_log ON WDS.log_level_map.log_level = WDS.service_log.log_level
GO