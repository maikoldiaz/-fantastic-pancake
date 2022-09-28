# -*- coding: utf-8 -*-
#Import libraries
import pandas as pd                 #version 0.25.1
import numpy as np                  #version 1.16.5
import time                         #Version Python 3.6.8
import joblib                       #Version 0.13.2
#import xgboost as xgb              #Version 0.90
from AzureSql import get_operative_movements as sql_get_operative_movements, get_operative_node_relationship as sql_get_operative_node_relationship, get_historic_ownership_values as sql_get_historic_ownership_values
from azure.storage.blob import BlockBlobService
from Secrets import get_secret
from pmdarima import auto_arima     #Versión 1.3.0
from fbprophet import Prophet       #Versión 0.5

t_start = time.time()
# =============================================================================
# INPUT PARAMETERS FROM API
# =============================================================================
#algorithmId = '3'   #type of analytic model to be executed given by user configuration ("1"=ARIMA,"2"=PROPHET,"3"=XGBOOST)
#movementType = "DESPACHO A LINEA"
#sourceNode = "BATERIA 1 PALAGUA"
#sourceNodeType = "Limite"
#destinationNode = "PALAGUA-VASCONIA"
#destinationNodeType = "Oleoducto"
#sourceProduct = "CRUDO MEZCLA"
#sourceProductType = "CRUDO"
#startDate = "2019-06-12"                    #start date given by the user YYYY-MM-DD
#endDate = "2019-06-15"                      #end date given by the user YYYY-MM-DD

# =============================================================================
# INPUT TABLES REQUIRED BY THE MODEL
# Functions used to extract all required data for training and evaluation process
# =============================================================================
#Function used to get operative movements from TRUE
def get_operative_movements(startDate,endDate):
    operative_movements = sql_get_operative_movements(startDate, endDate)
    #Convert all dataframe to lower case
    operative_movements = operative_movements.apply(lambda col: col.astype(str).str.lower())
    #Ensure dates formating from database's data
    operative_movements['operationalDate']  = pd.to_datetime(operative_movements['operationalDate']).dt.date
    operative_movements["netStandardVolume"] = operative_movements["netStandardVolume"].astype(float)
    return operative_movements

#Function used to get operative node relationship dictionary from TRUE
def get_op_relationship_dict():    
    print('get_op_relationship_dict')
    op_relationship_dict = sql_get_operative_node_relationship()
    #Convert all dataframe to lower case
    op_relationship_dict = op_relationship_dict.apply(lambda col: col.astype(str).str.lower())
    print('--------------loaded--------------')
    return op_relationship_dict

#Historic ownership percentage (this table is not used by xgboost model)
def get_historic_ownership_values(startDate,historic_window):
    print('get_historic_ownership_values')
    historic_ownership_values = sql_get_historic_ownership_values()
    #Convert all dataframe to lower case
    historic_ownership_values = historic_ownership_values.apply(lambda col: col.astype(str).str.lower())
    #Ensure dates formating from database's data
    historic_ownership_values['operationalDate']  = pd.to_datetime(historic_ownership_values['operationalDate']).dt.date
    historic_ownership_values['operationalDate']  = historic_ownership_values['operationalDate'].astype(str)
    end_date = startDate
    init_date = str(np.datetime64(end_date,"D") - np.timedelta64(historic_window, 'D'))
    historic_ownership_values = historic_ownership_values[(historic_ownership_values['operationalDate'] >= init_date) & (historic_ownership_values['operationalDate'] <=end_date)]
    historic_ownership_values['operationalDate']  = pd.to_datetime(historic_ownership_values['operationalDate']).dt.date
    historic_ownership_values["ownershipPercentage"] = historic_ownership_values["ownershipPercentage"].astype(float)
    return historic_ownership_values

# =============================================================================
# TRANSFER POINT AND RELATIONSHIPS IDENTIFICATION
# Functions to identify a specific transfer point of given relationship from TRUE
# =============================================================================

#Function used to identify the transfer point
def df_from_input(input_node_rls_values):
    df = pd.DataFrame(columns = ["movementType","sourceNode","sourceNodeType","destinationNode","destinationNodeType","sourceProduct","sourceProductType"])
    df.loc["0"]= input_node_rls_values
    return df

#Function used to find all relationships for a given transfer point
def search_relationships(nodes_relationship_in,op_relationship_dict):
    transfer_point = pd.merge(nodes_relationship_in,op_relationship_dict)["transferPoint"][0]
    operative_relationships = op_relationship_dict[op_relationship_dict["transferPoint"] == transfer_point]
    operative_relationships = pd.concat([operative_relationships,op_relationship_dict[op_relationship_dict["sourceField"] == transfer_point]],axis=0)
    operative_relationships = pd.concat([operative_relationships,op_relationship_dict[op_relationship_dict["fieldWaterProduction"] == transfer_point]],axis=0)
    return [transfer_point, operative_relationships]

# =============================================================================
# INTERNAL FUNCTIONS TO CREATE ANALYTIC DATABASES
# =============================================================================

#Function used to get the oil flow at the transfer point
def get_transfer_point_flow(operative_movements):
    transfer_point_flow = operative_movements[operative_movements["transferPoint"]!= "na"]
    transfer_point_flow = transfer_point_flow.groupby(["operationalDate", "transferPoint"],group_keys =False).agg({"netStandardVolume": sum})
    transfer_point_flow.reset_index(inplace=True)
    return transfer_point_flow

#Function used to get de oil flow from source field linked to the transfer point
def get_source_field_flow(operative_movements):   
    source_field_flow = operative_movements[operative_movements["sourceField"]!= "na"]
    if len(source_field_flow) >0:    
        source_field_flow = source_field_flow.assign(fieldID = "field-"+ source_field_flow["sourceNode"].astype(str)+"-"+ source_field_flow["destinationNode"].astype(str))
        source_field_flow = source_field_flow.groupby(["fieldID","sourceField","operationalDate"]).agg({"netStandardVolume": sum})    
        source_field_flow.reset_index(inplace=True)
        source_field_flow = pd.pivot_table(source_field_flow,values="netStandardVolume",columns=["fieldID"],aggfunc=np.sum,index=["sourceField","operationalDate"])
        source_field_flow.reset_index(inplace=True)
        source_field_flow.rename(columns={"sourceField":"transferPoint"},inplace=True)
    return source_field_flow

#Function used to get the water flow from source field linked to the transfer point
def get_fieldwater_flow(operative_movements):
    fieldwater_flow = operative_movements[operative_movements["fieldWaterProduction"]!= "na"]
    if len(fieldwater_flow) >0:
        fieldwater_flow = fieldwater_flow.assign(fieldwaterID = "water-"+ fieldwater_flow["sourceNode"].astype(str)+"-"+ fieldwater_flow["destinationNode"].astype(str))
        fieldwater_flow = fieldwater_flow.groupby(["fieldwaterID","fieldWaterProduction","operationalDate"]).agg({"netStandardVolume": sum})    
        fieldwater_flow.reset_index(inplace=True)
        fieldwater_flow = pd.pivot_table(fieldwater_flow,values="netStandardVolume",columns=["fieldwaterID"],aggfunc=np.sum,index=["fieldWaterProduction","operationalDate"])
        fieldwater_flow.reset_index(inplace=True)
        fieldwater_flow.rename(columns={"fieldWaterProduction":"transferPoint"},inplace=True)
    return fieldwater_flow

#Function used to create the analytic database required by the analytic model
def create_analytic_database_xgboost(transfer_point_flow,source_field_flow,fieldwater_flow):
    tables = [transfer_point_flow,source_field_flow,fieldwater_flow]
    tables_not_empty = [table for table in tables if len(table)>0]
    if len(tables_not_empty)<1:
        analytic_database = False
    elif len(tables_not_empty)==1:
        analytic_database = tables_not_empty[0]
    elif len(tables_not_empty)==2:
         analytic_database = pd.merge(tables_not_empty[0],tables_not_empty[1],how="outer")
    else:
        analytic_database = pd.merge(tables_not_empty[0],tables_not_empty[1],how="outer")
        analytic_database = pd.merge(analytic_database,tables_not_empty[2],how="outer")
    analytic_database = analytic_database.fillna(0) #nan values are assumed as no transaction flow for the given points.
    analytic_database = analytic_database.sort_values(by=["operationalDate"])
    return analytic_database

def create_analytic_database_timeseries(algorithmId,historic_ownership_values,transfer_point):
    #Time series models only take 2 variables: date and the continuous target viable 
    if algorithmId == '1': #ARIMA
        analytic_database = historic_ownership_values[historic_ownership_values["transferPoint"]==transfer_point]
        analytic_database = analytic_database[["operationalDate","ownershipPercentage"]]
        analytic_database.columns = ["ds","y"]                  #date and ownership historic values must be named "ds" and "y"
        analytic_database = analytic_database.assign(y= analytic_database["y"].astype(float))
        analytic_database = analytic_database.sort_values(by = ["ds"])
        analytic_database = analytic_database.set_index("ds")   #Autoarima's algorithm needs date column as an index of the given dataframe
    elif algorithmId == '2': #PROPHET
        analytic_database = historic_ownership_values[historic_ownership_values["transferPoint"]==transfer_point]
        analytic_database = analytic_database[["operationalDate","ownershipPercentage"]]
        analytic_database.columns = ["ds","y"]                  #date and ownership historic values must be named "ds" and "y"
        analytic_database = analytic_database.assign(y= analytic_database["y"].astype(float))
        analytic_database = analytic_database.sort_values(by = ["ds"])
    return analytic_database

# =============================================================================
# INTERNAL FUNCTIONS TO CALL AND APPLY ANALYTIC MODELS
# =============================================================================
    
def get_model_type(algorithmId):
    model = ''
    if algorithmId == '1':
        model = 'ARIMA'
    elif algorithmId == '2':
        model = 'PROPHET'
    elif algorithmId == '3':
        model = 'XGBOOST'
    return model

#Function used to find the analytic model for the given transfer point
def get_model_and_variables(model, transfer_point):
    print('creating BLOB Access')
    # secret = storageconnectionstring
    storage = get_secret('sp', 'storageconnectionstring').value

    accountIndex = storage.find('AccountName')
    accountStart = storage.find('=', accountIndex) + 1
    accountEnd = storage.find(';', accountIndex)
    account = storage[accountStart : accountEnd]

    key_index = storage.find('AccountKey')
    key_start = storage.find('=', key_index) + 1
    key_end = storage.find(';', key_index)
    key = storage[key_start : key_end]

    block_blob_service = BlockBlobService(account_name=account, account_key=key)
    container_name = "workfiles"
    filename = "{}_{}.dat".format(model, transfer_point.upper())
    block_blob_service.get_blob_to_path(container_name, filename, filename)

    xgb_model = joblib.load(filename)[0] #XGBOOST model for the given transfer point
    variables = joblib.load(filename)[1] #List of variables to be used by the model
    
    #os.remove(filename)
    return xgb_model,variables

#Function used to run the analytic model and fix the property percentage values
def run_xgboost_model(xgb_model,analytic_database,variables):
    not_use = ["operationalDate","ownershipPercentage","transferPoint"]
    variables = [variable for variable in variables if variable not in not_use]
    analytic_db_cols = list(analytic_database.columns)
    missing_cols = [col for col in variables if col not in analytic_db_cols]
    analytic_database = analytic_database
    for col in missing_cols:
        analytic_database[col] = 0
    analytic_database = analytic_database[variables]
    ownership_percentages = xgb_model.predict(analytic_database) 
    ownership_percentages = [0.0000 if x <0 else (1.0000 if x>1 else round(float(x),4)) for x in ownership_percentages]
    return ownership_percentages

#Function used to run the ARIMA model and fix the property percentage values
def run_arima_model(analytic_database,startDate,endDate):
     arima_model = auto_arima(analytic_database,trace=True, error_action='ignore', suppress_warnings=True) #Set parameters of the model
     arima_model.fit(analytic_database)  #fit model to historic data
     last_date = list(analytic_database.index)[-1]  #Last ownership percentage value date
     n_days = (np.datetime64(endDate,"D") - np.datetime64(last_date,"D")).astype(int) #compute number of predictions days
     if n_days==0:n_days=1
     forecast_dates = [np.datetime64(last_date,"D") + np.timedelta64(i+1, 'D') for i in range(n_days)]
     #make prediction using the arima trained model
     ownership_percentages = pd.DataFrame(arima_model.predict(n_periods=int(n_days)),columns=["ownershipPercentage"])
     ownership_percentages["operationalDate"] = pd.to_datetime(forecast_dates).date   
     ownership_percentages = ownership_percentages[["operationalDate","ownershipPercentage"]]
     ownership_percentages["ownershipPercentage"] = ownership_percentages["ownershipPercentage"].apply(lambda x: 0.0000 if x <0 else (1.0000 if x>1 else round(float(x),4))) #adjust percentage values if lower than 0 or greater than 1
     target_dates = pd.DataFrame(pd.to_datetime(forecast_dates).date.astype(str))
     target_dates = list(target_dates[(target_dates.iloc[:,0]>=startDate) & (target_dates.iloc[:,0]<=endDate)].iloc[:,0])
     ownership_percentages = ownership_percentages[ownership_percentages["operationalDate"].astype(str).isin(target_dates)]
     return ownership_percentages

#Function used to run the PROPHET model and fix the property percentage values
def run_prophet_model(analytic_database,startDate,endDate):
    prophet_model = Prophet() #Set parameters of the model
    prophet_model.fit(analytic_database)  #fit model to historic data
    last_date = list(analytic_database["ds"])[-1]  #Last ownership percentage value date
    n_days = (np.datetime64(endDate,"D") - np.datetime64(last_date,"D")).astype(int) #compute number of predictions days
    if n_days==0:n_days=1    
    forecast_dates = prophet_model.make_future_dataframe(periods=n_days,freq="D")    #build dataframe with historic and future dates
    #make prediction using the arima trained model
    ownership_percentages = prophet_model.predict(forecast_dates) #Make predictions
    ownership_percentages.rename(columns={"ds":"operationalDate","yhat":"ownershipPercentage"},inplace=True)
    ownership_percentages = ownership_percentages[["operationalDate","ownershipPercentage"]]
    ownership_percentages = ownership_percentages.iloc[-n_days:,:] #Filter only forecast dates
    ownership_percentages["ownershipPercentage"] = ownership_percentages["ownershipPercentage"].apply(lambda x: 0.0000 if x <0 else (1.0000 if x>1 else round(float(x),4))) #adjust percentage values if lower than 0 or greater than 1
    ownership_percentages["operationalDate"] = ownership_percentages["operationalDate"].dt.date
    target_dates = forecast_dates["ds"].dt.date.astype(str)
    target_dates = list(target_dates[(target_dates>=startDate) & (target_dates<=endDate)])
    ownership_percentages = ownership_percentages[ownership_percentages["operationalDate"].astype(str).isin(target_dates)]
    return ownership_percentages

# =============================================================================
# MAIN FUNCTION
# =============================================================================
def get_ownership(algorithmId,movementType,sourceNode,sourceNodeType,destinationNode,destinationNodeType,sourceProduct,sourceProductType,startDate,endDate):
    #Get operatives relationship dictionary
    op_relationship_dict = get_op_relationship_dict()
    #Get operative movements
    operative_movements = get_operative_movements(startDate,endDate)
    #Get historic ownership values acording to the maximum historic window
    historic_ownership_values = get_historic_ownership_values(startDate,historic_window=150)
    #Get standard table from node relationship input given by the user
    nodes_relationship_in = df_from_input([movementType.lower(),sourceNode.lower(),sourceNodeType.lower(),destinationNode.lower(),destinationNodeType.lower(),sourceProduct.lower(),sourceProductType.lower()])
    #Get transfer point and all its posible relationships
    transfer_point, operative_relationships = search_relationships(nodes_relationship_in,op_relationship_dict)

    if algorithmId == '1': #ARIMA model
        #Create analytic data base
        analytic_database = create_analytic_database_timeseries(algorithmId,historic_ownership_values,transfer_point)
        #Get the model and predictions for the given dates
        ownership_percentages = run_arima_model(analytic_database,startDate,endDate)
        #create copy of tables
        ownership_percentages_ = ownership_percentages
        ownership_percentages_["tempkey"] = 1
        operative_relationships_ = operative_relationships
        operative_relationships_["tempkey"] = 1
        operative_relationships_ = operative_relationships_[operative_relationships_["transferPoint"]==transfer_point]
        #Produce ouput table with ownership values
        output = pd.merge(ownership_percentages_,operative_relationships_,how="outer")
        output["transferPoint"] = transfer_point

    elif algorithmId == '2': #PROPHET model
        #Create analytic data base
        analytic_database = create_analytic_database_timeseries(algorithmId,historic_ownership_values,transfer_point)
        #Get the model and predictions for the given dates
        ownership_percentages = run_prophet_model(analytic_database,startDate,endDate)
        #create copy of tables
        ownership_percentages_ = ownership_percentages
        ownership_percentages_["tempkey"] = 1
        operative_relationships_ = operative_relationships
        operative_relationships_["tempkey"] = 1
        operative_relationships_ = operative_relationships_[operative_relationships_["transferPoint"]==transfer_point]
        #Produce ouput table with ownership values
        output = pd.merge(ownership_percentages_,operative_relationships_,how="outer")
        output["transferPoint"] = transfer_point
        
    elif algorithmId == '3': #XGBOOST model
        #Update operative_movements
        operative_movements = pd.merge(operative_movements,operative_relationships)
        #Oil Flow from transfer point
        transfer_point_flow= get_transfer_point_flow(operative_movements)
        #Oil Flow from source field related to the transfer point
        source_field_flow = get_source_field_flow(operative_movements)
        #Water Flow from source field related to the transfer point
        fieldwater_flow = get_fieldwater_flow(operative_movements)
        #Create analytic data base
        analytic_database = create_analytic_database_xgboost(transfer_point_flow,source_field_flow,fieldwater_flow)
        #Get the model and variables to be used for the specific transfer point
        modelType = get_model_type(algorithmId)
        xgb_model,variables = get_model_and_variables(modelType, transfer_point)
        #Compute the ownership percentages for all given inputs   
        ownership_percentages = run_xgboost_model(xgb_model,analytic_database,variables)
        #Add ownership percentage values to analytitc database
        analytic_database["ownershipPercentage"] = ownership_percentages
        #Add ownership percentage to all relationship values attached to the transfer point
        output = pd.merge(analytic_database,operative_relationships)
        
    #Sort output's table columns
    output = output[["operationalDate","transferPoint",'movementType', 'sourceNode', 'sourceNodeType', 
           'destinationNode', 'destinationNodeType','sourceProduct', 'sourceProductType',"ownershipPercentage"]]
    return output

#output = get_ownership(algorithmId,movementType,sourceNode,sourceNodeType,destinationNode,destinationNodeType,sourceProduct,sourceProductType,startDate,endDate)

print("tiempo de ejecución (segundos): " + str(round(time.time()-t_start,2)))