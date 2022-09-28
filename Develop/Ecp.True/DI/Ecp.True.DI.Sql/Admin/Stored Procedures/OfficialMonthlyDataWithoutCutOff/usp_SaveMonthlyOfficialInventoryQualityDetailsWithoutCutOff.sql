/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jul-10-2020
-- Update date: 	Sep-25-2020 Removed delete statements, CZ deletes are part of Cleanup SP's
-- Description:     This Procedure is to Get Official Monthly InventoryQuality details based on Element, Node, StartDate, EndDate.
--EXEC [Admin].[usp_SaveMonthlyOfficialInventoryQualityDetailsWithoutCutOff] 137236,30812,'2020-06-26','2020-06-28','bd5b494e-ac14-466b-b507-0f93fd22ed7f'
--SELECT * FROM ADMIN.OfficialMonthlyInventoryQualityDetails
-- ==============================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_SaveMonthlyOfficialInventoryQualityDetailsWithoutCutOff]
(
	  @ElementId             INT
     ,@NodeId                INT 
     ,@StartDate             DATE                      
     ,@EndDate               DATE                      
     ,@ExecutionId           INT

)
AS
BEGIN
	SET NOCOUNT ON
  
  --Variables Declaration
    DECLARE   @Previousdate                 DATE =  [Admin].[udf_GetTrueDate] ()-1
             ,@Todaysdate                   DATE =  [Admin].[udf_GetTrueDate] ()
                              


		INSERT INTO ADMIN.OfficialMonthlyInventoryQualityDetails
		(
			 [RNo]					
			,[System]               
			,[Version]              
			,[InventoryId]			
			,[NodeName]             
			,[Product]		       
			,[NetStandardVolume]    
			,[GrossStandardQuantity]
			,[MeasurementUnit]		
			,[Owner]                
			,[OwnershipVolume]      
			,[OwnershipPercentage]  
			,[Origin]               
			,[RegistrationDate]     
			,[Attribute]            
			,[AttributeValue]	   
			,[ValueAttributeUnit]	
			,[AttributeDescription] 
			,[InventoryProductId]         
			,[ExecutionId]
			,[CreatedBy]
			,[CreatedDate]
		 )
	   SELECT    OMI.[RNo]                                  AS [RNo]
				,OMI.[System]                               AS [System]
				,OMI.[Version]                              AS [Version]
				,OMI.[InventoryId]                          AS [InventoryId]
				,OMI.[NodeName]                             AS [NodeName]
				,OMI.[Product]                              AS [Product]
				,OMI.[NetStandardVolume]                    AS [NetStandardVolume]
				,OMI.[GrossStandardQuantity]                AS [GrossStandardQuantity]
				,OMI.[MeasurementUnit]                      AS [MeasurementUnit]
				,OMI.[Owner]                                AS [Owner]
				,OMI.[OwnershipVolume]                      AS [OwnershipVolume]
				,OMI.[OwnershipPercentage]                  AS [OwnershipPercentage]
				,OMI.[Origin]                               AS [Origin]
				,OMI.[RegistrationDate]                     AS [RegistrationDate]
				,AttributeId.[Name]							AS [Attribute]
				,Att.[AttributeValue]                       AS [AttributeValue]
				,AttributeUnitElement.[Name]                AS [ValueAttributeUnit]
				,Att.[AttributeDescription]                 AS [AttributeDescription]
				,OMI.[InventoryProductId]                   AS [InventoryProductId]
				,OMI.[ExecutionId]							AS [ExecutionId]
				,OMI.[CreatedBy]                            AS [CreatedBy]
				,@Todaysdate								AS [CreatedDate]
				FROM [Admin].[OfficialMonthlyInventoryDetails] OMI
				INNER JOIN [Admin].[Attribute] Att
				ON Att.[InventoryProductId]		= OMI.[InventoryProductId]
				INNER JOIN [Admin].[CategoryElement] AttributeUnitElement
				ON AttributeUnitElement.ElementId = Att.ValueAttributeUnit
				INNER JOIN [Admin].[CategoryElement] AttributeId
				ON AttributeId.ElementId = Att.AttributeId
				AND AttributeId.CategoryId = 20
				WHERE OMI.[ExecutionId]    	= @ExecutionId
END
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to feed the OfficialInventoryQualityInformation table',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveMonthlyOfficialInventoryQualityDetailsWithoutCutOff',
    @level2type = NULL,
    @level2name = NULL
