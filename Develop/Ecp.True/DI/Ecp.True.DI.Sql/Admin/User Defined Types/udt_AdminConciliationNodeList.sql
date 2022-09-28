/*-- ==============================================================================================================================
-- Author:          Intergrupo  
-- Created Date:    May-11-2021
 <Description>:		This is used as a parameter for GetTransferPointsForConciliationNodes SP.  </Description>
-- ================================================================================================================================*/

CREATE TYPE [Admin].[ConciliationNodeList] AS TABLE(
	[SourceNodeId] [int] NOT NULL,
	[DestinationNodeId] [int] NOT NULL
)
GO

EXEC sys.sp_addextendedproperty @name=N'Description',
								@value=N'This is used as a parameter for GetTransferPointsForConciliationNodes SP.',
								@level0type=N'SCHEMA',
								@level0name=N'Admin',
								@level1type=N'TYPE',
								@level1name=N'ConciliationNodeList'
GO
