﻿--SELECT DISTINCT TAB.TABLE_SCHEMA,TAB.TABLE_NAME,COL.COLUMN_NAME,COL.DATA_TYPE,COL.NUMERIC_PRECISION,COL.NUMERIC_SCALE 
--  FROM INFORMATION_SCHEMA.COLUMNS COL
--  JOIN INFORMATION_SCHEMA.TABLES TAB
--    ON COL.TABLE_SCHEMA = TAB.TABLE_SCHEMA
--   AND COL.TABLE_NAME = TAB.TABLE_NAME
--WHERE TAB.TABLE_TYPE = 'BASE TABLE'
--  AND COL.DATA_TYPE = 'Decimal'
--  AND TAB.TABLE_NAME NOT LIKE '%_Bkp_Jan%'
--  AND TAB.TABLE_NAME NOT LIKE '%Temp%'