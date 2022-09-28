/*-- ==============================================================================================================================
-- Author:				InterGrupo
-- Created date:		Jun-16-2021
-- Description:			This Procedure is used to get the User and Group.
 ================================================================================================================================*/
CREATE PROCEDURE [Admin].[usp_GetUserGroup] 
(
		 @ExecutionId			INT,
		 @userGroup				Admin.[UserRoleType] READONLY
)
AS 
BEGIN

		-- Deleting records from [Admin].[UserRoleReport] table which are older than 24 hours based on colombian timestamp
		DELETE FROM [Admin].[UserRoleReport]
		WHERE CreatedDate < (SELECT [Admin].[udf_GetTrueDate] ()-1 )
		OR (ExecutionId = @ExecutionId )


		insert into [Admin].[UserRoleReport] ([UserId],[Name],[UserType],[Email],[Role],[FeatureNameDescripcion],[FeatureNameId],executionID,CreatedBy)
		select UR.[UserId],UR.[Name],
		case when CHARINDEX('Guest',UR.[UserType]) >0 then 'Invitado'
			 when CHARINDEX('Member',UR.[UserType]) >0 then 'Miembro' end [UserType] 
		,UR.Email,r.RoleName,f.description,f.Name,@ExecutionId ,'ReportUser'
		from @userGroup UR
		inner join admin.[role] R on UR.RoleId = R.roleid 
		inner join admin.FeatureRole  FR on UR.[Roleid] = FR.RoleId
		inner join admin.Feature F on FR.FeatureId = F.FeatureId
		

END
