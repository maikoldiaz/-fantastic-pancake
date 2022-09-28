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


select fr.FileRegistrationId, frt.FileRegistrationTransactionId,m.MovementId, 
fr.UploadId,
fr.UploadDate,m.TransactionHash,m.BlockchainMovementTransactionId
from admin.FileRegistration fr
left join admin.FileRegistrationTransaction frt on frt.fileregistrationid = fr.FileRegistrationId 
left join Offchain.Movement m on m.FileRegistrationTransactionId = frt.FileRegistrationTransactionId
where fr.SystemTypeId = 7 and
cast(UploadDate as date) >= '20220317' order by UploadDate desc
