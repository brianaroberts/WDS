CREATE VIEW [wds].[vapplication_hosts]
AS
SELECT        WDS.applications.name AS app_name, WDS.hosts.host_name, WDS.hosts.description
FROM            WDS.application_hosts INNER JOIN
                         WDS.applications ON WDS.application_hosts.app_id = WDS.applications.app_id INNER JOIN
                         WDS.hosts ON WDS.application_hosts.host_id = WDS.hosts.host_id