"""
    Example of using ODBC to connect to a Sql Azure database with Python
    https://docs.microsoft.com/en-us/azure/sql-database/sql-database-connect-query-python

"""
import json
import pyodbc
import pandas as pd  # version 0.25.1
from Secrets import get_secret

# Get the connection string from KeyVault using Service Principal or Managed Identity (MSI)
# Connection string looks like:
# DRIVER={ODBC Driver 17 for SQL Server};SERVER=<database
# server>.database.windows.net;PORT=1433;DATABASE=<database
# name>;UID=<database user/admin>;PWD=<password>


def get_connection_string():
    conn = get_secret('sp', 'odbcsqlconnectionstring')
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
    result = pd.DataFrame(
        SQL_Query,
        columns=[
            'operationalDate',
            'destinationNode',
            'destinationNodeType',
            'movementType',
            'sourceNode',
            'sourceNodeType',
            'transferPoint',
            'fieldWaterProduction',
            'sourceField',
            'relatedSourceField',
            'netStandardVolume',
            'sourceProduct',
            'sourceProductType'])

    if not result.empty:
        print('get_operative_movements returning: ' + str(len(result)))

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
    result = pd.DataFrame(
        SQL_Query,
        columns=[
            'transferPoint',
            'sourceField',
            'fieldWaterProduction',
            'relatedSourceField',
            'destinationNode',
            'destinationNodeType',
            'movementType',
            'sourceNode',
            'sourceNodeType',
            'sourceProduct',
            'sourceProductType'])
    if not result.empty:
        print('get_operative_node_relationship returning: ' + str(len(result)))

    return result


def get_historic_ownership_values():
    conn = get_connection_string()
    #print('get_historic_ownership_values ConString: '+ conn.value)
    cnxn = pyodbc.connect(conn.value)

    print("connection successful")

    SQL_Query = pd.read_sql_query(
        '''SELECT [operationalDate]
      ,[transferPoint]
      ,[ownershipPercentage]
      FROM [Analytics].[OwnershipPercentageValues]''', cnxn)

    print('get_historic_ownership_values querying')

    result = pd.DataFrame(
        SQL_Query,
        columns=[
            'operationalDate',
            'transferPoint',
            'ownershipPercentage'])
    if not result.empty:
        print('get_historic_ownership_values returning: ' + str(len(result)))
    return result
