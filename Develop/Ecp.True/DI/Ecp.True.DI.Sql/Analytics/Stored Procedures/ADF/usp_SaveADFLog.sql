/*-- ==============================================================================================================================
-- Author:          Microsoft  
-- Created Date:    Feb-03-2020
-- Updated Date:	Mar-20-2020
-- <Description>:   This Procedure is to save ADF audit logs into respective tables   </Description>
-- ================================================================================================================================*/
CREATE PROCEDURE [Analytics].[usp_SaveADFLog]
	@RunId			VARCHAR (100),
	@Name			VARCHAR (100),
	@Status			VARCHAR (20),
	@ErrorMessage	VARCHAR (MAX)
AS
BEGIN
	   DECLARE  @StatusId		INT			= 0,
				@PipelineId		INT			= 0,
				@TodaysDate		DATETIME 

	   SELECT @TodaysDate		 = Admin.udf_GetTrueDate()

	   SELECT @StatusId = StatusId 
	   FROM [Analytics].AuditStatus 
	   WHERE [Status] = @Status

	   IF @StatusId = 1--InProgress
	   BEGIN
		   INSERT INTO [Analytics].[Pipelinelog] 
				(PipelineRunId,
				 PipelineName,
				 PipelineStatusId,
				 PipelineStartTime
				 )
			VALUES
				(
				 @RunId,
				 @Name,
				 @StatusId,
				 @TodaysDate
				 )
		END
		ELSE
		BEGIN   
			IF @StatusId = 2--Failed
			BEGIN
				SELECT @PipelineId = PipelineId
				FROM [Analytics].[Pipelinelog] 
				WHERE [PipelineRunId] = @RunId

				INSERT INTO [Analytics].[ErrorLog] 
					(
						[PipelineId],
						[ErrorMsg],
						[ErrorDate]
					) 
				 VALUES 
					(
						@PipelineId,
						@ErrorMessage,
						@TodaysDate
					)
			END

			--InCase of (Failed,Success)
			UPDATE [Analytics].[Pipelinelog] 
			   SET [PipelineStatusId] = @StatusId
			   	  ,[PipelineEndTime]  = @TodaysDate
			 WHERE [PipelineRunId]    = @RunId
		END
END


GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This Procedure is to save ADF audit logs into respective tables',
	@level0type = N'SCHEMA',
    @level0name = N'Analytics',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_SaveADFLog',
    @level2type = NULL,
    @level2name = NULL