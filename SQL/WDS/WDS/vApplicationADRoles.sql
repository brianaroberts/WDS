CREATE VIEW [wds].[vapplication_ad_roles]
AS
SELECT        WDS.applications.name AS app_name, WDS.roles.name AS role_name, WDS.application_AD_roles.ad_user_group, WDS.application_AD_roles.create_dt, WDS.application_AD_roles.mod_dt, 
                         WDS.application_AD_roles.expire_dt, WDS.application_AD_roles.is_active
FROM            WDS.application_AD_roles INNER JOIN
                         WDS.applications ON WDS.application_AD_roles.app_id = WDS.applications.app_id INNER JOIN
                         WDS.roles ON WDS.application_AD_roles.role_id = WDS.roles.role_id
GO