/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

SELECT * FROM Admin.Ticket T
LEFT JOIN [Admin].[NodeTag] NT
		ON  T.[CategoryElementId] = [NT].[ElementId]
WHERE TicketId IN (25931, 25929) OR TicketGroupId IN ('25931', '25929')