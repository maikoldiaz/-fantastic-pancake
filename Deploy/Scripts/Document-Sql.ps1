param
(
    [Parameter(Mandatory = $true)]
    [string]$ModulePath,

    [Parameter(Mandatory = $true)]
    [String]$SqlServerConnectionString,

    [Parameter(Mandatory = $false)]
    [bool]$WindowsAuth,

    [Parameter(Mandatory = $true)]
    [String]$WikiPath,

    [Parameter(Mandatory = $true)]
    [String]$filePath,

    [Parameter(Mandatory = $true)]
    [String]$FolderName
)

# Load needed assemblies
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMO") | Out-Null;
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMOExtended")| Out-Null;

# Query used for getting referencing and referenced entities
$referencingQuery = "WITH outputTable AS
(SELECT OBJECT_SCHEMA_NAME ( referencing_id ) AS referencing_schema_name,
    CASE
	WHEN o.type_desc = 'DEFAULT_CONSTRAINT'
	THEN OBJECT_NAME(o.parent_object_id)
	ELSE
	OBJECT_NAME(sed.referencing_id)
	end
	AS referencing_entity_name,
    o.type_desc AS referencing_desciption,
    referenced_schema_name,
	referenced_entity_name
FROM sys.sql_expression_dependencies AS sed
INNER JOIN sys.objects AS o ON sed.referencing_id = o.object_id
)
SELECT * FROM outputTable ";

Import-Module -Name "$ModulePath\GetValue"
Import-Module -Name "$ModulePath\ConvertToMarkdown"
Import-Module -Name "$ModulePath\ConvertDataRowsToObjects"

# Return all user databases on a sql server
function getDatabase
{
    param ($sql_server, $dbName);
    $db = $sql_server.Databases.Item($dbName);
    return $db;
}

# Get all schemas in a database
function getDatabaseSchemas
{
    param ($database);
    $schemas = $database.Schemas;
    return $schemas;
}

# Get all tables in a database
function getDatabaseTables
{
    param ($database);
    $tables = $database.Tables | Where-Object {$_.IsSystemObject -eq $false};
    return $tables;
}

# Get all stored procedures in a database
function getDatabaseStoredProcedures
{
    param ($database);
    $procs = $database.StoredProcedures | Where-Object {$_.IsSystemObject -eq $false};
    return $procs;
}

# Get all user defined functions in a database
function getDatabaseFunctions
{
    param ($database);
    $functions = $database.UserDefinedFunctions | Where-Object {$_.IsSystemObject -eq $false};
    return $functions;
}

# Get all views in a database
function getDatabaseViews
{
    param ($database);
    $views = $database.Views | Where-Object {$_.IsSystemObject -eq $false};
    return $views;
}

# Get all table types in a database
function getDatabaseTypes
{
    param ($database);
    $types = $database.UserDefinedTableTypes;
    return $types;
}

# This function will get the comments on objects
# MS calls these MS_Descriptionn when you add them through SSMS
function getDescriptionExtendedProperty
{
    param ($item);
    $description = "No MS_Description property on object.";
    foreach($property in $item.ExtendedProperties) {
        if($property.Name -eq "MS_Description") {
            $description = $property.Value;
        }
    }
    return $description;
}

# Returns a table of contents
function getTableOfContents
{
    param ($objects, $objectType);
    $grid = New-Object System.Collections.Generic.List[System.Object];
    $count = 1;
    foreach($item in $objects) {
        if($item.IsSystemObject -eq $false -or [string]$item.GetType() -eq "Microsoft.SqlServer.Management.Smo.UserDefinedTableType") {
            if([string]$item.GetType() -eq "Microsoft.SqlServer.Management.Smo.Schema") {
                $name = "["+$item.Name+"](#"+$item.Name+")";
            }
            else {
                $name = "["+$item.Schema+"."+$item.Name+"](#"+$item.Schema+"."+$item.Name+")";
            }
            $row = New-Object PSObject -Property ([ordered]@{
                "No." = $count
                $objectType = $name
            });
            $grid.Add($row);
            $count += 1;
        }
    }
    return ,$grid.ToArray();
}

# Returns a table of column details for a db table
function getTableColumnsDetails
{
    param ($table);
    $table_columns = $table.Columns;
    $grid = New-Object System.Collections.Generic.List[System.Object];
    foreach($column in $table_columns) {
        $identity = "";
        if($column.Identity){
            $seed = $column.IdentitySeed;
            $increment = $column.IdentityIncrement;
            $identity = "$seed-$increment";
        }
        $row = New-Object PSObject -Property ([ordered]@{
            "Key" = $(if($column.InPrimaryKey) { "PK"} else { if($column.IsForeignKey){"FK"} else {""}})
            "Name" = $column.Name
            "Data Type" = getColumnDataType $column
            "Max Length" = $column.DataType.MaximumLength
            "Nullability" = $column.Nullable
            "Identity" = $identity
            "Default" = $column.DefaultConstraint.Text
            "Description" = getDescriptionExtendedProperty $column
        });
        $grid.Add($row);
    }
    return ,$grid.ToArray();
}

# Returns a table of indexes details for a db table
function getTableIndexesDetails
{
    param ($table);
    $indexes = $table.Indexes;
    $grid = New-Object System.Collections.Generic.List[System.Object];
    foreach($index in $indexes) {
        $row = New-Object PSObject -Property ([ordered]@{
            "Key" = $(if($index.IndexKeyType -eq "DriPrimaryKey") { "PK"} elseif($index.IndexKeyType -eq "DriUniqueKey") { "UQ"} else {""})
            "Name" = $index.Name
            "Key Columns" = ($index.IndexedColumns -join ', ')
            "Type" = $index.IndexType
        });
        $grid.Add($row);
    }
    return ,$grid.ToArray();
}

# Returns a table of foreign key details for a db table
function getTableForeignKeysDetails
{
    param ($table);
    $foreignKeys = $table.ForeignKeys;
    $grid = New-Object System.Collections.Generic.List[System.Object];
    foreach($foreignKey in $foreignKeys) {
        if($foreignKey.IsEnabled) {
            $row = New-Object PSObject -Property ([ordered]@{
                "Name" = $foreignKey.Name
                "Columns" = getForeignKeyColumns $foreignKey
            });
            $grid.Add($row);
        }
    }
    return ,$grid.ToArray();
}

# Returns a table containing trigger details
function getTableTriggersDetails
{
    param ($table);
    $triggers = $table.Triggers;
    $grid = New-Object System.Collections.Generic.List[System.Object];
    foreach($trigger in $triggers) {
        if($trigger.IsEnabled -and !$trigger.IsSystemObject) {
            $on = "";
            if($trigger.InsteadOf) {
                $on = "INSTEAD OF -";
            }else{
                $on = "AFTER - ";
            }
            if($trigger.Insert) {
                $on = $on + "Insert ";
            }
            if($trigger.Update){
                $on = $on + "Update ";
            }
            if($trigger.Delete){
                $on = $on + "Delete ";
            }
            $row = New-Object PSObject -Property ([ordered]@{
                "Name" = $trigger.Name
                "Description" = getDescriptionExtendedProperty $trigger
                "On" = $on
                "Encrypted" = $trigger.IsEncrypted
                "Not For Replication" = $trigger.NotForReplication
            });
            $grid.Add($row);
        }
    }
    return ,$grid.ToArray();
}

# Returns a table of column details for a db view
function getViewColumnsDetails
{
    param ($view);
    $columns = $view.Columns;
    $grid = New-Object System.Collections.Generic.List[System.Object];
    foreach($column in $columns) {
        $row = New-Object PSObject -Property ([ordered]@{
            "Name" = $column.Name
            "Data Type" = getColumnDataType $column
            "Max Length" = $column.DataType.MaximumLength
        });
        $grid.Add($row);
    }
    return ,$grid.ToArray();
}

# Gets the parameters for a Stored Procedure or Function
function getParametersDetails
{
    param ($proc);
    $parameters = $proc.Parameters;
    $grid = New-Object System.Collections.Generic.List[System.Object];
    foreach($param in $parameters) {
        $row = New-Object PSObject -Property ([ordered]@{
            "Name" = $param.Name
            "Data Type" = getColumnDataType $param
            "Max Length" = $param.DataType.MaximumLength
        });
        $grid.Add($row);
    }
    return ,$grid.ToArray();
}

# Returns a table of column details for a db type
function getTypeColumnsDetails
{
    param ($type);
    $columns = $type.Columns;
    $grid = New-Object System.Collections.Generic.List[System.Object];
    foreach($column in $columns) {
        $row = New-Object PSObject -Property ([ordered]@{
            "Name" = $column.Name
            "Data Type" = getColumnDataType $column
            "Max Length" = $column.DataType.MaximumLength
            "Nullable" = $column.Nullable
        });
        $grid.Add($row);
    }
    return ,$grid.ToArray();
}

# This function gets column data type
function getColumnDataType
{
    param($obj);
    if($obj.DataType.Name -eq "decimal" -or $obj.DataType.Name -eq "numeric" ) {
        $dataType = $obj.DataType.Name + "("+$obj.DataType.NumericPrecision+","+$obj.DataType.NumericScale+")";
        return $dataType;
    }else{
        $dataType= $obj.DataType.Name;
        return $dataType;
    }
}

# This function gets the objects which the object uses
function getUses
{
    param($obj);
    $query = $referencingQuery + "WHERE referencing_schema_name = '"+$obj.Schema+"' AND referencing_entity_name = '"+$obj.Name+"'";
    $queryResults = Invoke-SqlCmd -Query $query -ConnectionString $SqlServerConnectionString
    $uses = $obj.Schema +"  `n";
    foreach($row in $queryResults) {
        $uses = $uses + $row.referenced_schema_name+"."+$row.referenced_entity_name+"  `n";
    }
    return $uses;
}

# This function gets the objects where the object was used
function getUsed
{
    param($obj);
    $query = $referencingQuery + "WHERE referenced_schema_name = '"+$obj.Schema+"' AND referenced_entity_name = '"+$obj.Name+"'";
    $queryResults = Invoke-SqlCmd -Query $query -ConnectionString $SqlServerConnectionString
    $used = "";
    foreach($row in $queryResults) {
        $used = $used + $row.referencing_schema_name+"."+$row.referencing_entity_name +"  `n";
    }
    return $used;
}

function getSummary
{
    param($objectTypes);
    $sections = [ordered]@{};
    $sections.Add("Content",$objectTypes)
    $query = "SELECT SCHEMA_NAME(schema_id) as 'Schema',
    (SELECT count(*) FROM sys.objects o1 where type = 'U' and o1.schema_id = o6.schema_id) AS 'Tables',
    (SELECT count(*) FROM sys.objects o2 where type = 'V' and o2.schema_id = o6.schema_id) AS 'Views',
    (SELECT count(*) FROM sys.objects o3 where (type IN ('TF', 'AF', 'FN', 'FS', 'FT', 'IF')) and o3.schema_id = o6.schema_id) AS 'Functions',
    (SELECT count(*) FROM sys.objects o4 where type = 'P' and o4.schema_id = o6.schema_id) AS 'Stored Procedures',
    (SELECT count(*) FROM sys.objects o5 where type = 'TR' and o5.schema_id = o6.schema_id) AS 'Triggers',
    (SELECT count(*) FROM sys.types t where t.schema_id = o6.schema_id and is_user_defined = 1) AS 'Types'
    FROM sys.objects o6
    where SCHEMA_NAME(schema_id) NOT IN ('sys', 'dbo')
    GROUP BY o6.schema_id
    ORDER BY 1";
    $queryResults = Invoke-SqlCmd -Query $query -ConnectionString $SqlServerConnectionString
    $results = Convert-DataRowsToObjects $queryResults;
    $sections.Add("Distribution", $results);
    $query = "EXEC sp_helpfile";
    $queryResults = Invoke-SqlCmd -Query $query -ConnectionString $SqlServerConnectionString
    $results = Convert-DataRowsToObjects $queryResults;
    $sections.Add("Database Size and Growth", $results);
    return $sections;
}

# This function gets the column names and referenced column names for a foreign key constraint
function getForeignKeyColumns
{
    param($fk);
    $columns="(";
    $referencedColumns="(";
    for($index=0;$index -ne $fk.Columns.Count;$index++) {
        $column = $fk.Columns[$index];
        $columns = $columns + $column.Name;
        $referencedColumns =  $referencedColumns + $fk.ReferencedTableSchema+"."+$fk.ReferencedTable+"."+ $column.ReferencedColumn;
        if($index -ne $fk.Columns.Count - 1) {
            $columns = $columns + ",";
            $referencedColumns = $referencedColumns + ",";
        }
    }
    $columns=$columns + ")";
    $referencedColumns= $referencedColumns + ")";
    $output = $columns + "->" + $referencedColumns;
    return $output;
}

function generateMarkdownDocumentation
{
    param($sections, $filePath, $objectType, $level);
    $page = "";
    if ($objectType -eq "Summary") {
        $page = $filePath + "$FolderName.md";
    } else {
        $pageName = $objectType.Replace(" ","-");
        $page = $filePath + "$FolderName\$pageName.md";
    }
    if ($level -eq 0){
        Add-Content -Path $page -Value "# $objectType `n";
    }
    foreach ($keyI in $sections.Keys) {
        Add-Content -Path $page -Value "## $keyI `n";
        $subSection = $sections[$keyI];
        if ($subSection -is [System.Collections.Specialized.OrderedDictionary] -and $level -eq 0) {
            generateMarkdownDocumentation $subSection $filePath $objectType 1;
        }
        else {
            if ($subSection -is [Object[]]) {
                $output = ConvertTo-Markdown($subSection);
                Add-Content -Path $page -Value $output;
            } elseif ($subSection -is [String]){
                Add-Content -Path $page -Value "$subSection `n";
            } elseif ($subSection -is [Object]) {
                $subSection.PSObject.Properties | ForEach-Object{
                    $propertyName = $_.Name;
                    $value = $_.Value;
                    Add-Content -Path $page -Value "### $propertyName `n"
                    Add-Content -Path $page -Value "$value `n"
                }
            }
        }
        if($level -eq 0) {
            Add-Content -Path $page -Value "`n ___ `n"
        }
    }
}

# This function gets the object details
function getObjectDetails
{
    param ($objectType, $objects);
    $sections = [ordered]@{};
    $content = getTableOfContents $objects $objectType;
    $sections.Add("Content",$content);
    if($objects.Count -gt 0) {
        foreach ($item in $objects) {
            if($item.IsSystemObject -eq $false -or [string]$item.GetType() -eq "Microsoft.SqlServer.Management.Smo.UserDefinedTableType") {
                $description = getDescriptionExtendedProperty($item);
                $title = $item.Schema + "." + $item.Name + "`n";
                if([string]$item.GetType() -eq "Microsoft.SqlServer.Management.Smo.Schema") {
                    $subSection = New-Object PSObject -Property ([ordered]@{
                        "Description" = $description
                    });
                    $sections.Add($item.Name, $subSection);
                }
                else {
                    $subSection= [ordered]@{};
                    $subSection.Add("Description",$description);
                    if([string]$item.GetType() -eq "Microsoft.SqlServer.Management.Smo.Table") {
                        $grid = getTableColumnsDetails $item;
                        $subSection.Add("Columns",$grid);
                        if($item.Indexes.Count -ne 0) {
                            $grid = getTableIndexesDetails($item);
                            $subSection.Add("Indexes", $grid);
                        }
                        if($item.ForeignKeys.Count -ne 0) {
                            $grid = getTableForeignKeysDetails $item;
                            $subSection.Add("Foreign Keys", $grid);
                        }
                        if($item.Triggers.Count -ne 0) {
                            $grid = getTableTriggersDetails $item;
                            $subSection.Add("Triggers", $grid);
                        }
                        if($item.LockEscalation -ne "Table") {
                            $lock = "";
                            if($item.LockEscalation -eq "Auto") {
                                $lock= "Auto (Enable lock escalation for the partitioned table)"
                            }
                            else{
                                $lock= "Disable lock escalation on the table"
                            }
                            $subSection.Add("Lock Escalation", $lock);
                        }
                    }
                    elseif([string]$item.GetType() -eq "Microsoft.SqlServer.Management.Smo.View") {
                        $grid=getViewColumnsDetails $item;
                        $subSection.Add("Columns",$grid);
                    }
                    elseif(([string]$item.GetType() -eq "Microsoft.SqlServer.Management.Smo.StoredProcedure") -or ([string]$item.GetType() -eq "Microsoft.SqlServer.Management.Smo.UserDefinedFunction")) {
                        if($item.Parameters.Count -ne 0) {
                            $grid=getParametersDetails $item;
                            $subSection.Add("Parameters",$grid);
                        }
                    }
                    elseif([string]$item.GetType() -eq "Microsoft.SqlServer.Management.Smo.UserDefinedTableType") {
                        $grid=getTypeColumnsDetails $item;
                        $subSection.Add("Columns",$grid);
                    }
                    $uses = getUses $item
                    $subSection.Add("Uses",$uses);
                    $used = getUsed $item
                    if($used -ne ""){
                        $subSection.Add("Used",$used);
                    }
                    $sections.Add($title, $subSection);
                }
            }
        }
    }
    return $sections;
}

# SQL Server Credentials
$svName = Get-Value $SqlServerConnectionString "Server";
$dbName = Get-Value $SqlServerConnectionString "Initial Catalog";
if ($WindowsAuth -eq $false) {
    $userId = Get-Value $SqlServerConnectionString "User ID";
    $password = Get-Value $SqlServerConnectionString "Password";  #pragma: allowlist secret
}

# Root directory where the html documentation will be generated
New-Item -Path $filePath -ItemType directory -Force | Out-Null;

# sql server that hosts the databases we wish to document
$sql_server = New-Object Microsoft.SqlServer.Management.Smo.Server $svname;
if ($WindowsAuth -eq $false) {
    $conContext = $sql_server.ConnectionContext
    $conContext.LoginSecure = $false
    $conContext.Login = $userId
    $conContext.Password = $password;
}

# IsSystemObject not returned by default so ask SMO for it
$sql_server.SetDefaultInitFields([Microsoft.SqlServer.Management.SMO.Table], "IsSystemObject");
$sql_server.SetDefaultInitFields([Microsoft.SqlServer.Management.SMO.View], "IsSystemObject");
$sql_server.SetDefaultInitFields([Microsoft.SqlServer.Management.SMO.StoredProcedure], "IsSystemObject");
$sql_server.SetDefaultInitFields([Microsoft.SqlServer.Management.SMO.Trigger], "IsSystemObject");
$sql_server.SetDefaultInitFields([Microsoft.SqlServer.Management.SMO.UserDefinedFunction], "IsSystemObject");

# Get databases on our server
$db = getDatabase $sql_server $dbName;
$dbObjectsOrder = "";
Write-Output "Started documenting database-" $db.Name;

# Clean directory
Remove-Item $($filePath +"\*") -Recurse -Force
# Directory for each database to keep everything tidy
New-Item -Path $($filePath + $FolderName) -ItemType directory -Force | Out-Null;

$objectTypesDocumented = New-Object System.Collections.Generic.List[System.Object];

# Schemas
$schemas = getDatabaseSchemas $db;
if($null -ne $schemas) {
    $sections = getObjectDetails "Schemas" $schemas;
    generateMarkdownDocumentation $sections $filePath "Schemas" 0;
    $dbObjectsOrder = $dbObjectsOrder + "Schemas`n";
    $objectTypesDocumented.Add((New-Object PSObject -Property @{"Objects" = "[Schemas]($WikiPath/$FolderName/Schemas)"}));
    Write-Output "Documented schemas";
}
# Tables
$tables = getDatabaseTables $db;
if($null -ne $tables) {
    $sections = getObjectDetails "Tables" $tables;
    generateMarkdownDocumentation $sections $filePath "Tables" 0;
    $dbObjectsOrder = $dbObjectsOrder + "Tables`n";
    $objectTypesDocumented.Add((New-Object PSObject -Property @{"Objects" = "[Tables]($WikiPath/$FolderName/Tables)"}));
    Write-Output "Documented tables";
}
# Views
$views = getDatabaseViews $db;
if($null -ne $views) {
    $sections = getObjectDetails "Views" $views;
    generateMarkdownDocumentation $sections $filePath "Views" 0;
    $dbObjectsOrder = $dbObjectsOrder + "Views`n";
    $objectTypesDocumented.Add((New-Object PSObject -Property @{"Objects" = "[Views]($WikiPath/$FolderName/Views)"}));
    Write-Output "Documented views";
}
# Stored procedures
$procs = getDatabaseStoredProcedures $db;
if($null -ne $procs) {
    $sections = getObjectDetails "Stored Procedures" $procs;
    generateMarkdownDocumentation $sections $filePath "Stored Procedures" 0;
    $dbObjectsOrder = $dbObjectsOrder + "Stored-Procedures`n";
    $objectTypesDocumented.Add((New-Object PSObject -Property @{"Objects" = "[Stored Procedures]($WikiPath/$FolderName/Stored-Procedures)"}));
    Write-Output "Documented stored procedures";
}
# Functions
$functions = getDatabaseFunctions $db;
if($null -ne $functions) {
    $sections = getObjectDetails "Functions" $functions;
    generateMarkdownDocumentation $sections $filePath "Functions" 0;
    $dbObjectsOrder = $dbObjectsOrder + "Functions`n";
    $objectTypesDocumented.Add((New-Object PSObject -Property @{"Objects" = "[Functions]($WikiPath/$FolderName/Functions)"}));
    Write-Output "Documented functions";
}
# Types
$types = getDatabaseTypes $db;
if($null -ne $types) {
    $sections = getObjectDetails "Types" $types;
    generateMarkdownDocumentation $sections $filePath "Types" 0;
    $dbObjectsOrder = $dbObjectsOrder + "Types`n";
    $objectTypesDocumented.Add((New-Object PSObject -Property @{"Objects" = "[Types]($WikiPath/$FolderName/Types)"}));
    Write-Output "Documented Types";
}
# Summary
$sections = getSummary ($objectTypesDocumented.ToArray());
generateMarkdownDocumentation $sections $filePath "Summary" 0;

$page = $filePath + "$FolderName\.order";
Add-Content -Path $page -Value $dbObjectsOrder;
Write-Output "Finished documenting database-" $db.Name;