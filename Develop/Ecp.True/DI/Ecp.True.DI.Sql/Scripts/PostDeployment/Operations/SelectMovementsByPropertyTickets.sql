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

 SELECT * FROM Offchain.Movement WHERE OwnershipTicketId IN (25978,25980,25982,25987,25990,25993)