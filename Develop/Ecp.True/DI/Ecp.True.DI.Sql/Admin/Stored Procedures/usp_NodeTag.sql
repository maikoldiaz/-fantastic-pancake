/*-- ===================================================================================================================================================================
-- Author:			Microsoft  
-- Created Date:	Nov-11-2019
-- Updated Date:	Mar-20-2020
-- <Description>:	This SP is used to Insert, Update or Expire the existing NodeTags based on the given ElementId, InputDate and List of NodeTags.  </Description>
-- ===================================================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_NodeTag]
(
    @OperationType	INT = 0,			--> 1 = Insert; 2 = Update; 3 = Expire
	@ElementId		INT = 0,
	@InputDate		DATETIME,
    @NodeTag [Admin].[NodeTagType] READONLY
	
)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION

			DECLARE @todaysDate DATETIME = (select DATEADD(dd, DATEDIFF(dd, 0, Admin.udf_GetTrueDate()), 0));

			IF (@OperationType = 1)		--> Insert
			BEGIN

				-- Validation1: StartDate cannot be less than today's date.
				IF (@InputDate < @todaysDate)
					THROW 53001, 'StartDateLessThanCurrentDate', 1


				-- Validation2: Check if already same combination of NodeId and CategoryId exists or not.
				IF EXISTS (SELECT 'X'
							FROM [Admin].[view_NodeTagWithCategoryId] x
							INNER JOIN (Select * from [Admin].[udf_NodeTagWithCategoryId](@OperationType, @ElementId, @InputDate, @NodeTag)) y 
							ON	x.NodeId = y.NodeId 
								AND x.CategoryId = y.CategoryId
							WHERE	x.EndDate >= @todaysDate)
					THROW 53002, 'UNIQUENESS_FAILED' , 2


				-- Validation3: Time range consistency: 
			--					A node can end open, but combinations that end in effect must ensure that they do not overlap over time. 
			--					There may even be empty periods (no grouping in effect), 
			--					but never in time can a node have two conflicting lifetimes for the same category.

				IF EXISTS (
							SELECT 'X'
							FROM [Admin].[view_NodeTagWithCategoryId] db
							INNER JOIN (Select * from [Admin].[udf_NodeTagWithCategoryId](@OperationType, @ElementId, @InputDate, @NodeTag)) DT
							ON db.NodeId = DT.NodeID
							AND db.categoryId = DT.CategoryID
							WHERE DT.StartDate  Between DB.StartDate and DB.EndDate
				)
					THROW 53003, 'TIME_OVERLAP' , 3

				INSERT INTO [Admin].[NodeTag] ([NodeId], [ElementId], [StartDate], [EndDate], [CreatedBy], [CreatedDate])
					SELECT [NodeId], @ElementId, @InputDate as [StartDate], cast('9999-12-31 00:00:00.000' as datetime) as [EndDate], [CreatedBy], [CreatedDate] FROM @NodeTag;
			END

			ELSE IF (@OperationType = 2)		--> Update
			BEGIN

				-- Validation4: Verifying if the new Element belong to the same category or not. 
				--				This was needed because we need to handle the special case of default Elements associated with Node.

					DECLARE @newCategoryId INT = (Select CategoryId from Admin.CategoryElement WHERE ElementId = @ElementId);

					IF EXISTS (Select NodeTagId, ElementId, CategoryId, @newCategoryId as NewCategoryId from [Admin].[view_NodeTagWithCategoryId]
								WHERE	NodeTagId IN (SELECT NodeTagId FROM @NodeTag)
									AND CategoryId <> @newCategoryId)

								THROW 53004, 'CategoryTypeShouldBeSame', 4
					
					
					-- Validation5-6: Time range consistency: 
					--					A node can end open, but combinations that end in effect must ensure that they do not overlap over time. 
					--					There may even be empty periods (no grouping in effect), 
					--					but never in time can a node have two conflicting lifetimes for the same category.
					
					IF EXISTS (Select x.NodeId, x.ElementId, x.StartDate, x.EndDate, @InputDate as NewEndDate
								FROM [Admin].[view_NodeTagWithCategoryId] x
								INNER JOIN 
								(Select * from [Admin].[udf_NodeTagWithCategoryId](@OperationType, @ElementId, @InputDate, @NodeTag)) y 
								ON x.NodeId = y.NodeId AND x.categoryId = y.CategoryId
								WHERE x.NodeTagId NOT IN (Select NodetagId FROM @NodeTag)
								AND (@InputDate BETWEEN x.StartDate and x.EndDate))
									
						THROW 53005, 'TIME_OVERLAP', 5
					ELSE IF EXISTS (
									SELECT 'X'
									FROM [Admin].[view_NodeTagWithCategoryId] NT
									INNER JOIN (Select * from [Admin].[udf_NodeTagWithCategoryId](@OperationType, @ElementId, @InputDate, @NodeTag)) T1
									ON NT.NodeId = T1.NodeId
									And NT.CategoryId = T1.CategoryId
									WHERE T1.NodeTagId IN (SELECT NodeTagId FROM @NodeTag)
									AND NT.NodeTagId IN (SELECT NodeTagId FROM @NodeTag)
									AND @InputDate < NT.EndDate
									AND NT.EndDate <> '9999-12-31 00:00:00.000'
									AND T1.StartDate between NT.StartDate and NT.EndDate
								)
							THROW 53006, 'TIME_OVERLAP', 6
					

					-- Validation7: EndDateLessThanStartDate: 
					ELSE IF EXISTS (SELECT 'X'
									FROM [Admin].[view_NodeTagWithCategoryId] NT
									INNER JOIN (Select * from [Admin].[udf_NodeTagWithCategoryId](@OperationType, @ElementId, @InputDate, @NodeTag)) T1
									ON NT.NodeId = T1.NodeId
									And NT.CategoryId = T1.CategoryId
									WHERE @InputDate < NT.StartDate)
								THROW 53007, 'EndDateLessThanStartDate', 7

					DECLARE @endDateMinusOne DATETIME = DATEADD(dd, -1, @InputDate);
					UPDATE x SET EndDate = @endDateMinusOne,
									LastModifiedBy = y.LastModifiedBy,
									LastModifiedDate = y.LastModifiedDate
							FROM [Admin].[NodeTag] x
							INNER JOIN @NodeTag y
							ON x.NodeTagId = y.NodeTagId
							WHERE x.EndDate = '9999-12-31 00:00:00.000'


					INSERT INTO [Admin].[NodeTag]
								([NodeId]
								,[ElementId]
								,[StartDate]
								,[EndDate]
								,[CreatedBy]
								,[CreatedDate])
							SELECT [NodeId], @ElementId, @InputDate as [StartDate], cast('9999-12-31 00:00:00.000' as datetime) as [EndDate], [CreatedBy], [CreatedDate] FROM @NodeTag;


					/* Scenarios handled differently:
					1. We cannot Update the same combination of Node-Category if there is already an active record exists when the NewStartDate is less than the existing Stardate in future.
					2. 
						*/
			END

			ELSE IF (@OperationType = 3)		--> Set Expire
			BEGIN	
					-- Validation8: Verify if not updating the default (special) elements.
					IF EXISTS (SELECT x.CategoryId FROM (Select * from [Admin].[udf_NodeTagWithCategoryId](@OperationType, @ElementId, @InputDate, @NodeTag)) x
								WHERE x.CategoryId IN (1, 2, 3))
						THROW 53008, 'CannotExpireDefaultCategories', 8

					-- Validation9: EndDate cannot be less than today's date.
					SET @todaysDate = (select DATEADD(dd, DATEDIFF(dd, 0, Admin.udf_GetTrueDate()), 0));
					IF (@InputDate < @todaysDate)
						THROW 53009, 'EndDateLessThanCurrentDate', 9
					

					-- Validation10-11: TIME_OVERLAP
					IF EXISTS (SELECT 'X'
								FROM [Admin].[view_NodeTagWithCategoryId] x
								INNER JOIN 
								(Select * from [Admin].[udf_NodeTagWithCategoryId](@OperationType, @ElementId, @InputDate, @NodeTag)) y 
								ON x.NodeId = y.NodeId AND x.categoryId = y.CategoryId
								WHERE x.NodeTagId NOT IN (Select NodetagId FROM @NodeTag)
								AND (@InputDate BETWEEN x.StartDate and x.EndDate))
									
						THROW 53010, 'TIME_OVERLAP', 10
						
					ELSE IF EXISTS (
									SELECT 'X'
									FROM [Admin].[view_NodeTagWithCategoryId] NT
									INNER JOIN (Select * from [Admin].[udf_NodeTagWithCategoryId](@OperationType, @ElementId, @InputDate, @NodeTag)) T1
									ON NT.NodeId = T1.NodeId
									And NT.CategoryId = T1.CategoryId
									WHERE T1.NodeTagId IN (SELECT NodeTagId FROM @NodeTag)
									AND NT.NodeTagId NOT IN (SELECT NodeTagId FROM @NodeTag)
									AND @InputDate > NT.EndDate
									AND NT.EndDate <> '9999-12-31 00:00:00.000'
									AND T1.StartDate Not between NT.StartDate and NT.EndDate
									AND T1.StartDate <= NT.EndDate
								)
							THROW 53011, 'TIME_OVERLAP', 11


					-- Validation12: EndDateLessThanStartDate: 
					ELSE IF EXISTS (SELECT 'X'
									FROM [Admin].[view_NodeTagWithCategoryId] NT
									INNER JOIN (Select * from [Admin].[udf_NodeTagWithCategoryId](@OperationType, @ElementId, @InputDate, @NodeTag)) T1
									ON NT.NodeId = T1.NodeId
									And NT.CategoryId = T1.CategoryId
									WHERE @InputDate < NT.StartDate)
								THROW 53012, 'EndDateLessThanStartDate', 12

					ELSE
					BEGIN

						UPDATE [Admin].[NodeTag]
						SET  EndDate				= @InputDate
							,LastModifiedBy			= Tbl.LastModifiedBy
							,LastModifiedDate		= Tbl.LastModifiedDate
						FROM [Admin].[NodeTag] NT
						INNER JOIN @NodeTag Tbl
						ON NT.NodeTagId = Tbl.NodeTagId

					END

			END

		COMMIT TRANSACTION
    END TRY
    
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW
    END CATCH
END

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This SP is used to Insert, Update or Expire the existing NodeTags based on the given ElementId, InputDate and List of NodeTags.',
	@level0type = N'SCHEMA',
    @level0name = N'Admin',
    @level1type = N'PROCEDURE',
    @level1name = N'usp_NodeTag',
    @level2type = NULL,
    @level2name = NULL