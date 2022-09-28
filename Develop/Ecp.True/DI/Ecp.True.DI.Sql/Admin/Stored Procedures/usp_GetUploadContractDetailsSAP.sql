/*-- ===============================================================================================================================================
-- Author:          Intergrupo
-- Created Date: 	Mar-05-2021	
-- <Description>:	This Procedure is used to get the File registration contract Details based on the UploadId</Description>
EXEC [Admin].[usp_GetUploadContractDetailsSAP] '46c32868-6dd5-4e6a-9cdc-937757a2b5c9'
-- ================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetUploadContractDetailsSAP]  
(
	@UploadId nvarchar(64)
)
AS
BEGIN
		SELECT	 FR.FileRegistrationId							AS FileRegistrationId
				,FRT.FileRegistrationTransactionId				AS TransactionId
				,FORMAT(FRT.CreatedDate, 'yyyy-MM-dd HH:mm:ss')	AS ProcessingTime
				,'S'											AS StatusMessage
				,'Mensaje procesado correctamente del pedido con ID ' + CONVERT(NVarchar, FRT.FileRegistrationTransactionId) +
				'enviado del Sistema Origen ' + CONVERT(NVarchar, FR.SystemTypeId) +
				'con fecha de recibido ' + FORMAT(FRT.CreatedDate, 'dd-MM-yyyy HH:mm:ss')
																AS Information
				,CT.DocumentNumber								AS OrderId
				,CT.OriginMessageId								    AS OriginMessageId
				,CASE
					WHEN FR.SourceTypeId = 10
					THEN 'MM086'
					ELSE 'SD156'		
				END												AS SourceTypeId
		FROM Admin.FileRegistration FR
		INNER JOIN Admin.FileRegistrationTransaction FRT
		ON FR.FileRegistrationId = FRT.FileRegistrationId
		INNER JOIN Admin.Contract CT
		ON CT.FileRegistrationTransactionId = FRT.FileRegistrationTransactionId
		WHERE CT.ContractId IS NOT NULL
		AND	FR.UploadId = @UploadId
		UNION ALL
		SELECT	 FR.FileRegistrationId							AS FileRegistrationId
				,NULL											AS TransactionId
				,FORMAT(FRT.CreatedDate, 'yyyy-MM-dd HH:mm:ss')	AS ProcessingTime
				,'E'											AS StatusMessage
				,PTE.ErrorMessage								AS Information
				,PT.Identifier									AS OrderId
				,PT.OriginMessageId								AS OriginMessageId
				,CASE
					WHEN FR.SourceTypeId = 10
					THEN 'MM086'
					ELSE 'SD156'		
				END												AS SourceTypeId
		FROM Admin.FileRegistration FR
		INNER JOIN Admin.FileRegistrationTransaction FRT
		ON FR.FileRegistrationId = FRT.FileRegistrationId
		LEFT JOIN Admin.Contract CT
		ON CT.FileRegistrationTransactionId = FRT.FileRegistrationTransactionId
		INNER JOIN Admin.PendingTransaction PT
		ON PT.MessageId = FR.UploadId
		INNER JOIN Admin.PendingTransactionError PTE
		ON PTE.TransactionId = PT.TransactionId
		WHERE FR.UploadId = @UploadId
END
GO