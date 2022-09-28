
/*-- ===============================================================================================================================================
-- Author:          Intergrupo
-- Created Date: 	Jun-01-2021
-- Updated Date: 	Jun-21-2021 Updated returning columns
-- Updated Date: 	Sep-30-2021 Add segment validation
-- <Description>:	This Procedure is used to calculate date for delta movements </Description>
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_CalculateDateForDeltaMovements](@NodeId INT)
AS
BEGIN
	SELECT TOP 1
		T.TicketId,
		T.CategoryElementId AS SegmentId,
		DATEADD(DAY,1,T.EndDate) AS OperationDate
	FROM Admin.Ticket T WITH (NOLOCK) WHERE TicketTypeId = 1 AND T.Status IN (0, 1)
						AND CategoryElementId = (SELECT TOP 1 n.ElementId FROM Admin.NodeTag n JOIN Admin.CategoryElement c ON n.ElementId = c.ElementId
												  WHERE n.NodeId = @NodeId AND c.CategoryId = 2)
	ORDER BY Status, T.EndDate DESC
END
GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to calculate date for delta movements',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_CalculateDateForDeltaMovements',
							@level2type = NULL,
							@level2name = NULL