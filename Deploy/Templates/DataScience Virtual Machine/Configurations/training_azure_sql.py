"""
    Example of using ODBC to connect to a Sql Azure database with Python
    https://docs.microsoft.com/en-us/azure/sql-database/sql-database-connect-query-python

"""
import json
import pyodbc
import pandas as pd
from Secrets import get_secret
from datetime import date

# Get the connection string from KeyVault using Service Principal or Managed Identity (MSI)
# Connection string looks like:
# DRIVER={ODBC Driver 17 for SQL Server};SERVER=<database server>.database.windows.net;PORT=1433;DATABASE=<database name>;UID=<database user/admin>;PWD=<password>
def get_connection_string():    
    conn = get_secret('msi', 'odbcsqlconnectionstring')
    return conn

def get_operative_movements(startDate, endDate):
    conn = get_connection_string()
    cnxn = pyodbc.connect(conn.value)
    print("connection successful")

    query = '''SELECT [operationalDate]
                ,[destinationNode]
                ,[destinationNodeType]
                ,[movementType]
                ,[sourceNode]
                ,[sourceNodeType]
                ,[sourceProduct]
                ,[sourceProductType]
                ,[transferPoint]
                ,[fieldWaterProduction]
                ,[sourceField]
                ,[relatedSourceField]
                ,[netStandardVolume]
            FROM [Analytics].[OperativeMovements]
            WHERE [OperationalDate] between \'{}\' and \'{}\' '''.format(startDate, endDate)
    SQL_Query = pd.read_sql_query(query, cnxn)

    print('get_operative_movements querying')
    result = pd.DataFrame(SQL_Query, columns=['operationalDate','destinationNode','destinationNodeType','movementType','sourceNode','sourceNodeType',
    'transferPoint','fieldWaterProduction','sourceField','relatedSourceField','netStandardVolume','sourceProduct','sourceProductType'])

    if not result.empty:
        print('get_operative_movements returning: ' + str(len(result)))

    return result

def get_historic_ownership_values(startDate, endDate):
    conn = get_connection_string()
    #print('get_historic_ownership_values ConString: '+ conn.value)
    cnxn = pyodbc.connect(conn.value)
    print("connection successful")
    
    query = '''SELECT [operationalDate]
      ,[transferPoint]
      ,[ownershipPercentage]
      FROM [Analytics].[OwnershipPercentageValues]
      WHERE [OperationalDate] between \'{}\' and \'{}\' '''.format(startDate, endDate)

    SQL_Query = pd.read_sql_query(query, cnxn)
    print('get_historic_ownership_values querying')
    result = pd.DataFrame(SQL_Query, columns=['operationalDate','transferPoint','ownershipPercentage'])
    
    if not result.empty:
        print('get_historic_ownership_values returning: ' + str(len(result)))
        
    return result


def get_operative_node_relationship():
    conn = get_connection_string()
    #print('get_operative_node_relationship ConString: '+ conn.value)
    cnxn = pyodbc.connect(conn.value)
    print("connection successful")

    SQL_Query = pd.read_sql_query(
        '''SELECT [transferPoint]
            ,[sourceField]
            ,[fieldWaterProduction]
            ,[relatedSourceField]
            ,[destinationNode]
            ,[destinationNodeType]
            ,[movementType]
            ,[sourceNode]
            ,[sourceNodeType]
            ,[sourceProduct]
            ,[sourceProductType]
        FROM [Analytics].[OperativeNodeRelationship]''', cnxn)
    
    print('get_operative_node_relationship querying')
    result = pd.DataFrame(SQL_Query, columns=['transferPoint','sourceField','fieldWaterProduction','relatedSourceField','destinationNode',
    'destinationNodeType','movementType','sourceNode','sourceNodeType','sourceProduct','sourceProductType'])
    if not result.empty:
        print('get_operative_node_relationship returning: ' + str(len(result)))
    return result

def get_model_evaluation_table():
    conn = get_connection_string()
    #print('get_operative_node_relationship ConString: '+ conn.value)
    cnxn = pyodbc.connect(conn.value)
    print("connection successful")

    SQL_Query = pd.read_sql_query(
        '''SELECT [operationalDate]
            ,[transferPoint]
            ,[ownershipPercentage]
            ,[algorithmId]
            ,[algorithmType]
            ,[meanAbsoluteError]
        FROM [Analytics].[ModelEvaluation]''', cnxn)
    
    print('get_model_evaluation_table querying')
    result = pd.DataFrame(SQL_Query, columns=['operationalDate','transferPoint','ownershipPercentage','algorithmId','algorithmType',
    'meanAbsoluteError'])
    if not result.empty:
        print('get_model_evaluation_table returning: ' + str(len(result)))
    
    return result


def write_model_evaluation(model_evaluation):
    conn = get_connection_string()
    #print('write_model_evaluation ConString: '+ conn.value)
    cnxn = pyodbc.connect(conn.value)
    print("connection successful")
    
    cursor = cnxn.cursor()     
    
    for index, row in model_evaluation.iterrows():
        cursor.execute('''INSERT INTO [Analytics].[ModelEvaluation]

           ([OperationalDate]
           ,[TransferPoint]
           ,[OwnershipPercentage]
           ,[AlgorithmId]
           ,[AlgorithmType]  
           ,[MeanAbsoluteError]    
           ,[LoadDate])

        VALUES
            (?,?,?,?,?,?,?)''',
            row["operationalDate"],
            row["transferPoint"],
            row["ownershipPercentage"],
            row["algorithmId"],
            row["algorithmType"],
            row["meanAbsoluteError"],
            str(date.today()))
            
    cursor.close()
    cnxn.commit()
    
    return  print('Successful Operation')
    
def delete_model_evaluation_table():
    
    conn = get_connection_string()
    #print('write_model_evaluation ConString: '+ conn.value)
    cnxn = pyodbc.connect(conn.value)
    print("connection successful")
    
    cursor = cnxn.cursor()  
        
    cursor.execute('''DELETE FROM [Analytics].[ModelEvaluation]''')

    cursor.close()
    cnxn.commit()
    
    return print('Successful Operation')