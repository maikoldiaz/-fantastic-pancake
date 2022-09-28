/*-- =================================================================================================================================
-- Author: IG Service
-- Created Date: Oct-12-2021
-- <Description>: This Procedure is used to get the storage node.
* get the nodes by segment.
* get the failes logistic movement.
</Description>
-- ==================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetStorageNode]
(
		@TicketId						INT
)
AS
 BEGIN 

	SELECT  
			 N.LogisticCenterId
			,SUBSTRING(MIN (SL.StorageLocationId),CHARINDEX(':',MIN (SL.StorageLocationId))+1,LEN(MIN (SL.StorageLocationId))) AS SapStorage
			,p.ProductId AS ProductId
			,n.NodeId
			FROM Admin.Node N
			LEFT JOIN [Offchain].[LogisticMovement] AS LM 
			on LM.DestinationLogisticCenterId = N.LogisticCenterId OR LM.SourceLogisticCenterId = N.LogisticCenterId
			LEFT JOIN Admin.NodeStorageLocation NLS ON NLS.NodeId = N.NodeId
			LEFT JOIN Admin.StorageLocation SL ON SL.LogisticCenterId = N.LogisticCenterId
			LEFT JOIN admin.StorageLocationProduct SLP ON SLP.NodeStorageLocationId = NLS.NodeStorageLocationId
			LEFT JOIN admin.Product P ON P.ProductId = SLP.ProductId
			WHERE LM.TicketId = @TicketId 
			AND P.ProductId IN(LM.SourceProductId ,LM.DestinationProductId)
			AND N.NodeId  IN(LM.SourceLogisticNodeId, LM.DestinationLogisticNodeId) 
			AND N.LogisticCenterId IN (LM.SourceLogisticCenterId, LM.DestinationLogisticCenterId)
			GROUP BY  N.LogisticCenterId ,P.ProductId ,N.NodeId
		
	 	
END

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description',
@value=N'This Procedure is used to get the storage Nodes for ticket.
* get the nodes by segment.
* get the storage nodes list.' ,
@level0type=N'SCHEMA',
@level0name=N'Admin',
@level1type=N'PROCEDURE',
@level1name=N'usp_GetStorageNode'

GO
