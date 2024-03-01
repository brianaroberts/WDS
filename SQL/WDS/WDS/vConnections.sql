CREATE VIEW [wds].[vConnections]
AS
SELECT        WDS.connections.connection_id, WDS.hosts.host_name, WDS.connections.name, WDS.connections.connection_type, WDS.connections.production, WDS.connections.staging, WDS.connections.development, 
                         WDS.connections.create_dt, WDS.connections.mod_dt, WDS.connections.is_active
FROM            WDS.connections INNER JOIN
                         WDS.hosts ON WDS.connections.host_id = WDS.hosts.host_id
GO