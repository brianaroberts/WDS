CREATE VIEW [wds].[vapplication_user_roles]
AS
SELECT        WDS.applications.name AS app_name, WDS.roles.name AS role_name, WDS.application_user_roles.user_id, WDS.application_user_roles.create_dt, WDS.application_user_roles.mod_dt, 
                         WDS.application_user_roles.expire_dt, WDS.application_user_roles.is_active
FROM            WDS.application_user_roles INNER JOIN
                         WDS.applications ON WDS.application_user_roles.app_id = WDS.applications.app_id INNER JOIN
                         WDS.roles ON WDS.application_user_roles.role_id = WDS.roles.role_id
GO