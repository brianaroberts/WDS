/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
ALTER DATABASE [WDS] SET PAGE_VERIFY CHECKSUM 
GO

IF NOT EXISTS (SELECT * FROM [wds].[log_level_map] WHERE [log_description] = 'Information')
BEGIN
    INSERT INTO [wds].[log_level_map] values (0,'Information');
    INSERT INTO [wds].[log_level_map] values (1,'Warning');
    INSERT INTO [wds].[log_level_map] values (2,'Error');
END

-- Variables to preconfigure database
DECLARE @hostName varchar(64) = 'HQNZTK923404'
DECLARE @serverOwnerEmail varchar(256) = 'brianaroberts@live.com'
DECLARE @hostId INT

EXEC [wds].[add_host] @hostName, 1, @hostId OUTPUT

-- Add WDS to the Schema
EXEC [wds].[add_application] @hostName, 'WDS', 'Default Application Domain for the Web Data Service', @serverOwnerEmail;
EXEC [wds].[add_application] @hostName, 'TestingOnly', 'Just for testing', @serverOwnerEmail;

-- Add Some API Keys
EXEC [wds].[add_APIKey] @hostName, 'Admin', 'B+qlxM5RQwfksGjkVdJiaRBNhOQrWfOev1ZbvuYIqz3fbMoIf2GTentZ6EGbeovZ', '*', 'This is the default WDS Admin Account', @serverOwnerEmail, 1
EXEC [wds].[add_APIKey_application_role] 'Admin', 'WDS', 'WDS.Admin,WDS.Write,WDS.Read,TestingOnly.Admin,TestingOnly.Read,TestingOnly.Write'

EXEC [wds].[add_APIKey] @hostName, 'Testing Admin', 'z4g+UVVtGqKdkKtyPmOTBA4ny8wXkO/zXbMMrhWG1KJGmiftJuiOnpJA6qabeTwp', '((l|L)ocal(h|H)ost)|127.0.0.1', 'This admin account for TestingOnly App', @serverOwnerEmail, 1
EXEC [wds].[add_APIKey_application_role] 'Testing Admin', 'TestingOnly', 'WDS.Read,TestingOnly.Admin,TestingOnly.Read,TestingOnly.Write'
	

-- Insert the return Types
BEGIN
    IF NOT EXISTS (SELECT * FROM [wds].[returnType_map] WHERE [returnType_id] = 0)
    BEGIN
        INSERT INTO [wds].[returnType_map] VALUES (0, 'NotSet', 'The system default will apply for this recordset', 1);
    END 

    IF NOT EXISTS (SELECT * FROM [wds].[returnType_map] WHERE [returnType_id] = 1)
    BEGIN
        INSERT INTO [wds].[returnType_map] VALUES (1, 'Json', 'Json format', 1);
    END 
    IF NOT EXISTS (SELECT * FROM [wds].[returnType_map] WHERE [returnType_id] = 2)
    BEGIN
        INSERT INTO [wds].[returnType_map] VALUES (2, 'CSV', 'CSV format with the column names at the top', 1);
    END 
    IF NOT EXISTS (SELECT * FROM [wds].[returnType_map] WHERE [returnType_id] = 3)
    BEGIN
        INSERT INTO [wds].[returnType_map] VALUES (3, 'Xml', 'Xml format from .NET DataSet object', 1);
    END 
    IF NOT EXISTS (SELECT * FROM [wds].[returnType_map] WHERE [returnType_id] = 4)
    BEGIN
        INSERT INTO [wds].[returnType_map] VALUES (4, 'Email', 'Formated for the body of an email', 1);
    END 

    IF NOT EXISTS (SELECT * FROM [wds].[returnType_details_map] WHERE [details_id] = 0)
    BEGIN
        INSERT INTO [wds].[returnType_details_map] VALUES (0, 'NotSet', 'The system default will apply for this recordset', 1);
    END
    IF NOT EXISTS (SELECT * FROM [wds].[returnType_details_map] WHERE [details_id] = 1)
    BEGIN
        INSERT INTO [wds].[returnType_details_map] VALUES (1, 'Default', 'Default will apply. Recordsets with WDS Metadata', 1);
    END
    IF NOT EXISTS (SELECT * FROM [wds].[returnType_details_map] WHERE [details_id] = 2)
    BEGIN
        INSERT INTO [wds].[returnType_details_map] VALUES (2, 'Simple', 'Only the recordset data will be included in the return', 1);
    END
END

    -- Call Types
BEGIN
    IF NOT EXISTS (SELECT * FROM [wds].[callType_map] WHERE [callType_id] = 0)
    BEGIN
        INSERT INTO [wds].[callType_map] VALUES (0, 'NotSet', 'The system default will apply for this recordset', 1);
    END
    IF NOT EXISTS (SELECT * FROM [wds].[callType_map] WHERE [callType_id] = 1)
    BEGIN
        INSERT INTO [wds].[callType_map] VALUES (1, 'StoredProc', 'Stored Proc is called', 1);
    END
    IF NOT EXISTS (SELECT * FROM [wds].[callType_map] WHERE [callType_id] = 2)
    BEGIN
        INSERT INTO [wds].[callType_map] VALUES (2, 'SQL', 'SQL is sent to a SQL Database', 1);
    END
    IF NOT EXISTS (SELECT * FROM [wds].[callType_map] WHERE [callType_id] = 3)
    BEGIN
        INSERT INTO [wds].[callType_map] VALUES (3, 'GetDataFromFile', 'Retrieves data from a file', 1);
    END
    IF NOT EXISTS (SELECT * FROM [wds].[callType_map] WHERE [callType_id] = 4)
    BEGIN
        INSERT INTO [wds].[callType_map] VALUES (4, 'FileUpdate', 'Will result in an update to a file', 1);
    END
    IF NOT EXISTS (SELECT * FROM [wds].[callType_map] WHERE [callType_id] = 5)
    BEGIN
        INSERT INTO [wds].[callType_map] VALUES (5, 'DailyFileUpdate', 'Intended to update a file in a daily format', 1);
    END
    IF NOT EXISTS (SELECT * FROM [wds].[callType_map] WHERE [callType_id] = 6)
    BEGIN
        INSERT INTO [wds].[callType_map] VALUES (6, 'WebService', 'This recordset will result from a call made to another web service', 1);
    END
    IF NOT EXISTS (SELECT * FROM [wds].[callType_map] WHERE [callType_id] = 7)
    BEGIN
        INSERT INTO [wds].[callType_map] VALUES (7, 'FileOperations', 'Generic File Operations', 1);
    END
END
-- Test Data - Countries
EXEC [tst].[reset_countries]

-- Test Data - Dim Dates
EXEC [tst].[reset_dimDates]

EXEC [tst].[reset_states]