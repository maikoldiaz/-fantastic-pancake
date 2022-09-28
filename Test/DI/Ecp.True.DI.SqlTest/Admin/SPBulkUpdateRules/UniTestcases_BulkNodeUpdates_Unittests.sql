
/*-- ============================================================================================================================
-- Author:          Microsoft
-- Created date: 	Mar-09-2020
-- Description:     These are test cases for SP usp_BulkUpdateRules
-- Database backup Used:	appdb_dev_0309 (Backup of dev on local)
-- ==============================================================================================================================*/

-- Common Declare statements to be used with each of the individual test cases.
DECLARE @BulkUploadIds Admin.[KeyType]
DECLARE @VariableTypeIds Admin.[KeyType]
DECLARE @typeOftable NVARCHAR(100) -- Node,NodeConnectionProduct,StorageLocationProduct,StorageLocationProductVariable
DECLARE @ruleid INT 
DECLARE @Result INT
DECLARE @lastmodif nvarchar(260)
set @lastmodif='Riya'


------ CASE 1: NO BULK UPLOAD IDS PASSED IN TABLE TYPE VARIABLE
------ EXPECTED BEHAVIOR: It should fail with  'No ids to update' exception
----                        RETURN VALUE = 0

SET @typeOftable ='node'
SET @ruleid =1
EXEC @Result=admin.usp_BulkUpdateRules @BulkUploadIds, @VariableTypeIds, @typeOftable,@ruleid,@lastmodif
select @Result

SET @typeOftable ='NodeConnectionProduct'
SET @ruleid =2
EXEC @Result=admin.usp_BulkUpdateRules @BulkUploadIds, @VariableTypeIds, @typeOftable,@ruleid,@lastmodif
select @Result

SET @typeOftable ='StorageLocationProduct'
SET @ruleid =2
EXEC @Result=admin.usp_BulkUpdateRules @BulkUploadIds, @VariableTypeIds, @typeOftable,@ruleid,@lastmodif
select @Result

-- ========= CASE 2: UPDATE TABLES WITH RULEIDS WHEN PASSED CORRECT BULK UPLOAD IDS ==============
--NODE IDS  (5920,5921,5922) WITH RULEID=2 
--NODE CONECTION PRODUCT IDS (3547,3548,3549) WITH RULEID=3
--STORAGE LOCATION PRODUCT ID (7609,7610,7611) WITH RULEID=2
-- EXPECTED BEHAVIOUR: These nodeids should have update Ruleids in respected tables 
--					   Return Code should be 1
-----### NODE #####-----
INSERT INTO @BulkUploadIds VALUES (5920)
INSERT INTO @BulkUploadIds VALUES (5921)
INSERT INTO @BulkUploadIds VALUES (5922)

SET @typeOftable ='node'
SET @ruleid =2
EXEC @Result=admin.usp_BulkUpdateRules @BulkUploadIds, @VariableTypeIds, @typeOftable,@ruleid,@lastmodif
select @Result
delete from @BulkUploadIds
select * from admin.node where NodeId in (5920,5921,5922)
-- #### NodeConnectionProduct ####-----

INSERT INTO @BulkUploadIds VALUES (3547)
INSERT INTO @BulkUploadIds VALUES (3548)
INSERT INTO @BulkUploadIds VALUES (3549)
SET @typeOftable ='NodeConnectionProduct'
SET @ruleid =3
EXEC @Result=admin.usp_BulkUpdateRules @BulkUploadIds, @VariableTypeIds, @typeOftable,@ruleid,@lastmodif
select @Result
delete from @BulkUploadIds
select * from admin.NodeConnectionProduct where NodeConnectionProductId in (3547,3548,3549)

----- #### StorageLocationProduct ###-----
INSERT INTO @BulkUploadIds VALUES (7609)
INSERT INTO @BulkUploadIds VALUES (7610)
INSERT INTO @BulkUploadIds VALUES (7611)
SET @typeOftable ='StorageLocationProduct'
SET @ruleid =2
EXEC @Result=admin.usp_BulkUpdateRules @BulkUploadIds, @VariableTypeIds, @typeOftable,@ruleid,@lastmodif
select @Result
delete from @BulkUploadIds
select * from admin.StorageLocationProduct where StorageLocationProductId in (7609,7610,7611)



---- ========= CASE 3: UPDATE NODE TABLE WITH NODE IDS  (5920,5921,5922) WITH RULEID=10(Something not present in NodeOwnershipRule table ) ==============
---- EXPECTED BEHAVIOUR: It should fail with foreign key constraint error
----					   Return Code should be 0
INSERT INTO @BulkUploadIds VALUES (5920)
INSERT INTO @BulkUploadIds VALUES (5921)
INSERT INTO @BulkUploadIds VALUES (5922)
 
--select * from @BulkUploadIds
SET @typeOftable ='node'
SET @ruleid =10
EXEC @Result=admin.usp_BulkUpdateRules @BulkUploadIds, @VariableTypeIds, @typeOftable,@ruleid,@lastmodif
select @Result

-- ========= CASE 4: Pass wrong value in typeOftable ==============
-- EXPECTED BEHAVIOUR: 'It should fail with invalid Type of table to be updated' exception
--					   Return Code should be 0
INSERT INTO @BulkUploadIds VALUES (5920)
INSERT INTO @BulkUploadIds VALUES (5921)
INSERT INTO @BulkUploadIds VALUES (5922)
 
--select * from @BulkUploadIds
SET @typeOftable ='node_wrong'
SET @ruleid =1
EXEC @Result=admin.usp_BulkUpdateRules @BulkUploadIds, @VariableTypeIds, @typeOftable,@ruleid,@lastmodif
select @Result


/* ========= CASE 5: Insert into empty StorageLocationProductVariable ==============
-- EXPECTED BEHAVIOUR: 'It update the [Admin].[StorageLocationProduct] with the given ruleId;
--                      And it should also insert/delete [Admin].[StorageLocationProductVariable] based on the incoming variable Ids.
--                      And it is also expected that in this case there will be only one StorageLocationProductId in @BulkUploadIds
--	
-- Expeted Output: 
        - Return Code should be 1.
        - There should be three new entries into [StorageLocationProductVariable] with vaues -> (1,1), (1,2), (1,3).
*/
INSERT INTO @BulkUploadIds VALUES (1)

INSERT INTO @VariableTypeIds VALUES (1),(2),(3);

SET @typeOftable ='StorageLocationProductVariable'
SET @ruleid =1
EXEC @Result=admin.usp_BulkUpdateRules @BulkUploadIds, @VariableTypeIds, @typeOftable, @ruleid, @lastmodif
select @Result;
Select * from admin.StorageLocationProduct Where StorageLocationProductId In (Select [Key] From @BulkUploadIds);
Select * from Admin.StorageLocationProductVariable;



/* ========= CASE 6: Update StorageLocationProductVariable; In continuation with Case (5) ==============
-- EXPECTED BEHAVIOUR: 'It update the [Admin].[StorageLocationProduct] with the given ruleId;
--                      And it should also insert/delete [Admin].[StorageLocationProductVariable] based on the incoming variable Ids.
--                      And it is also expected that in this case there will be only one StorageLocationProductId in @BulkUploadIds
--	
-- Expeted Output: 
        - Return Code should be 1.
        - There should be one entry removed form  [StorageLocationProductVariable] with vaues -> (1,2).
*/
INSERT INTO @BulkUploadIds VALUES (1)

INSERT INTO @VariableTypeIds VALUES (1),(3);

SET @typeOftable ='StorageLocationProductVariable'
SET @ruleid =2
EXEC @Result=admin.usp_BulkUpdateRules @BulkUploadIds, @VariableTypeIds, @typeOftable, @ruleid, @lastmodif
select @Result;
Select * from admin.StorageLocationProduct Where StorageLocationProductId In (Select [Key] From @BulkUploadIds);
Select * from Admin.StorageLocationProductVariable;



/* ========= CASE 7: Update StorageLocationProductVariable; In continuation with Case (6) ==============
-- EXPECTED BEHAVIOUR: 'It update the [Admin].[StorageLocationProduct] with the given ruleId;
--                      And it should also insert/delete [Admin].[StorageLocationProductVariable] based on the incoming variable Ids.
--                      And it is also expected that in this case there will be only one StorageLocationProductId in @BulkUploadIds
--	
-- Expeted Output: 
        - Return Code should be 1.
        - There should be three more entries added to [StorageLocationProductVariable] with vaues -> (2,1), (2,2), (2,3).
*/
INSERT INTO @BulkUploadIds VALUES (2);

INSERT INTO @VariableTypeIds VALUES (1),(2),(3);

SET @typeOftable ='StorageLocationProductVariable'
SET @ruleid = 1
EXEC @Result=admin.usp_BulkUpdateRules @BulkUploadIds, @VariableTypeIds, @typeOftable, @ruleid, @lastmodif
select @Result;
Select * from admin.StorageLocationProduct Where StorageLocationProductId In (Select [Key] From @BulkUploadIds);
Select * from Admin.StorageLocationProductVariable;



/* ========= CASE 8: Update StorageLocationProductVariable; In continuation with Case (7) ==============
-- EXPECTED BEHAVIOUR: 'It update the [Admin].[StorageLocationProduct] with the given ruleId;
--                      And it should also insert/delete [Admin].[StorageLocationProductVariable] based on the incoming variable Ids.
--                      And it is also expected that in this case there will be only one StorageLocationProductId in @BulkUploadIds
--	
-- Expeted Output: 
        - Return Code should be 1.
        - There should be no change to [StorageLocationProductVariable] and only Ruleid should change into [StorageLocationProduct].
*/
INSERT INTO @BulkUploadIds VALUES (2);

INSERT INTO @VariableTypeIds VALUES (1),(2),(3);

SET @typeOftable ='StorageLocationProductVariable'
SET @ruleid = 2
EXEC @Result=admin.usp_BulkUpdateRules @BulkUploadIds, @VariableTypeIds, @typeOftable, @ruleid, @lastmodif
select @Result;
Select * from admin.StorageLocationProduct Where StorageLocationProductId In (Select [Key] From @BulkUploadIds);
Select * from Admin.StorageLocationProductVariable;



/* ========= CASE 9: Update StorageLocationProductVariable; In continuation with Case (8) ==============
-- EXPECTED BEHAVIOUR: 'It update the [Admin].[StorageLocationProduct] with the given ruleId;
--                      And it should also insert/delete [Admin].[StorageLocationProductVariable] based on the incoming variable Ids.
--                      And it is also expected that in this case there will be only one StorageLocationProductId in @BulkUploadIds
--	
-- Expeted Output: 
        - Return Code should be 1.
        - All the variables associated with StorageLocationProductId = 1, should be removed. And remaining {(2,1), (2,2), (2,3)} should remain as it is.
*/
INSERT INTO @BulkUploadIds VALUES (1);

--INSERT INTO @VariableTypeIds VALUES (1),(2),(3);

SET @typeOftable ='StorageLocationProductVariable'
SET @ruleid = 1
EXEC @Result=admin.usp_BulkUpdateRules @BulkUploadIds, @VariableTypeIds, @typeOftable, @ruleid, @lastmodif
select @Result;
Select * from admin.StorageLocationProduct Where StorageLocationProductId In (Select [Key] From @BulkUploadIds);
Select * from Admin.StorageLocationProductVariable;

