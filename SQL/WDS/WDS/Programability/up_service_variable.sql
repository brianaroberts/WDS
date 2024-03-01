CREATE PROCEDURE [wds].[up_service_variable]
	@hostName varchar(64),
	@variableName varchar(64),
	@variableValue varchar(max), 
	@description varchar(255)
AS
	DECLARE @hostId INT

	IF @hostName = '*'
		BEGIN		
			WHILE (1=1) --We are updating the variable for all hosts and must loop through. 
			BEGIN
				SELECT TOP 1 @hostId = host_id, @hostName = host_name FROM [wds].[hosts] WHERE host_id > @hostId; 

				EXEC [wds].[add_serivce_variable] @hostName, @variableName, @variableValue 

				IF @@ROWCOUNT = 0 BREAK; 
			END
		END
	ELSE
		BEGIN
			-- Go ahead and create the host if it doesn't exist. 
			EXEC [wds].[add_host] @hostName, '1', @hostId OUTPUT
			
			IF @hostId IS NOT NULL
			BEGIN
				IF EXISTS (SELECT * FROM [wds].[service_variables] WHERE [server_host_id] = @hostId and var_name = @variableName)
					UPDATE [wds].[service_variables] SET [var_value] = @variableValue, [mod_dt] = GETDATE() WHERE [server_host_id] = @hostId and var_name = @variableName
				ELSE		
					INSERT INTO [wds].[service_variables] VALUES (@hostId, @variableName, @variableValue, @description, default, default, default, default)
			END
		END
RETURN 0
