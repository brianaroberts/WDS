CREATE PROCEDURE [wds].[del_recordset]
	@recordsetId int	
AS
	DECLARE @paramIDs TABLE (parameter_id int)
	DECLARE @connectionId INT
	DECLARE @connectionCount INT

	INSERT INTO @paramIDs 
		SELECT parameter_id FROM [wds].[recordset_parameter_map] INNER JOIN [wds].[recordsets] ON [recordset_parameter_map].recordset_id = [recordsets].recordset_id WHERE [recordsets].recordset_id = @recordsetId
		EXCEPT
		SELECT parameter_id FROM [wds].[recordset_parameter_map] INNER JOIN [wds].[recordsets] ON [recordset_parameter_map].recordset_id = [recordsets].recordset_id WHERE [recordsets].recordset_id <> @recordsetId
	
	SELECT @connectionId = connection_id FROM [wds].[recordsets] WHERE recordset_id = @recordsetId
	-- REMOVE parameter mappings first
	UPDATE [wds].[recordset_parameter_map] SET [is_active] = 0, [mod_dt] = getdate() WHERE recordset_id = @recordsetId
	-- DELETE parameters only if they are only referenced by this application
	UPDATE [wds].[parameters] SET [is_active] = 0, [mod_dt] = getdate() WHERE parameter_id IN (SELECT parameter_id FROM @paramIDs)

	UPDATE [wds].[recordsets] SET [is_active] = 0, [mod_dt] = getdate() WHERE recordset_id = @recordsetId

	-- If this was the last usage of a connection, clean up the connections table.
	SELECT @connectionCount = COUNT(connection_id) FROM [wds].[recordsets] WHERE connection_id = @connectionId
	If (@connectionId = 0)
		UPDATE [wds].[connections] SET [is_active] = 0, [mod_dt] = getdate() WHERE connection_id = @connectionId
RETURN 0
