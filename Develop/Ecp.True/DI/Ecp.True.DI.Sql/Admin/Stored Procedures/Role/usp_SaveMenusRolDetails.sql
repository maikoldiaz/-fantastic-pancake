/*-- ============================================================================================================================
-- Author:          Microsoft
-- Create date: 	Jun-11-2021
-- Description:     This Procedure is to Get details relationship between roles and application menus

-- ==============================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_SaveMenusRolDetails] 
(
	 @ExecutionId INT
)
AS
BEGIN


	-- Deleting records from [Admin].[EventsInformation] table which are older than 24 hours based on colombian timestamp
	DELETE FROM [Admin].[FeatureRoleReport]
	WHERE CreatedDate < ( SELECT [Admin].[udf_GetTrueDate] ()-1 )
			OR (ExecutionId = @ExecutionId )

	 -- VARIABLE DECLARATION
     DECLARE @Todaysdate	   DATETIME 

  -- SETTING VALUE TO VARIABLES
     SET @Todaysdate  =  [Admin].[udf_GetTrueDate] ()

  -- CREATEING TEMP TABLES
	IF OBJECT_ID('tempdb..#TempMenusRolData')IS NOT NULL
	DROP TABLE #TempMenusRolData
	
	select FR.FeatureRoleId,r.RoleName,f.Name FeatureNameId, f.[description] FeatureNameDescripcion ,@ExecutionId as ExecutionId, 'ReportUser' AS CreatedBy
	into #TempMenusRolData
	from admin.FeatureRole as FR
	inner join admin.[Role] R on R.RoleId = FR.RoleId 
	inner join admin.[Feature] f on f.FeatureId = fr.FeatureId 
	order by 1


	INSERT INTO [admin].[FeatureRoleReport] (FeatureRoleId,RoleName,FeatureNameId,FeatureNameDescripcion,ExecutionId,CreatedBy)
	SELECT FeatureRoleId,RoleName,FeatureNameId,FeatureNameDescripcion,ExecutionId,CreatedBy
	FROM #TempMenusRolData
END
