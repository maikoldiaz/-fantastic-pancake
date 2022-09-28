/*-- ============================================================================================================================
-- Author:          Microsoft 
-- Created Date:    Dec-13-2019 
-- Updated Date:    Dec 16-2019 (Getting actual columns instead of dummy values.) 
-- Updated Date:    Dec 18-2019 (Getting measurementUnit name instead of Id.) 
-- Updated Date:    Feb 14-2020 (Get the data based on @UploadId OR @FileRegistrationTransactionGUID) 
-- Updated Date:    July 24-2020 Joined with Category Element table to get SystemName 
-- Updated Date:    Aug-06-2020  Removed Casting on CEUnit.elementid = PT.units
-- Updated Date:    Sep-10-2020  Updated to Procedure To A Proper Format
-- <Description>:   This Procedure is to get error details based on the input of Upload Id  or Record ID. </Description>
--Will get value for @UploadId in Case of NonRetriable 
--Will get value for @RecordId in Case of Retriable 
-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetErrorDetails] 
(
 @PendingTransactionId NVARCHAR(50)  = NULL,
 @RecordId             NVARCHAR(250) = NULL
) 
AS 
  BEGIN 
      IF @PendingTransactionId IS NOT NULL 
        BEGIN 
            SELECT DISTINCT STRING_AGG(CAST(PTE.errormessage AS NVARCHAR(MAX)) , '__')        AS [Error],  
                            CESystem.[Name]                             AS [SystemName],
                            SysTyp.[Name]                               AS [SystemTypeName],
                            PTE.createddate                             AS [CreationDate], 
                            PT.identifier                               AS [Identifier], 
                            RFAType.[fileactiontype]                    AS [Type], 
                            CESegment.[name]                            AS [Segment], 
                            MT.[name]                                   AS [Process], 
                            PT.volume                                   AS [Volume], 
                            CEUnit.[name]                               AS [MeasurementUnit], 
                            PT.startdate                                AS [InitialDate], 
                            PT.enddate                                  AS [FinalDate], 
                            FR.[name]                                   AS [FileName], 
                            SrcNode.[name]                              AS [SourceNode], 
                            DestNode.[name]                             AS [DestinationNode], 
                            SrcPrd.[name]                               AS [SourceProduct], 
                            DestPrd.[name]                              AS [DestinationProduct], 
                            CAST(0 As BIT)			                    AS [IsRetry], 
                            NULL				                        AS [FileRegistrationTransactionId]
            FROM   [Admin].[pendingtransaction] PT 
                   INNER JOIN [Admin].[pendingtransactionerror] PTE 
                           ON PT.transactionid = PTE.transactionid 
                   LEFT JOIN  Admin.CategoryElement CESystem
			               ON CESystem.ElementId = PT.SystemName
                           AND CESystem.CategoryId = 22
                   LEFT JOIN [Admin].[SystemType] SysTyp
                           ON SysTyp.SystemTypeId = PT.SystemTypeId
                   LEFT JOIN [Admin].[fileregistration] FR 
                           ON FR.blobpath = PT.blobname 
                   LEFT JOIN [Admin].[registerfileactiontype] RFAType 
                          ON RFAType.actiontypeid = PT.actiontypeid 
                   LEFT JOIN [Admin].[categoryelement] CESegment 
                          ON CESegment.elementid = PT.segmentid 
                   LEFT JOIN [Admin].[messagetype] MT 
                          ON MT.messagetypeid = PT.messagetypeid 
                   LEFT JOIN [Admin].[node] SrcNode 
                          ON SrcNode.nodeid = CAST(PT.sourcenodeid AS INT) 
                   LEFT JOIN [Admin].[node] DestNode 
                          ON DestNode.nodeid = CAST(PT.destinationnodeid AS INT) 
                   LEFT JOIN [Admin].[product] SrcPrd 
                          ON SrcPrd.productid = PT.sourceproductid 
                   LEFT JOIN [Admin].[product] DestPrd 
                          ON DestPrd.productid = PT.destinationproductid 
                   LEFT JOIN [Admin].[categoryelement] CEUnit 
                          ON CEUnit.elementid = PT.units 
            WHERE  ISNULL(PT.transactionid, '') = CASE WHEN @PendingTransactionId IS NOT NULL 
                                                       THEN @PendingTransactionId 
                                                       ELSE ISNULL(PT.transactionid, '') 
                                                       END
            AND PTE.IsRetrying = 0
	        AND PTE.Comment IS NULL
            GROUP BY  CESystem.[Name] 
                     ,SysTyp.[Name] 
                     ,PTE.createddate                            
                     ,PT.identifier                             
                     ,RFAType.[fileactiontype]                  
                     ,CESegment.[name]                          
                     ,MT.[name]                                 
                     ,PT.volume                                 
                     ,CEUnit.[name]                             
                     ,PT.startdate                              
                     ,PT.enddate                                
                     ,FR.[name]                                 
                     ,SrcNode.[name]                            
                     ,DestNode.[name]                           
                     ,SrcPrd.[name]                             
                     ,DestPrd.[name]  
                     ,PT.transactionid
        END 
        ELSE IF @RecordId IS NOT NULL 
        BEGIN
        
            SELECT [Error], 
                   [SystemName], 
                   [SystemTypeName],
                   [CreationDate], 
                   [Identifier], 
                   [Type], 
                   [Segment], 
                   [Process], 
                   [Volume], 
                   [MeasurementUnit], 
                   [InitialDate], 
                   [FinalDate], 
                   [FileName], 
                   [SourceNode], 
                   [DestinationNode], 
                   [SourceProduct], 
                   [DestinationProduct], 
                   [IsRetry], 
                   [FileRegistrationTransactionId]
            FROM 
            (
                    SELECT DISTINCT STRING_AGG(CAST(PTE.errormessage AS NVARCHAR(MAX)), '__')        AS [Error], 
                                    CESystem.[Name]                             AS [SystemName], 
                                    SysTyp.[Name]                               AS [SystemTypeName],
                                    PTE.CreatedDate                             AS [CreationDate], 
                                    PT.identifier                               AS [Identifier], 
                                    RFAType.[fileactiontype]                    AS [Type], 
                                    CESegment.[name]                            AS [Segment], 
                                    MT.[name]                                   AS [Process], 
                                    PT.volume                                   AS [Volume], 
                                    CEUnit.[name]                               AS [MeasurementUnit], 
                                    PT.startdate                                AS [InitialDate], 
                                    PT.enddate                                  AS [FinalDate], 
                                    FR.[name]                                   AS [FileName], 
                                    SrcNode.[name]                              AS [SourceNode], 
                                    DestNode.[name]                             AS [DestinationNode], 
                                    SrcPrd.[name]                               AS [SourceProduct], 
                                    DestPrd.[name]                              AS [DestinationProduct], 
                                    CAST(1 As BIT)								AS [IsRetry], 
                                    FRT.FileRegistrationTransactionId			AS [FileRegistrationTransactionId],
                                    ROW_NUMBER()OVER(PARTITION BY PTE.RecordID ORDER BY PTE.CreatedDate DESC)Rnum
                    FROM   [Admin].[pendingtransaction] PT 
                    INNER JOIN [Admin].[pendingtransactionerror] PTE 
                    ON PT.transactionid = PTE.transactionid 
                    LEFT JOIN  Admin.CategoryElement CESystem
			        ON CESystem.ElementId = PT.SystemName
                    AND CESystem.CategoryId = 22
                    LEFT JOIN [Admin].[SystemType] SysTyp
                    ON SysTyp.SystemTypeId = PT.SystemTypeId
                    INNER JOIN [Admin].[fileregistrationtransaction] FRT 
                    ON PTE.recordid = FRT.recordid 
                    INNER JOIN [Admin].[fileregistration] FR 
                    ON FR.fileregistrationid = FRT.fileregistrationid 
                    LEFT JOIN [Admin].[registerfileactiontype] RFAType 
                    ON RFAType.actiontypeid = PT.actiontypeid 
                    LEFT JOIN [Admin].[categoryelement] CESegment 
                    ON CESegment.elementid = PT.segmentid 
                    LEFT JOIN [Admin].[messagetype] MT 
                    ON MT.messagetypeid = PT.messagetypeid 
                    LEFT JOIN [Admin].[node] SrcNode 
                    ON SrcNode.nodeid = CAST(PT.sourcenodeid AS INT) 
                    LEFT JOIN [Admin].[node] DestNode 
                    ON DestNode.nodeid = CAST(PT.destinationnodeid AS INT) 
                    LEFT JOIN [Admin].[product] SrcPrd 
                    ON SrcPrd.productid = PT.sourceproductid 
                    LEFT JOIN [Admin].[product] DestPrd 
                    ON DestPrd.productid = PT.destinationproductid 
                    LEFT JOIN [Admin].[categoryelement] CEUnit 
                    ON CEUnit.elementid = PT.units 
                    WHERE  ISNULL(PTE.recordid, '') = CASE WHEN @RecordID IS NOT NULL 
                                                           THEN @RecordID 
                                                           ELSE ISNULL(PTE.recordid, '') 
                                                           END 
                    AND PTE.IsRetrying = 0
	                AND PTE.Comment IS NULL
                    GROUP BY   CESystem.[Name] 
                              ,SysTyp.[Name] 
                              ,PTE.CreatedDate                  
                              ,PT.identifier                    
                              ,RFAType.[fileactiontype]         
                              ,CESegment.[name]                 
                              ,MT.[name]                        
                              ,PT.volume                        
                              ,CEUnit.[name]                    
                              ,PT.startdate                     
                              ,PT.enddate                       
                              ,FR.[name]                        
                              ,SrcNode.[name]                   
                              ,DestNode.[name]                  
                              ,SrcPrd.[name]                    
                              ,DestPrd.[name] 					
                              ,FRT.FileRegistrationTransactionId
                              ,PTE.RecordID
            )A
            WHERE Rnum = 1
        END 
      ELSE 
        BEGIN 
            RAISERROR ('Invalid ErrorId',1,1) 
        END 
  END 

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to get error details based on the input of Upload Id  or Record ID.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_GetErrorDetails',
    @level2type = NULL,
    @level2name = NULL