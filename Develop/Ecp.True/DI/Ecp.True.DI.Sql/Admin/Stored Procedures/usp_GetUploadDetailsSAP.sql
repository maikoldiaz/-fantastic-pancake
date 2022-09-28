/*-- ===============================================================================================================================================
-- Author:          Microsoft
-- Created Date: 	Jun-18-2020	
-- Updated Date:	May-05-2020 -- solved to bug 116869
-- Updated Date:	Jun-28-2021 -- solved to HU 133911
-- <Description>:	This Procedure is used to get the File registration Details based on the UploadId</Description>
EXEC [Admin].[usp_GetUploadDetailsSAP] 'pce5pvmkmo8fj5e85jodww992ncgwnj5uls'
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetUploadDetailsSAP]  
(
	@UploadId nvarchar(64)
)
AS
BEGIN
		SELECT   FR.FileRegistrationId								AS FileRegistrationId
				,FR.UploadId										AS ProcessId
				,case when mov.SourceSystemId IN(160,161) and ISJSON(mov.Observations) = 1 then json_value(mov.Observations,'$.idMessageRomss')
					  when INVPROD.SourceSystemId IN(160,161) and ISJSON(INVPROD.Observations) = 1 then json_value(INVPROD.Observations,'$.idMessageRomss')
					  else ISNULL(MOV.MovementId, INVPROD.InventoryId) end COLLATE SQL_Latin1_General_CP1_CS_AS AS DocumentId
				,FRT.FileRegistrationTransactionId					AS TransactionId
				,'0000'												AS ErrorCode
				,'Transacción exitosa'								AS ErrorMessage
				,FRT.LastModifiedDate								AS LastDate
		FROM Admin.FileRegistration FR
		INNER JOIN Admin.FileRegistrationTransaction FRT
		ON FR.FileRegistrationId = FRT.FileRegistrationId
		LEFT JOIN Offchain.Movement MOV
		ON FRT.FileRegistrationTransactionId = MOV.FileRegistrationTransactionId
		LEFT JOIN Offchain.InventoryProduct INVPROD
		ON FRT.FileRegistrationTransactionId = INVPROD.FileRegistrationTransactionId
		WHERE (MOV.MovementTransactionId IS NOT NULL OR INVPROD.InventoryProductId IS NOT NULL)
		AND	   FR.UploadId = @UploadId
		UNION ALL
		SELECT	 FR.FileRegistrationId								AS FileRegistrationId
				,FR.UploadId										AS ProcessId
				,PT.Identifier										AS DocumentId
				,NULL												AS TransactionId
				,CASE WHEN (ISNUMERIC(LTRIM(RTRIM(SUBSTRING(PTE.ErrorMessage, 0, CHARINDEX('-', PTE.ErrorMessage))))) > 0) 
					  THEN LTRIM(RTRIM(SUBSTRING(PTE.ErrorMessage, 0, CHARINDEX('-', PTE.ErrorMessage))))
					  ELSE '9999'
				 END												AS ErrorCode
				,CASE WHEN (ISNUMERIC(LTRIM(RTRIM(SUBSTRING(PTE.ErrorMessage, 0, CHARINDEX('-', PTE.ErrorMessage))))) > 0) 
					  THEN LTRIM(RTRIM(SUBSTRING(PTE.ErrorMessage, CHARINDEX('-', PTE.ErrorMessage) + 1, LEN(PTE.ErrorMessage))))
					  ELSE PTE.ErrorMessage
				 END												AS ErrorMessage
				 ,PT.CreatedDate									AS LastDate
		FROM Admin.FileRegistration FR		
		INNER JOIN Admin.PendingTransaction PT
		ON PT.MessageId = FR.UploadId
		INNER JOIN Admin.PendingTransactionError PTE
		ON PTE.TransactionId = PT.TransactionId
		WHERE FR.UploadId = @UploadId
END
Go
GO
EXEC sp_addextendedproperty @name		= N'MS_Description',
							@value		= N'This Procedure is used to get the File registration Details based on the UploadId',
							@level0type = N'SCHEMA',
							@level0name = N'Admin',
							@level1type = N'PROCEDURE',
							@level1name = N'usp_GetUploadDetailsSAP',
							@level2type = NULL,
							@level2name = NULL