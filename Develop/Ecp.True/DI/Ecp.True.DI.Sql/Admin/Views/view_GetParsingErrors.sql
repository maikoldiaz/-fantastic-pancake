/*-- =================================================================================================
-- Author:		Microsoft
-- Create date: Feb-14-2020
-- Updated date: Mar-20-2020
-- Updated Date: July 24-2020 Joined with Category Element table to get SystemName 
-- <Description>:	This View is to Fetch all the records where parsing was successfull(FileRegTran) 
				and un parsed(PTE with FRTID = null)</Description>:
-- ===================================================================================================*/

CREATE VIEW [Admin].[view_GetParsingErrors]
AS
SELECT  CAST(ROW_NUMBER() OVER (Order By MessageId) As INT) As Id
		,[ErrorId]
		,[MessageId]
		,[SystemName]
		,[SystemTypeName]
		,[Process]
		,[FileName]
		,[CreationDate]
		,[IsRetry]
		,[FileRegistrationTransactionId]
		,[UploadId]
FROM
(
	--Non Retriable Errors
	--Table [Admin].FileRegistrationTransaction FRT will not have the data incase of Non-Retriable
	--Default null value should be passed for Process
	SELECT			  CAST(PT.TransactionId AS VARCHAR(10))			AS [MessageId]
					 ,CASE	WHEN	ISNULL(CE.[Name],'') = ''
							THEN	SysTyp.[Name]
							ELSE	CE.[Name] END					AS [SystemName]
					 ,SysTyp.[Name]									AS [SystemTypeName]
					 ,MT.Name										AS [Process]
					 ,CASE WHEN Fr.SystemTypeId = 3--Excel
						   THEN FR.Name
						   ELSE NULL END							AS [FileName]
					 ,PTE.CreatedDate								AS [CreationDate]
					 ,CAST(0 As BIT)								AS [IsRetry]
					 ,NULL											AS [FileRegistrationTransactionId]
					 ,FR.UploadId									AS [UploadId]
					 ,PTE.ErrorId									AS [ErrorId]
	FROM [Admin].[FileRegistration]  FR	
	INNER JOIN [Admin].[PendingTransaction] PT
	ON PT.MessageId = FR.UploadId
	INNER JOIN [Admin].[PendingTransactionError] PTE
	ON PT.TransactionId = PTE.TransactionId
	LEFT JOIN  Admin.CategoryElement CE
	ON CE.ElementId = PT.SystemName
	AND CE.CategoryId = 22
	LEFT JOIN [Admin].[SystemType] SysTyp
    ON SysTyp.SystemTypeId = PT.SystemTypeId
	LEFT JOIN [Admin].[MessageType] MT
	ON MT.MessageTypeId = PT.MessageTypeId
	WHERE PTE.RecordID IS NULL--Will be Null in Case of Non-Retriable
	AND PTE.IsRetrying = 0
	AND PTE.Comment IS NULL

	UNION

	--Fetching the data For which Error Can be retried(Retriable Errors)
	--Table [Admin].FileRegistrationTransaction FRT will  have the data incase of Retriable
	--RecordId Column from table [Admin].[PendingTransactionError] will have the Data
	SELECT  [MessageId]
		   ,CASE	WHEN	ISNULL([SystemName],'') = ''
					THEN	[SystemTypeName]
					ELSE	[SystemName] END	AS [SystemName]
		   ,[SystemTypeName]
		   ,[Process]
		   ,[FileName]
		   ,[CreationDate]
		   ,[IsRetry]
		   ,[FileRegistrationTransactionId]
		   ,[UploadId]
		   ,[ErrorId]
	FROM 
	(
		SELECT  PteSubQ.RecordID							AS [MessageId]
			   ,PteSubQ.SystemName							AS [SystemName]
			   ,PteSubQ.[SystemTypeName]					AS [SystemTypeName]
			   ,MT.Name										AS [Process]
			   ,CASE WHEN Fr.SystemTypeId = 3--Excel
	 				 THEN FR.Name
	 				 ELSE NULL END							AS [FileName]
			  ,PteSubQ.CreatedDate							AS [CreationDate]
			  ,CAST(1 As BIT)								AS [IsRetry]
			  ,FRT.FileRegistrationTransactionId			AS [FileRegistrationTransactionId]
			  ,FR.UploadId									AS [UploadId]
			  ,PteSubQ.ErrorId								AS [ErrorId]
			  ,ROW_NUMBER()OVER(PARTITION BY PteSubQ.RecordID ORDER BY PteSubQ.CreatedDate DESC)Rnum
		FROM
		(
			SELECT DISTINCT   PTE.RecordID		AS RecordID
							 ,CE.[Name]			AS [SystemName]
							 ,SysTyp.[Name]		AS [SystemTypeName]
							 ,PTE.CreatedDate	AS [CreatedDate]
							 ,PTE.ErrorId		AS [ErrorId]
							 ,PT.MessageTypeId  AS MessageTypeId
			FROM [ADMIN].[PendingTransactionError] PTE
			INNER JOIN [Admin].[PendingTransaction] PT
			ON PT.TransactionId = PTE.TransactionId
			LEFT JOIN  Admin.CategoryElement CE
			ON CE.ElementId = PT.SystemName
			AND CE.CategoryId = 22
			LEFT JOIN [Admin].[SystemType] SysTyp
            ON SysTyp.SystemTypeId = PT.SystemTypeId
			WHERE PTE.IsRetrying = 0
			AND PTE.Comment IS NULL
			AND PTE.RecordID IS NOT NULL--Will Not be Null in Case of Retriable
		)PteSubQ
		INNER JOIN [ADMIN].[FileRegistrationTransaction] FRT
		ON PteSubQ.RecordID = FRT.RecordID
		INNER JOIN [Admin].FileRegistration FR
		ON FR.[FileRegistrationId] = FRT.[FileRegistrationId]
		LEFT JOIN [Admin].[MessageType] MT
		ON MT.MessageTypeId = PteSubQ.MessageTypeId
		WHERE FRT.StatusTypeId = 2 --Failed(Fallido)
	)SubQ
	WHERE Rnum = 1
) A

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This View is to Fetch all the records where parsing was successfull(FileRegTran) and un parsed(PTE with FRTID = null)',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'VIEW',
    @level1name = N'view_GetParsingErrors',
    @level2type = NULL,
    @level2name = NULL
