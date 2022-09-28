/*-- ==============================================================================================================================
-- Author:			Microsoft  
-- Created Date:    Dec-24-2019
-- Updated Date:	Mar-20-2020
-- <Description>:	This Procedure is used to update the pipeline run id in the respective table (executionId) based on input parameter(Table Name). </Description>
-- ================================================================================================================================*/
CREATE PROCEDURE [Audit].[usp_UpdateRunId]
	@RunId			VARCHAR (100),
	@SchemaName		VARCHAR (100),
	@TableName		VARCHAR (100)
AS
BEGIN
	DECLARE @FullTableName VARCHAR (1000), @Query NVARCHAR (2000), @Query1 NVARCHAR (2000)
	SET @FullTableName = @SchemaName + '.' + @TableName

	SET @Query = N'UPDATE ' + @FullTableName 
			   + ' SET ExecutionId = ' + '''' + @RunId + '''' 
			   + ' WHERE ExecutionId IS NULL'
	EXEC sp_executesql @Query

	SET @Query1 = N'DELETE FROM ' + @FullTableName 
			    + ' WHERE ExecutionId != ' + '''' + @RunId + ''''
				+ ' AND SourceSystem =''CSV'''
	EXEC sp_executesql @Query1
END

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to update the pipeline run id in the respective table (executionId) based on input parameter(Table Name).',
	@level0type = N'SCHEMA',
    @level0name = N'Audit',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_UpdateRunId',
    @level2type = NULL,
    @level2name = NULL