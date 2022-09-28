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




SELECT TIC.*,ST.StatusType 
FROM ADMIN.Ticket TIC
INNER JOIN ADMIN.StatusType ST
	ON ST.StatusTypeId = TIC.Status
WHERE (TIC.TicketTypeId in (3,6))  AND CAST(TIC.CreatedDate AS DATE) >= CAST('2022-03-01' AS DATE)
