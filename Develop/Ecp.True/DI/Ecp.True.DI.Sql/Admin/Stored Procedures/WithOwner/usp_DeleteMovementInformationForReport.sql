/*-- =================================================================================================
-- Author:		Microsoft
-- Created date: Dec-24-2020
-- Updated date: May-20-2021  add NodeId Validation
-- Updated date: May-20-2021  add NodeId Validation
-- Updated date: Sep-28-2021  fix NodeId Validation
-- <Description>: This procedure is to delete data from movement details, attribute details and backup movement details based on ownershipticketid.</Description>
-- ===================================================================================================*/

CREATE PROCEDURE [Admin].[usp_DeleteMovementInformationForReport]
(
	      @OwnershipTicketId INT,
          @NodeId INT = NULL
)
AS
BEGIN
 DECLARE @NodeName	 NVARCHAR(2000)
    IF @NodeId IS NOT NULL
    SET @NodeName =  (select Name from Admin.Node where NodeId = @NodeId)
  SET NOCOUNT ON
  DELETE FROM [Admin].[MovementDetailsWithOwner]  WHERE OwnershipTicketId=@OwnershipTicketId AND (@NodeName IS NULL OR (SourceNode = @NodeName or DestinationNode = @NodeName));
  DELETE FROM [Admin].[AttributeDetailsWithOwner] WHERE OwnershipTicketId=@OwnershipTicketId AND (@NodeName IS NULL OR (SourceNodeName = @NodeName or DestinationNodeName = @NodeName));
  DELETE FROM [Admin].[BackupMovementDetailsWithOwner] WHERE OwnershipTicketId=@OwnershipTicketId AND (@NodeName IS NULL OR (SourceNode = @NodeName or DestinationNode = @NodeName));
END
GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This procedure is to delete data from movement details, attribute details and backup movement details based on ownershipticketid.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_DeleteMovementInformationForReport',
    @level2type = NULL,
    @level2name = NULL