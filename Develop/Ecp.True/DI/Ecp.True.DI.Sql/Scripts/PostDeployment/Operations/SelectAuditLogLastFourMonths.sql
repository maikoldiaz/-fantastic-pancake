/*
 Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

SELECT 
	[User],
	LogType,
	DAY(LogDate) as [Day],
	MONTH(LogDate) as [Month],
	YEAR(LogDate) as [Year],
	COUNT(AuditLogId) as ActionCounter
FROM [Audit].AuditLog
WHERE LogDate BETWEEN '2021-09-01' AND '2022-02-28'
GROUP BY [User], LogType,DAY(LogDate), MONTH(LogDate), YEAR(LogDate)
ORDER BY YEAR(LogDate), MONTH(LogDate), DAY(LogDate), [User]

