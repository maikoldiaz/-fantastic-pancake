/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jul-10-2020
-- Modified date:	Jul-14-2020
					Renamed table OfficialMonthlyMovementQualityDetails to OfficialMonthlyMovementQualityDetails
-- Description:     This Procedure is to Get Official Monthly movement quality details based on Element, Node, StartDate, EndDate.
--EXEC [Admin].[usp_SaveMonthlyOfficialMovementQualityDetailsWithoutCutOff] 137236,30812,'2020-07-03','2020-07-06','bd5b494e-ac14-466b-b507-0f93fd22ed7f'
--SELECT * FROM ADMIN.OfficialMonthlyMovementQualityDetails
-- ==============================================================================================================================*/

CREATE PROCEDURE [Admin].[usp_SaveMonthlyOfficialMovementQualityDetailsWithoutCutOff]
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
                           

		INSERT INTO ADMIN.OfficialMonthlyMovementQualityDetails
		(
			 [RNo]                  
			,[System]               
			,[Version]              
			,[MovementId]           
			,[TypeMovement]         
			,[Movement]             
			,[SourceNode]           
			,[DestinationNode]      
			,[SourceProduct]        
			,[DestinationProduct]   
			,[NetQuantity]          
			,[GrossQuantity]        
			,[MeasurementUnit]      
			,[Owner]                
			,[Ownershipvolume]      
			,[Ownershippercentage]  
			,[Origin]               
			,[RegistrationDate]     
			,[Attribute]            
			,[AttributeValue]	     
			,[ValueAttributeUnit]	 
			,[AttributeDescription] 
			,[MovementTransactionId]        
			,[ExecutionId]
			,[CreatedBy]
			,[CreatedDate]
		 )
	   SELECT    OMM.[RNo]									           AS [RNo]
				,OMM.[System]										   AS [System]
				,OMM.[Version]										   AS [Version]
				,OMM.[MovementId]         							   AS [MovementId]
				,OMM.[TypeMovement]       							   AS [TypeMovement]
				,OMM.[Movement]           							   AS [Movement]
				,OMM.[SourceNode]         							   AS [SourceNode]
				,OMM.[DestinationNode]    							   AS [DestinationNode]
				,OMM.[SourceProduct]      							   AS [SourceProduct]
				,OMM.[DestinationProduct] 							   AS [DestinationProduct]
				,OMM.[NetQuantity]        							   AS [NetQuantity]
				,OMM.[GrossQuantity]      							   AS [GrossQuantity]
				,OMM.[MeasurementUnit]    							   AS [MeasurementUnit]
				,OMM.[Owner]              							   AS [Owner]
				,OMM.[Ownershipvolume]    							   AS [Ownershipvolume]
				,OMM.[Ownershippercentage]							   AS [Ownershippercentage]
				,OMM.[Origin]             							   AS [Origin]
				,OMM.[RegistrationDate]   							   AS [RegistrationDate]
				,AttributeId.[Name]             					   AS [Attribute]
				,Att.[AttributeValue]								   AS [AttributeValue]
				,AttributeUnitElement.[Name]						   AS [ValueAttributeUnit]
				,Att.[AttributeDescription]							   AS [AttributeDescription]
				,OMM.[MovementTransactionId]						   AS [MovementTransactionId]
				,OMM.[ExecutionId]									   AS [ExecutionId]
				,OMM.[CreatedBy]									   AS [CreatedBy]
				,@Todaysdate										   AS [CreatedDate]
				FROM [Admin].[OfficialMonthlyMovementDetails] OMM
				INNER JOIN [Admin].[Attribute] Att
				ON Att.[MovementTransactionId]	= OMM.[MovementTransactionId]
				INNER JOIN [Admin].[CategoryElement] AttributeUnitElement
				ON AttributeUnitElement.ElementId = Att.ValueAttributeUnit
				INNER JOIN [Admin].[CategoryElement] AttributeId
				ON AttributeId.ElementId = Att.AttributeId
				AND AttributeId.CategoryId = 20
				WHERE OMM.[ExecutionId]    	= @ExecutionId
END
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to feed the OfficialMovementQuality table',
    @level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveMonthlyOfficialMovementQualityDetailsWithoutCutOff',
    @level2type = NULL,
    @level2name = NULL
