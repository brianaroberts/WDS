-- Leaving the testing tables and data
USE WDS

-- Dropping Tables in the correct order
DROP TABLE [wds].[application_access_log]
DROP TABLE [wds].[application_log]
DROP TABLE [wds].[service_log]
DROP TABLE [wds].[userchanges_log]
	  
DROP TABLE [wds].[recordset_parameter_map]
DROP TABLE [wds].[parameter_example_values]
DROP TABLE [wds].[parameters]
DROP TABLE [wds].[recordsets]
	  
DROP TABLE [wds].[application_roles]
DROP TABLE [wds].[application_hosts]
DROP TABLE [wds].[application_user_roles]
DROP TABLE [wds].[application_apikey_roles]
DROP TABLE [wds].[application_AD_roles]
DROP TABLE [wds].[applications]
DROP TABLE [wds].[roles]
	  
DROP TABLE [wds].[connections]
DROP TABLE [wds].[service_variables]
DROP TABLE [wds].[users]
DROP TABLE [wds].[user_claims]
DROP TABLE [wds].[API_Keys]
DROP TABLE [wds].[hosts]

DROP TABLE [wds].[returnType_map]
DROP TABLE [wds].[returnType_details_map]
DROP TABLE [wds].[callType_map]
DROP TABLE [wds].[log_level_map]


