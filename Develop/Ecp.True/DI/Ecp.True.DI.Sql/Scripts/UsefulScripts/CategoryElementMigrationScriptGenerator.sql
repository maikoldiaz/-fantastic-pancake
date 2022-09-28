/* Note: The below script is used to generate all the tables which references the 
         ElementId of CategoryElement table as foreign key.
         And it also generates the UPDATE statement for all such tables.

         This should always be used whenever a new table is created which uses the CategoryElemnt as Fk.
         Or a new column is added in the existing table which uses CategoryElemnt as Fk.

         The output of this query should be updated into CategoryElementMigration.sql
 */
--------------------------------------------------------------------------------------


;With Cte
As
(
select schema_name(fk_tab.schema_id) + '.' + fk_tab.name as foreign_table,
    '>-' as rel,
    schema_name(pk_tab.schema_id) + '.' + pk_tab.name as primary_table,
    fk_cols.constraint_column_id as [no], 
    fk_col.name as fk_column_name,
    ' = ' as [join],
    pk_col.name as pk_column_name,
    fk.name as fk_constraint_name
from sys.foreign_keys fk
    inner join sys.tables fk_tab
        on fk_tab.object_id = fk.parent_object_id
    inner join sys.tables pk_tab
        on pk_tab.object_id = fk.referenced_object_id
    inner join sys.foreign_key_columns fk_cols
        on fk_cols.constraint_object_id = fk.object_id
    inner join sys.columns fk_col
        on fk_col.column_id = fk_cols.parent_column_id
        and fk_col.object_id = fk_tab.object_id
    inner join sys.columns pk_col
        on pk_col.column_id = fk_cols.referenced_column_id
        and pk_col.object_id = pk_tab.object_id
--order by schema_name(fk_tab.schema_id) + '.' + fk_tab.name,
--    schema_name(pk_tab.schema_id) + '.' + pk_tab.name, 
--    fk_cols.constraint_column_id
)
Select *
,('Select * from ' + foreign_table + ' where LastModifiedBy = ''MigrationSript''') As SelectQuery
,('UPDATE A SET A.' + fk_column_name + ' = B.NewElementId, LastModifiedBy = ''MigrationSript'', LastModifiedDate = Admin.udf_GETTRUEDATE() FROM ' + foreign_table + ' A Inner JOIN Admin.tempCategoryElementMapping B On A.' + fk_column_name + ' = B.OldElementId')
	As UpdateQuery
from Cte
Where pk_column_name = 'ElementId'
order by foreign_table, primary_table, [no]

