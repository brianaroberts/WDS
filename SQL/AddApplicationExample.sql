USE WDS

DECLARE @appName varchar(64) = 'FakeApp'
DECLARE @roles varchar(255) = @appName + '.Read,WDS.Read'
DECLARE @keyName varchar(64) = 'SCKey'
DECLARE @hostName varchar(64) = 'HQNZTK923404'
DECLARE @ownerEmail varchar(64) = 'brianaroberts@live.com'
DECLARE @apiEKey varchar(64) = 'fD/Bpn1tH7ynKiKY3+ni6KHwXE3gF4eAFUgCs30v6nJba+cF5PqkhavoFocAnKJq'
-- API KEY: 856c39a4-8e71-49d6-902d-4f692b6b5c89
DECLARE @connectionId INT

-- DELETE FROM [wds].[application_access_log]

-- Delete all this crap -Moin
EXEC [wds].[del_Application] 0, @appName

-- API Key
EXEC [wds].[add_APIKey] @hostName, @keyName, @apiEKey, 'localhost', 'This key will allow FakeApp Read communication from localhost.', @ownerEmail, 1
-- Add Connection
EXEC [wds].[up_connection] @hostName, @appName, 'Database', 'Data Source=dbProd;Initial Catalog=FakeApp;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true', 'Data Source=dbStage;Initial Catalog=FakeApp;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true', 'Data Source=dbDev;Initial Catalog=FakeApp;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true', 1, @connectionId
-- Add Application
EXEC [wds].[add_application] @hostName, @appName, 'FakeApp Application', @ownerEmail
-- Associate the application with the Key

EXEC [wds].[add_APIKey_application_role] @keyName, @appName, @roles
-- Above creates the three roles
SET @roles = @appName + '.Read'
EXEC [wds].[add_Recordset] @hostName, @appName, 'ChangeHistory', null, @appName, @roles, '[dbo].[get_change_history]', 1, 1, 1, 0, 60, 0, 1
EXEC [wds].[add_parameter] 'recordId', 'record_id', 0, '', 'null', '', 1, @appName, 'ChangeHistory'
EXEC [WDS].[add_parameter_example_value] 'FakeApp', 'ChangeHistory', 'SetA', 'recordId', '450A3098-CF64-42B4-831C-D225FFF4C18C'
EXEC [WDS].[add_parameter_example_value] 'FakeApp', 'ChangeHistory', 'SetB', 'recordId', '04D1E5C2-66E7-4AAC-AB59-DB23971D6305'
EXEC [WDS].[add_parameter_example_value] 'FakeApp', 'ChangeHistory', 'SetC', 'recordId', '5CF12563-BAA3-4499-8AE3-6FFFF47E75BF'

EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesJson', 'Monday', 'Day_Name', 'Monday'
EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesJson', 'Tuesday', 'Day_Name', 'Tuesday'
EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesJson', 'Wednesday', 'Day_Name', 'Wednesday'
EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesJson', 'Thursday', 'Day_Name', 'Thursday'
EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesJson', 'Friday', 'Day_Name', 'Friday'
EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesJson', 'Saturday', 'Day_Name', 'Saturday'
EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesJson', 'Sunday', 'Day_Name', 'Sunday'

EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesXml', 'Monday', 'Day_Name', 'Monday'
EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesXml', 'Tuesday', 'Day_Name', 'Tuesday'
EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesXml', 'Wednesday', 'Day_Name', 'Wednesday'
EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesXml', 'Thursday', 'Day_Name', 'Thursday'
EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesXml', 'Friday', 'Day_Name', 'Friday'
EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesXml', 'Saturday', 'Day_Name', 'Saturday'
EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesXml', 'Sunday', 'Day_Name', 'Sunday'

EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesCSV', 'Monday', 'Day_Name', 'Monday'
EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesCSV', 'Tuesday', 'Day_Name', 'Tuesday'
EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesCSV', 'Wednesday', 'Day_Name', 'Wednesday'
EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesCSV', 'Thursday', 'Day_Name', 'Thursday'
EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesCSV', 'Friday', 'Day_Name', 'Friday'
EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesCSV', 'Saturday', 'Day_Name', 'Saturday'
EXEC [WDS].[add_parameter_example_value] 'TestingOnly', 'DimDatesCSV', 'Sunday', 'Day_Name', 'Sunday'
