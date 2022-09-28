/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Mar-09-2020
-- Updated Date:	Mar-20-2020
-- Updated Date:	Mar-31-2020; Updated the SP to accept a new table parameter with variableids and apply merge operation
-- <Description>:   This Procedure is used to bulk update Rules for Node, NodeConnectionProduct and StorageLocationProduct tables. </Description>
-- ================================================================================================================================================*/

CREATE PROCEDURE [Admin].usp_BulkUpdateRules
(
  @BulkUploadIds [admin].[KeyType] READONLY
, @VariableTypeIds [admin].[KeyType] READONLY
, @ruleType NVARCHAR(100) -- Node, NodeConnectionProduct, StorageLocationProduct, StorageLocationProductVariable
, @ruleId INT 
, @lastmodifiedBy NVARCHAR(260) 
)
AS
BEGIN
    DECLARE @countofIds int
    SELECT @countofIds= COUNT(*) FROM @BulkUploadIds
    IF (@countofIds <1)
        BEGIN
            RAISERROR ('No Ids to Update',16,1)
            RETURN 0 
        END
    ELSE 
    BEGIN
        IF (@ruleType ='Node'  OR @ruleType = 'NodeConnectionProduct' OR @ruleType = 'StorageLocationProduct' OR @ruleType = 'StorageLocationProductVariable')
        BEGIN
            IF @ruleType= 'Node'
                BEGIN
                    UPDATE admin.Node
                    SET NodeOwnershipRuleId=@ruleId
                    ,LastModifiedBy=@lastmodifiedBy
                    ,LastModifiedDate=Admin.udf_GETTRUEDATE()
                    WHERE NodeId in (SELECT [Key] FROM @BulkUploadIds)
                    RETURN 1 -- SUCCESS
                END
            ELSE IF @ruleType= 'NodeConnectionProduct'
                BEGIN
                    UPDATE admin.NodeConnectionProduct
                    SET NodeConnectionProductRuleId=@ruleId
                    ,LastModifiedBy=@lastmodifiedBy
                    ,LastModifiedDate=Admin.udf_GETTRUEDATE()
                    WHERE NodeConnectionProductId in (SELECT [Key] FROM @BulkUploadIds)
                    RETURN 1 -- SUCCESS
                END
            ELSE IF @ruleType= 'StorageLocationProduct'
                BEGIN
                    UPDATE admin.StorageLocationProduct
                    SET NodeProductRuleId=@ruleId
                    ,LastModifiedBy=@lastmodifiedBy
                    ,LastModifiedDate=Admin.udf_GETTRUEDATE()
                    WHERE StorageLocationProductId in (SELECT [Key] FROM @BulkUploadIds)
                    RETURN 1 -- SUCCESS
                END
            ELSE IF @ruleType= 'StorageLocationProductVariable'
                BEGIN
                    UPDATE admin.StorageLocationProduct
                    SET NodeProductRuleId=@ruleId
                    ,LastModifiedBy=@lastmodifiedBy
                    ,LastModifiedDate=Admin.udf_GETTRUEDATE()
                    WHERE StorageLocationProductId in (SELECT [Key] FROM @BulkUploadIds)

                    ------------------------------------------
                    MERGE [Admin].[StorageLocationProductVariable] AS TARGET
					USING ( SELECT   blu.[Key] As StorageLocationProductId
							,vt.[Key] As VariableTypeId
							from @BulkUploadIds blu, @VariableTypeIds vt
                            UNION 
							SELECT StorageLocationProductId, VariableTypeId
							From [Admin].[StorageLocationProductVariable]
							Where StorageLocationProductId NOT IN 
								(Select [Key] From @BulkUploadIds) 
							) 
					AS SOURCE (StorageLocationProductId, VariableTypeId)
					ON	(TARGET.StorageLocationProductId = SOURCE.StorageLocationProductId
					AND  TARGET.VariableTypeId = SOURCE.VariableTypeId)

					WHEN NOT MATCHED BY TARGET
					THEN INSERT (StorageLocationProductId, VariableTypeId, CreatedBy) 
							VALUES (SOURCE.StorageLocationProductId, 
									SOURCE.VariableTypeId,
									@lastmodifiedBy)
									
					WHEN NOT MATCHED BY SOURCE
					THEN DELETE;

                    RETURN 1 -- SUCCESS
                END
        END
	    ELSE
	        BEGIN
	    	    RAISERROR ('Invalid Type of table to be updated',16,2) 
                RETURN 0 
	    	END
    END

END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is used to bulk update Rules for Node, NodeConnectionProduct and StorageLocationProduct tables.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_BulkUpdateRules',
    @level2type = NULL,
    @level2name = NULL