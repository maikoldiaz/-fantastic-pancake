
--Select * from Offchain.Unbalance
--Select * from Offchain.InventoryProduct
--Select * from Offchain.Movement
--Select * from Offchain.Owner
--select * from Offchain.Ownership


--Select distinct T.name from (
--Select *,OBJECT_NAME(id) as [name] from sys.syscomments 
--) T
--Where T.name like '%usp_%'
--AND text like '%Ownership%'


-- SELECT referencing_schema_name, referencing_entity_name, referencing_id, referencing_class_desc, is_caller_dependent
-- FROM sys.dm_sql_referencing_entities ('Offchain.Unbalance', 'OBJECT');

--  SELECT referencing_schema_name, referencing_entity_name, referencing_id, referencing_class_desc, is_caller_dependent
-- FROM sys.dm_sql_referencing_entities ('Offchain.InventoryProduct', 'OBJECT');

--  SELECT referencing_schema_name, referencing_entity_name, referencing_id, referencing_class_desc, is_caller_dependent
-- FROM sys.dm_sql_referencing_entities ('Offchain.Movement', 'OBJECT');

--  SELECT referencing_schema_name, referencing_entity_name, referencing_id, referencing_class_desc, is_caller_dependent
-- FROM sys.dm_sql_referencing_entities ('Offchain.Owner', 'OBJECT');

--  SELECT referencing_schema_name, referencing_entity_name, referencing_id, referencing_class_desc, is_caller_dependent
-- FROM sys.dm_sql_referencing_entities ('Offchain.Ownership', 'OBJECT');

