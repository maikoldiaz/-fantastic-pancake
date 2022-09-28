﻿--/*
--The purpose of this script is to generate the Delete/Select statements of all tables based on their dependency on each other.
--The delete sequence is such that we can easily run them in the generated sequence.
--*/

--;WITH DependencyList 
--     AS (SELECT DISTINCT ChildTable = onTableSchema.name+'.'+OnTable.NAME,--OnTable.NAME, 
--                ParentTable = AgainstTableSchema.name+'.'+AgainstTable.NAME			
--		 FROM   sysforeignkeys fk 
--		 INNER JOIN sys.objects onTable
--		 ON Fk.fkeyid = onTable.object_id
--		 INNER JOIN sys.schemas onTableSchema
--		 ON onTable.schema_id = onTableSchema.schema_id
--		 INNER JOIN sys.objects AgainstTable
--		 ON Fk.rkeyid = AgainstTable.object_id
--		 INNER JOIN sys.schemas AgainstTableSchema
--		 ON AgainstTable.schema_id = AgainstTableSchema.schema_id
--		 AND onTableSchema.name+'.'+OnTable.NAME <> AgainstTableSchema.name+'.'+AgainstTable.NAME	
--		), 
--     UserDefinedList 
--     AS (SELECT ChildTable = onTableSchema.name+'.'+OnTable.NAME,
--				ParentTable = DependencyList.ParentTable 
--		 FROM   sys.objects onTable
--		 INNER JOIN sys.schemas onTableSchema
--		 ON onTable.schema_id = onTableSchema.schema_id
--		 LEFT JOIN DependencyList 
--		 ON onTableSchema.name+'.'+OnTable.NAME = DependencyList.ChildTable 
--		 WHERE  1 = 1 
--         AND onTable.type = 'U' 
--         AND onTable.NAME NOT LIKE 'sys%'
--		 ),
--     LoopData 
--     AS ( 
--        -- base case 
--        SELECT TableName = ChildTable, 
--               Lvl = 1 
--        FROM   UserDefinedList 
--        WHERE  1 = 1 
--               AND ParentTable IS NULL 
--         -- recursive case 
--         UNION ALL 
--         SELECT TableName = ChildTable, 
--                Lvl = r.lvl + 1 
--         FROM   UserDefinedList d 
--                INNER JOIN LoopData r 
--                        ON d.ParentTable = r.tablename) 
--SELECT Lvl				= Max(lvl), 
--       tablename, 
--       strSql			= 'DELETE FROM ' + tablename + '' ,
--	   ReSeedScript		= 'DBCC CHECKIDENT ('''+tablename+''', RESEED, 0)',
--	   CountOfRecords	= 'SELECT COUNT(1) AS '+Replace(tablename,'.','_')+' FROM '+ tablename + '' ,
--	   DataOfTable		= 'SELECT * FROM '+ tablename + '' 
--FROM   LoopData 
--GROUP  BY tablename 
--ORDER  BY 1 DESC, 
--          2 DESC 
--	   --Select To Give Count of Records
--	   --Select To Data of Records
