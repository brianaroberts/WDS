CREATE VIEW [wds].[vapplication_roles]
AS
SELECT        WDS.applications.name AS app_name, WDS.roles.name AS role_name, WDS.roles.description, WDS.application_roles.create_dt, WDS.application_roles.is_active
FROM            WDS.application_roles INNER JOIN
                         WDS.applications ON WDS.application_roles.app_id = WDS.applications.app_id INNER JOIN
                         WDS.roles ON WDS.application_roles.role_id = WDS.roles.role_id
GO