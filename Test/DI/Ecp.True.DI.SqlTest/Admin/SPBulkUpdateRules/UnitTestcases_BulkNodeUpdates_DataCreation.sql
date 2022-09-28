/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Mar-09-2020
-- Description:     This data generation is for SP usp_BulkUpdateRules
-- Database backup Used:	appdb_dev_0309 (Backup of dev on local)
-- ==============================================================================================================================*/


-- Data Insertion into Rule tables --
-- NodeConnectionProductRule--
INSERT INTO admin.NodeConnectionProductRule VALUES( 2,'NCPRule2',1,'system',GETDATE(),null,null)
INSERT INTO admin.NodeConnectionProductRule VALUES( 3,'NCPRule3',1,'system',GETDATE(),null,null)
INSERT INTO admin.NodeConnectionProductRule VALUES( 4,'NCPRule4',1,'system',GETDATE(),null,null)
--NodeOwnershipRule --
INSERT INTO admin.NodeOwnershipRule VALUES( 2,'OwnershipRule2',1,'system',GETDATE(),null,null)
INSERT INTO admin.NodeOwnershipRule VALUES( 3,'OwnershipRule3',1,'system',GETDATE(),null,null)
INSERT INTO admin.NodeOwnershipRule VALUES( 4,'OwnershipRule4',1,'system',GETDATE(),null,null)
--NodeProductRule
INSERT INTO admin.NodeProductRule VALUES( 2,'NPRule2',1,'system',GETDATE(),null,null)
INSERT INTO admin.NodeProductRule VALUES( 3,'NPRule3',1,'system',GETDATE(),null,null)
INSERT INTO admin.NodeProductRule VALUES( 4,'NPRule4',1,'system',GETDATE(),null,null)

-- Check Rule tables for Insertion --

select * from admin.NodeConnectionProductRule
/*
RuleId	RuleName	IsActive	CreatedBy	CreatedDate				LastModifiedBy	LastModifiedDate
2		NCPRule2	1			system		2020-03-0912:22:02.330	NULL			NULL
3		NCPRule3	1			system		2020-03-0912:22:02.330	NULL			NULL
4		NCPRule4 	1			system		2020-03-0912:22:02.330	NULL			NULL
*/
select * from admin.NodeOwnershipRule
/*
RuleId	RuleName		IsActive	CreatedBy	CreatedDate				LastModifiedBy	LastModifiedDate
2		OwnershipRule2	1			system		2020-03-0912:22:02.330	NULL			NULL
3		OwnershipRule3	1			system		2020-03-0912:22:02.330	NULL			NULL
4		OwnershipRule4	1			system		2020-03-0912:22:02.330	NULL			NULL
*/
select * from admin.NodeProductRule
/*RuleId	RuleName	IsActive	CreatedBy	CreatedDate	LastModifiedBy	LastModifiedDate
2	NPRule2	1	system	2020-03-09 12:22:02.330	NULL	NULL
3	NPRule3	1	system	2020-03-09 12:22:02.330	NULL	NULL
*/