/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jun-12-2020
-- <Description>:	This Procedure is used to get the Delta movements Data based on the input of SegmentId, Start Date, End Date</Description>
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetDeltaMovements]
(
	   @SegmentId				INT,
       @StartDate				DATE,
	   @EndDate					DATE
	   
)
AS
BEGIN
		SELECT  Mov.MovementId
			   ,Mov.MovementTypeName
			   ,Mov.MovementTypeName
			   ,Mov.SourceNodeName
			   ,Mov.DestinationNodeName
			   ,Mov.SourceProductName
			   ,Mov.DestinationProductName
			   ,Mov.NetStandardVolume
			   ,CE.Name AS MeasurementUnit
			   ,Mov.OperationalDate
			   ,Mov.EventType
		FROM [Admin].[view_MovementInformation] Mov
		INNER JOIN Admin.CategoryElement CE
		ON Mov.MeasurementUnit = CE.ElementId 
		WHERE Mov.SegmentId = @SegmentId
		AND Mov.OperationalDate BETWEEN @StartDate AND @EndDate		
END

GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to get the Delta movements Data based on the input of SegmentId, Start Date, End Date',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetDeltaMovements',
							@level2type = NULL,
							@level2name = NULL