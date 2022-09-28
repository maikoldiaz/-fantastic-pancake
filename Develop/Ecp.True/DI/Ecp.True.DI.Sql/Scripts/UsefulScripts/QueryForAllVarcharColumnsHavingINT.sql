/* Note: The below script is used to get all such tables and their columns which are having varchar tpe but using interger values.
         
         The output of this query should be 1st analysed and updated into the upper section of CategoryElementMigration.sql

         Step to Use:
       ---------------------
         1. Run the query.
         2. Take all the select statements from the 1st column.
         3. Remove the last 'UNION ALL' in the select statements.
         4. Wrap up the select Statements into a CTE and add additional filters and generate new Select statements as below:

            ;with cte (
                <all the generated queries in step 3>
            ) As
            Select *
                , ('SELECT DISTINCT ' + ColumnName + ' FROM ' + TableName) AS Query
                from cte
                Where ColumnName <> '[ProductId]'
                AND ColumnName <> '[SourceProductId]'
                And ColumnName <> '[DestinationProductId]'
                And ColumnName <> '[Comment]'
                And ColumnName <> '[LogisticCenterId]'
                And ColumnName <> '[MMYYYY]'
                And ColumnName <> '[Name]'
                AND TableName <> '[Audit].[AuditLog]'

         5. Run all the above select statements and get the result.
         6. Analyse each table output and determine if it could have CategoryElement as value, if it has then shortlist it.
         7. Add all such queries in the same manner as used in the upper restricted section of CategoryElementMigration.sql

 */
--------------------------------------------------------------------------------------


SELECT DISTINCT 'SELECT DISTINCT ISNUMERIC(['+Col.name +']) As IsNum, ''['+Icol.TABLE_SCHEMA+'].['+OBJECT_NAME(Col.Object_Id)+']'' AS TableName '+
                +', ''['+Col.name +']'''+' AS ColumnName'+
                ' FROM ['+Icol.TABLE_SCHEMA+'].['+OBJECT_NAME(Col.Object_Id)+'] WHERE ISNUMERIC(['+Col.name+'])'+' = 1 UNION ALL' AS TableName
                ,Col.name AS ColumnName
                ,ICol.DATA_TYPE
FROM Sys.all_columns Col
INNER JOIN Sys.objects Obj
ON Col.object_id = Obj.object_id
INNER JOIN INFORMATION_SCHEMA.COLUMNS  ICol
ON ICol.TABLE_NAME = OBJECT_NAME(Col.Object_Id)
AND Col.name = IcOl.COLUMN_NAME
WHERE obj.type_desc = 'USER_TABLE'
AND ICol.DATA_TYPE IN ('VARCHAR','NVARCHAR','CHAR','NCHAR')
AND Col.name NOT LIKE '%Date%'



