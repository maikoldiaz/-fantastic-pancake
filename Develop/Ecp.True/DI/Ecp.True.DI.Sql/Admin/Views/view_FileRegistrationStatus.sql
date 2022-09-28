/*-- =================================================================================================
-- Author:		 Microsoft
-- Create date:  Dec-02-2019
-- Updated date: Mar-20-2020
				 Aug-20-2020 Modified Condition "IN" to "="
-- <Description>:	This View is to Fetch Data view_FileRegistrationStatus to primarily display the calculated Status,RecordsProcessed and ErrorCount of Fileregistrations with other default columns.</Description>:
-- ===================================================================================================*/

CREATE VIEW [Admin].[view_FileRegistrationStatus]
AS
SELECT [UploadId]	
	  ,[CreatedDate]
	  ,[SegmentName]
	  ,[Name]
	  ,[SystemTypeId]
	  ,[FileActionType]	
	  ,[CreatedBy]
	  ,Case WHEN IsParsed = 0 Then 'Fallido'
			WHEN ZeroCount = 0 AND OneCount = 0 and TwoCount = 0 Then 'Procesando'
			WHEN OneCount > 0	Then 'Procesando'	--> 1 = PROCESSING
			WHEN TwoCount > 0	Then 'Fallido'		--> 2 = FAILED
			ELSE 'Finalizado'	--> 0 = FINALIZED			
		END AS [Status]
	  ,CASE WHEN IsParsed = 0 THEN 0
			ELSE (ZeroCount + TwoCount) 
		END AS [RecordsProcessed]
	  ,CASE WHEN IsParsed = 0 THEN 1
			ELSE TwoCount 
		END AS [ErrorCount]
	  ,IsParsed
FROM 
(
	SELECT DISTINCT  Fr.FileRegistrationId
					,Fr.UploadId
					,Fr.CreatedDate
					,Ce.Name As SegmentName
					,Fr.Name
					,Fr.SystemTypeId
					,Rfat.FileActionType
					,Fr.CreatedBy
					,Fr.IsParsed
					,(SELECT COUNT(StatusTypeId)
						FROM Admin.FileRegistrationTransaction Frt
						WHERE Fr.FileRegistrationId = Frt.FileRegistrationId
						AND StatusTypeId = 0		--> 0 = PROCESSED
						) As ZeroCount

					,(SELECT Count(StatusTypeId)
						FROM Admin.FileRegistrationTransaction Frt
						WHERE Fr.FileRegistrationId = Frt.FileRegistrationId
						AND StatusTypeId  = 1	--> 1 = PROCESSING
						)	AS 	OneCount

					,( SELECT COUNT(StatusTypeId)
						FROM Admin.FileRegistrationTransaction Frt
						WHERE Fr.FileRegistrationId = Frt.FileRegistrationId
						AND StatusTypeId = 2	--> 2 = FAILED
						) As TwoCount

	FROM [Admin].FileRegistration Fr
	LEFT JOIN [Admin].[CategoryElement] Ce
		ON Fr.SegmentId = Ce.ElementId
	LEFT JOIN [Admin].[RegisterFileActionType] Rfat
		On Fr.[Action] = Rfat.ActionTypeId
) A;

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch Data view_FileRegistrationStatus to primarily display the calculated Status,RecordsProcessed and ErrorCount of Fileregistrations with other default columns.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'view_FileRegistrationStatus',
    @level2type = NULL,
    @level2name = NULL


