CREATE VIEW [wds].[vRecordsets]
AS
SELECT        WDS.applications.name AS app_name, WDS.recordsets.name AS recordset_name, WDS.recordsets.statement, WDS.recordsets.cache, WDS.recordsets.cache_timeout, WDS.recordsets.endpoint_required, 
                         WDS.connections.name AS connection, WDS.callType_map.name AS call_type, WDS.returnType_details_map.name AS return_type_details, WDS.returnType_map.name AS return_type, WDS.recordsets.create_dt, 
                         WDS.recordsets.mod_dt, WDS.recordsets.is_active
FROM            WDS.recordsets INNER JOIN
                         WDS.applications ON WDS.recordsets.app_id = WDS.applications.app_id INNER JOIN
                         WDS.application_roles ON WDS.recordsets.access_role_id = WDS.application_roles.role_id AND WDS.applications.app_id = WDS.application_roles.app_id INNER JOIN
                         WDS.connections ON WDS.recordsets.connection_id = WDS.connections.connection_id INNER JOIN
                         WDS.callType_map ON WDS.recordsets.callType_id = WDS.callType_map.callType_id INNER JOIN
                         WDS.returnType_details_map ON WDS.recordsets.returnType_details_id = WDS.returnType_details_map.details_Id INNER JOIN
                         WDS.returnType_map ON WDS.recordsets.returnType_id = WDS.returnType_map.returnType_Id
GO