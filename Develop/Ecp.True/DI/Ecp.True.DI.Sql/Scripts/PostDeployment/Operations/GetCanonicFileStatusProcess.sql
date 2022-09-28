/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
This file contains SQL statements that will be appended to the build script.		
Use SQLCMD syntax to include a file in the post-deployment script.			
Example:      :r .\myfile.sql								
Use SQLCMD syntax to reference a variable in the post-deployment script.		
Example:      :setvar TableName MyTable							
            SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/



SELECT Fr.* 
FROM [Admin].FileRegistration Fr
left join (SELECT FileRegistrationId, COUNT(StatusTypeId) cant
						FROM Admin.FileRegistrationTransaction group by FileRegistrationId ) Frt on Frt.FileRegistrationId = Fr.FileRegistrationId
where fr.SystemTypeId = 3 and isnull(Frt.cant,0) = 0 and cast(UploadDate as date) between '2022-03-15' and cast(getdate() as date)

