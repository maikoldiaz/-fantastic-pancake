#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
@author: DataScientist
Script creado para el entrenamiento y prueba de modelos analíticos con datos de propiedad diaria.
previo al despliegue en TRUE
"""

#Import libraries
import pandas as pd
import numpy as np
import time
from tqdm import tqdm
import xgboost as xgb
from itertools import compress
import random
import copy
import joblib
from pmdarima import auto_arima
from fbprophet import Prophet
from training_azure_sql import get_operative_movements as sql_get_operative_movements, get_operative_node_relationship as sql_get_operative_node_relationship, get_historic_ownership_values as sql_get_historic_ownership_values,get_model_evaluation_table as sql_get_model_evaluation_table, write_model_evaluation as sql_write_model_evaluation, delete_model_evaluation_table as sql_delete_model_evaluation_table
import sys

t_start = time.time()
base_directory = "\\".join(sys.argv[0].split("\\")[:-1]) + "\\Salidas entrenamiento"

#%%
# =============================================================================
# Training and evaluation period scheme
# Use this parameters to indicate if model will be trained daily ("d") or monthly ("m")
# =============================================================================

period_scheme = "d" #Use "d" for daily models, use "m" for monthly models

#%%
# =============================================================================
# Date and evaluation window parameters for each analytic model
# This section should be use for the user to change training dates and evaluation window
# =============================================================================
#Define dates for ARIMA models "YYYY-MM-DD"
startTrainDate_arima = "2016-01-01"
endTrainDate_arima = "2020-02-29"

#Define dates for Prophet models "YYYY-MM-DD"
startTrainDate_prophet = "2016-01-01"
endTrainDate_prophet = "2020-02-29"

#Define dates for XGBOOST models "YYYY-MM-DD"
startTrainDate_xgb = "2016-01-01"
endTrainDate_xgb = "2020-02-29"

#Define model evaluation historic window (Days)
evaluation_window = 100

#%%
# =============================================================================
# Genetic algorithm parameters
# This section should be use for the user to change genetic algorithm parameters used for feature selection of XGBoost models
# =============================================================================
init_solutions_prob = [0.8,0.2]    #Probability of ceros-ones to be incluided in the init solutions generation *sum of probs has to be equal to one
pop_size=300          #Population size of genetic algorithm (number of solutions to be evaluated for each generation)
pop_perc=0.6          #Proportion of best individuals to be crisscrossed
mutation_prob=0.05    #Mutation probability of individuals in population
elite_perc=0.1        #Proportion of best solutions that automatically goes to the next generation
generations=6         #Number of generations as an stop criterion for genetic algorithm

#%%
# =============================================================================
# REQUIRED INPUT TABLES
# Functions used to extract all required data for training and evaluation process
# =============================================================================

#Operative movements
def get_operative_movements(startTrainDate,endTrainDate):
    operative_movements = sql_get_operative_movements(startTrainDate,endTrainDate)
    #Convert all dataframe to lower case
    operative_movements = operative_movements.apply(lambda col: col.astype(str).str.lower())
    #Ensure dates formating from database's data
    operative_movements['operationalDate']  = pd.to_datetime(operative_movements['operationalDate']).dt.date
    operative_movements['operationalDate']  = operative_movements['operationalDate'].astype(str)
    operative_movements = operative_movements[(operative_movements['operationalDate'] >= startTrainDate) & (operative_movements['operationalDate'] <=endTrainDate)]
    operative_movements['operationalDate']  = pd.to_datetime(operative_movements['operationalDate']).dt.date
    operative_movements["netStandardVolume"] = operative_movements["netStandardVolume"].astype(float)
    return operative_movements

#Operative node relationship dictionary
def get_op_relationship_dict():   
    op_relationship_dict = sql_get_operative_node_relationship()
    #Convert all dataframe to lower case
    op_relationship_dict = op_relationship_dict.apply(lambda col: col.astype(str).str.lower())
    return op_relationship_dict

#Historic ownership percentages to train xgboost models
def get_historic_ownership_values_xgboost(startTrainDate,endTrainDate):
    historic_ownership_values = sql_get_historic_ownership_values(startTrainDate,endTrainDate)
    #Convert all dataframe to lower case
    historic_ownership_values = historic_ownership_values.apply(lambda col: col.astype(str).str.lower())
    #Ensure dates formating from database's data
    historic_ownership_values['operationalDate']  = pd.to_datetime(historic_ownership_values['operationalDate']).dt.date
    historic_ownership_values['operationalDate']  = historic_ownership_values['operationalDate'].astype(str)
    historic_ownership_values = historic_ownership_values[(historic_ownership_values['operationalDate'] >= startTrainDate) & (historic_ownership_values['operationalDate'] <=endTrainDate)]
    historic_ownership_values['operationalDate']  = pd.to_datetime(historic_ownership_values['operationalDate']).dt.date
    historic_ownership_values["ownershipPercentage"] = historic_ownership_values["ownershipPercentage"].astype(float)
    return historic_ownership_values

#Historic ownership percentages to evaluate timeseries models
def get_historic_ownership_values_timeseries(startTrainDate,endTrainDate):
    historic_ownership_values = sql_get_historic_ownership_values(startTrainDate,endTrainDate)
    #Convert all dataframe to lower case
    historic_ownership_values = historic_ownership_values.apply(lambda col: col.astype(str).str.lower())
    #Ensure dates formating from database's data
    historic_ownership_values['operationalDate']  = pd.to_datetime(historic_ownership_values['operationalDate']).dt.date
    historic_ownership_values['operationalDate']  = historic_ownership_values['operationalDate'].astype(str)
    historic_ownership_values = historic_ownership_values[(historic_ownership_values['operationalDate'] >= startTrainDate) & (historic_ownership_values['operationalDate'] <=endTrainDate)]
    historic_ownership_values['operationalDate']  = pd.to_datetime(historic_ownership_values['operationalDate']).dt.date
    historic_ownership_values["ownershipPercentage"] = historic_ownership_values["ownershipPercentage"].astype(float)
    return historic_ownership_values

#%%
# =============================================================================
# INTERNAL FUNCTIONS TO CREATE ANALYTIC DATABASES
# =============================================================================
#Return monnthly dates for a given dataframe
def get_monthly_date(dataframe):
    date_col = pd.to_datetime(dataframe["operationalDate"])
    month= date_col.dt.month
    day = date_col.dt.days_in_month
    year = date_col.dt.year
    date_col = pd.to_datetime(year.astype(str)+ "-" + month.astype(str)+"-" + day.astype(str)).dt.date
    dataframe["operationalDate"] = date_col
    return dataframe

#Function used to get daily oil flow at the transfer point
def get_transfer_point_flow(operative_movements):
    transfer_point_flow = operative_movements[operative_movements["transferPoint"]!= "na"]
    transfer_point_flow = transfer_point_flow.groupby(["operationalDate", "transferPoint"]).agg({"netStandardVolume": sum})
    transfer_point_flow.reset_index(inplace=True)
    #If period scheme is monthly, add operative movements using daily average
    if period_scheme.lower() == "m":
        transfer_point_flow = get_monthly_date(transfer_point_flow)
        transfer_point_flow = transfer_point_flow.groupby(["operationalDate", "transferPoint"]).agg({"netStandardVolume": np.mean})
        transfer_point_flow.reset_index(inplace=True)
    return transfer_point_flow

#Function used to get daily oil flow from source fields linked to the transfer point
def get_source_field_flow(operative_movements):   
    source_field_flow = operative_movements[operative_movements["sourceField"]!= "na"]
    if len(source_field_flow) >0:    
        source_field_flow = source_field_flow.assign(fieldID = "field-"+ source_field_flow["sourceNode"].astype(str)+"-"+ source_field_flow["destinationNode"].astype(str))
        source_field_flow = source_field_flow.groupby(["fieldID","sourceField","operationalDate"]).agg({"netStandardVolume": sum})    
        source_field_flow.reset_index(inplace=True)
        #If period scheme is monthly, add operative movements using daily average
        if period_scheme.lower() == "m":
            #Group by month
            source_field_flow = get_monthly_date(source_field_flow)
            source_field_flow = source_field_flow.groupby(["fieldID","sourceField","operationalDate"]).agg({"netStandardVolume": np.mean})
            source_field_flow.reset_index(inplace=True)
        #Pivot table
        source_field_flow = pd.pivot_table(source_field_flow,values="netStandardVolume",columns=["fieldID"],aggfunc=np.sum,index=["sourceField","operationalDate"])
        source_field_flow.reset_index(inplace=True)
        source_field_flow.rename(columns={"sourceField":"transferPoint"},inplace=True)
    return source_field_flow

#Function used to get daily water flow from source field linked to the transfer point
def get_fieldwater_flow(operative_movements):
    fieldwater_flow = operative_movements[operative_movements["fieldWaterProduction"]!= "na"]
    if len(fieldwater_flow) >0:
        fieldwater_flow = fieldwater_flow.assign(fieldwaterID = "water-"+ fieldwater_flow["sourceNode"].astype(str)+"-"+ fieldwater_flow["destinationNode"].astype(str))
        fieldwater_flow = fieldwater_flow.groupby(["fieldwaterID","fieldWaterProduction","operationalDate"]).agg({"netStandardVolume": sum})    
        fieldwater_flow.reset_index(inplace=True)
        #If period scheme is monthly, add operative movements using daily average
        if period_scheme.lower() == "m":
            #Group by month
            fieldwater_flow = get_monthly_date(fieldwater_flow)
            fieldwater_flow = fieldwater_flow.groupby(["fieldwaterID","fieldWaterProduction","operationalDate"]).agg({"netStandardVolume": np.mean})
            fieldwater_flow.reset_index(inplace=True)
        #Pivot table
        fieldwater_flow = pd.pivot_table(fieldwater_flow,values="netStandardVolume",columns=["fieldwaterID"],aggfunc=np.sum,index=["fieldWaterProduction","operationalDate"])
        fieldwater_flow.reset_index(inplace=True)
        fieldwater_flow.rename(columns={"fieldWaterProduction":"transferPoint"},inplace=True)
    return fieldwater_flow

#Function used to lag a dataframe a fixed numbers of periods 
#This function could be used to add autoregresive component to XGBoost models
def dataframe_lag(dataframe,cols_list,periods):
    lagged_frame = dataframe.loc[:,cols_list]
    for i in range(periods):
        frame = dataframe.loc[:,cols_list].shift(i+1)
        nombres_nuevos = []
        for columnaix, columna in enumerate(frame.columns):
            nombres_nuevos.append(columna + str("(-") + str(i+1) + str(")"))
        frame.columns = nombres_nuevos
        lagged_frame = pd.concat([lagged_frame,frame],axis=1)
    return lagged_frame

#Function used to create the analytic database required by the analytic model
def create_analytic_databases_xgboost(historic_ownership_values,operative_movements,op_relationship_dict):
    #Find valid operative movements
    operative_movements = pd.merge(operative_movements,op_relationship_dict)
    #Get required tables for analytic database
    ownership_percentages = historic_ownership_values
    transfer_point_flow = get_transfer_point_flow(operative_movements)
    source_field_flow = get_source_field_flow(operative_movements) 
    fieldwater_flow = get_fieldwater_flow(operative_movements)
    #Join all tables, for all transfer points
    big_analytic_database = pd.merge(ownership_percentages,transfer_point_flow,how="left")
    big_analytic_database = pd.merge(big_analytic_database,source_field_flow,how="left")
    big_analytic_database = pd.merge(big_analytic_database,fieldwater_flow,how="left")

    #List of transfer points (conections)
    transfer_points  = list(operative_movements["transferPoint"].unique())
    transfer_points = [i for i in transfer_points if i!= "na"]
    #divide analytic databases by transfer point
    analytic_databases = []
    for i in transfer_points:
        analytic_database = big_analytic_database[big_analytic_database["transferPoint"]==i]
        analytic_database = analytic_database.dropna(axis=1,how="all")
        analytic_database.fillna(0,inplace=True) #nan inputation
        analytic_databases.append(analytic_database)
    return transfer_points,analytic_databases

def create_analytic_database_timeseries(algorithmId,historic_ownership_values):
    #List of transfer points (conections)
    transfer_points  = list(historic_ownership_values["transferPoint"].unique())
    transfer_points = [i for i in transfer_points if i!= "na"]
    #divide analytic databases by transfer point
    analytic_databases = []
    
    #Time series models only take 2 variables: date and the continuous target viable 
    if algorithmId == 1: #ARIMA
        for i in transfer_points:
            analytic_database = historic_ownership_values[historic_ownership_values["transferPoint"]==i]
            analytic_database = analytic_database[["operationalDate","ownershipPercentage","transferPoint"]]
            analytic_database.columns = ["ds","y","transferPoint"]                  #date and ownership historic values must be named "ds" and "y"
            analytic_database = analytic_database.assign(y= analytic_database["y"].astype(float))
            analytic_database = analytic_database.sort_values(by = ["ds"])
            analytic_database = analytic_database.set_index("ds")   #Autoarima's algorithm needs date column as an index of the given dataframe
            analytic_databases.append(analytic_database)
   
    elif algorithmId == 2: #PROPHET
        for i in transfer_points:
            analytic_database = historic_ownership_values[historic_ownership_values["transferPoint"]==i]
            analytic_database = analytic_database[["operationalDate","ownershipPercentage","transferPoint"]]
            analytic_database.columns = ["ds","y","transferPoint"]                  #date and ownership historic values must be named "ds" and "y"
            analytic_database = analytic_database.assign(y= analytic_database["y"].astype(float))
            analytic_database = analytic_database.sort_values(by = ["ds"])
            analytic_databases.append(analytic_database)
   
    return transfer_points,analytic_databases
#%%
# =============================================================================
# INTERNAL FUNCTIONS TO MAKE EVALUATIONS OF A GIVEN MODEL
# =============================================================================
#Function used to evaluate a xgboost model for a given database and time-window
def xgboost_evaluation(analytic_database,periods):
    X = analytic_database.drop(["ownershipPercentage","transferPoint","operationalDate"],axis = 1)#Features
    y = analytic_database["ownershipPercentage"]   #Model target
    #train and validation data sets
    X_train = X.iloc[:-periods,:]
    X_test = X.iloc[-periods:,:]
    y_train = y[:-periods]
    y_test = y[-periods:]
    #Model training
    xgb_model = xgb.XGBRegressor(random_state=42, objective='reg:squarederror',n_jobs=2)
    xgb_model.fit(X_train, y_train)
    #Make predictions for the given periods window
    OwnershipPercentage_predictions = list(xgb_model.predict(X_test).astype(float)) 
    OwnershipPercentage_predictions = [ 0.0 if x <0 else (1.0 if x>1 else x) for x in OwnershipPercentage_predictions] #Fix values when are value < 0% or value > 100%   
    mean_absolute_error = round(np.mean(abs(np.array(OwnershipPercentage_predictions) - np.array(y_test))),4)    

    return mean_absolute_error #Return the specific error

#Function used to get report of final model evaluation for xgboost model
def xgboost_evaluation_table(analytic_database,periods):
    X = analytic_database.drop(["ownershipPercentage","transferPoint","operationalDate"],axis = 1)#Features
    y = analytic_database["ownershipPercentage"]   #Model target
    #train and validation data sets
    X_train = X.iloc[:-periods,:]
    X_test = X.iloc[-periods:,:]
    y_train = y[:-periods]
    y_test = y[-periods:]
    #Model training
    xgb_model = xgb.XGBRegressor(random_state=42, objective='reg:squarederror',n_jobs=2)
    xgb_model.fit(X_train, y_train)
    #Make predictions for the given periods window
    ownership_percentage_predictions = list(xgb_model.predict(X_test).astype(float)) 
    ownership_percentage_predictions = [ 0.0 if x <0 else (1.0 if x>1 else round(x,4)) for x in ownership_percentage_predictions] #Fix values when are value < 0% or value > 100%   
    mean_absolute_error = round(np.mean(abs(np.array(ownership_percentage_predictions) - np.array(y_test))),4)    
  
    #Model evaluation table
    xgboost_evaluation_table = analytic_database[["operationalDate","transferPoint","ownershipPercentage"]]
    xgboost_evaluation_table.loc[:,"algorithmId"] = 0
    xgboost_evaluation_table.loc[:,"algorithmType"] = "Dato histórico"
    
    predictions = analytic_database.iloc[-periods:,].reset_index()[["operationalDate","transferPoint","ownershipPercentage"]]
    predictions.loc[:,"ownershipPercentage"] = ownership_percentage_predictions
    predictions.loc[:,"algorithmId"] = 3
    predictions.loc[:,"algorithmType"] = "XGBoost"
    predictions.loc[:,"meanAbsoluteError"] = mean_absolute_error
    
    xgboost_evaluation_table = pd.merge(xgboost_evaluation_table,predictions,how="outer")
    
    return xgboost_evaluation_table #return model evaluation table
    

#Function used to evaluate an ARIMA model for a given database and time-window
def arima_evaluation_table(analytic_database,periods):
    analytic_database_ = analytic_database[["y"]]
    train_data = analytic_database_.iloc[:-periods,:]
    test_data =  analytic_database_.iloc[-periods:,:]
    #Model training
    arima_model = auto_arima(train_data,trace=True, error_action='ignore', suppress_warnings=True) #Set parameters of the model
    arima_model.fit(train_data)  #fit model to historic data
    #Make predictions for the given periods window
    ownership_percentage_predictions = list(arima_model.predict(periods).astype(float)) 
    ownership_percentage_predictions = [ 0.0 if x <0 else (1.0 if x>1 else round(x,4)) for x in ownership_percentage_predictions] #Fix values when are value < 0% or value > 100%   
    mean_absolute_error = round(np.mean(abs(np.array(ownership_percentage_predictions) - np.array(test_data.y))),4)    

    #Model evaluation table
    arima_evaluation_table = analytic_database.reset_index()
    arima_evaluation_table.columns = ["operationalDate","ownershipPercentage","transferPoint"] 
    arima_evaluation_table.loc[:,"algorithmId"] = 0
    arima_evaluation_table.loc[:,"algorithmType"] = "Dato histórico"

    predictions = arima_evaluation_table.iloc[-periods:,].reset_index()[["operationalDate","transferPoint","ownershipPercentage"]]
    predictions.loc[:,"ownershipPercentage"] = ownership_percentage_predictions
    predictions.loc[:,"algorithmId"] = 1
    predictions.loc[:,"algorithmType"] = "ARIMA"
    predictions.loc[:,"meanAbsoluteError"] = mean_absolute_error

    arima_evaluation_table = pd.merge(arima_evaluation_table,predictions,how="outer")
    
    return arima_evaluation_table  #Return model evaluation table
    
#Function used to evaluate a Prophet model for a given database and time-window
def prophet_evaluation_table(analytic_database,periods):
    analytic_database_ = analytic_database[["ds","y"]]
    train_data = analytic_database_.iloc[:-periods,:]
    test_data =  analytic_database_.iloc[-periods:,:]
    #Model training
    prophet_model = Prophet() #Set parameters of the model
    prophet_model.fit(train_data)  #fit model to historic data
    #Make predictions for the given periods window
    ownership_percentage_predictions = list(prophet_model.predict(test_data)["yhat"])
    ownership_percentage_predictions = [ 0.0 if x <0 else (1.0 if x>1 else round(x,4)) for x in ownership_percentage_predictions] #Fix values when are value < 0% or value > 100%      
    mean_absolute_error = round(np.mean(abs(np.array(ownership_percentage_predictions) - np.array(test_data.y))),4)    
    
    #Model evaluation table
    prophet_evaluation_table = analytic_database[["ds","y","transferPoint"]] #Avoid chained frames
    prophet_evaluation_table.columns = ["operationalDate","ownershipPercentage","transferPoint"] 
    prophet_evaluation_table.loc[:,"algorithmId"] = 0
    prophet_evaluation_table.loc[:,"algorithmType"] = "Dato histórico"

    predictions = prophet_evaluation_table.iloc[-periods:,].reset_index()[["operationalDate","transferPoint","ownershipPercentage"]]
    predictions.loc[:,"ownershipPercentage"] = ownership_percentage_predictions
    predictions.loc[:,"algorithmId"] = 2
    predictions.loc[:,"algorithmType"] = "Prophet"
    predictions.loc[:,"meanAbsoluteError"] = mean_absolute_error

    prophet_evaluation_table = pd.merge(prophet_evaluation_table,predictions,how="outer")
    
    return prophet_evaluation_table   #Return model evaluation table

# =============================================================================
# INTERNAL FUNCTIONS TO MAKE FEATURE SELECTION FOR XGBOOST MODELS
# =============================================================================
#Function to get the fitness values of genetic algorithm
def get_fitness_values(all_features_list,feature_binary_vector,analytic_database,periods):
    must_in_variables = ['operationalDate', 'transferPoint', 'ownershipPercentage', 'netStandardVolume']
    filtered_features = list(compress(all_features_list, feature_binary_vector)) + must_in_variables
    analytic_database = analytic_database[filtered_features]
    fitness_values = xgboost_evaluation(analytic_database,periods)
    return fitness_values

#Function to select and crossover a given population
def select_crossover(all_features_list,population,analytic_database,pop_size,elite_perc,pop_perc,periods):
    fitness_values = [[get_fitness_values(all_features_list,i,analytic_database,periods),i] for i in population]    #get fitness values for the given population
    sorted_population = [i[1] for i in sorted(fitness_values,reverse=False)]   #sort each solution from least to greatest
    elite_population =  sorted_population[:int(pop_size*elite_perc)]           #get the n best solutions to pass automatically to the next generation
    crossover_population = sorted_population[:int(pop_size*pop_perc)]          #get the n best solutions to be candidate for crossover
    #New generation
    new_generation = [] + elite_population
    #Make selection and crossover of the best solution to complete number of solutions of the new generation
    num_genes = len(population[0]) #Vector size (number of genes in solutions)
    for i in range(len(population)-len(elite_population)):
        fathers = random.sample(crossover_population, 2)                #Random selection of fathers
        cut_point = random.randint(1,num_genes-1)                       #Random selection of cut point to cross the genetic material of the given fathers
        son = fathers[0][:cut_point] + fathers[1][cut_point:]           #Resulting crossover Son
        new_generation.append(son)
    return new_generation

#Function to mutate a given population
def mutate(population,pop_size,elite_perc,mutation_prob):
    mutated_population = copy.deepcopy(population)                 #avoid modify the original population making a copy of them
    num_elite = int(pop_size*elite_perc)
    num_genes = len(population[0])                                 #size of solution vector
    for ix in range(len(mutated_population)-num_elite):
        if random.random() <= mutation_prob:
            mutate_point = random.randint(0,num_genes-1)           #Get random gen
            new_chromosome = random.randint(0,1)
            mutated_population[int(num_elite+ix)][mutate_point] = new_chromosome
    return mutated_population

#Function to get the list of all variables
def get_filtered_features_list(all_features_list,feature_binary_vector):
    selected_features = list(compress(all_features_list, feature_binary_vector))
    return selected_features
    
#Function to make feature selection
def features_selection(analytic_database,init_solutions_prob,pop_size,mutation_prob,elite_perc,pop_perc,generations,periods): 
    must_in_variables = ['operationalDate', 'transferPoint', 'ownershipPercentage', 'netStandardVolume']
    all_features_list = list(analytic_database.drop(must_in_variables,axis=1).columns)
    # =============================================================================
    # Run genetic algorithm
    # =============================================================================    
    #initial population
    population = [list(np.random.choice([0,1], len(all_features_list), p=init_solutions_prob)) for i in range(pop_size)]
    if len(all_features_list) <1:
        results = must_in_variables
    if len(all_features_list) >=5:
        #Evolucionar población
        for i in range(generations):
            new_generation = select_crossover(all_features_list,population,analytic_database,pop_size,elite_perc,pop_perc,periods) #Make Crossover
            mutated_population = mutate(new_generation,pop_size,elite_perc,mutation_prob)   #Mutate the given population
            population = mutated_population   #Update population
    
    #Regresar lista con atributos seleccionados     
    results = sorted([[get_fitness_values(all_features_list,i,analytic_database,periods),get_filtered_features_list(all_features_list,i)] for i in population])[0][1] + must_in_variables
    return results

#function used to build an XGBoost model for a given analytic database
def build_xgboost(analytic_database):  
    #explainable features
    X = analytic_database.drop(["ownershipPercentage","transferPoint","operationalDate"],axis = 1)
    #Target feature
    y = analytic_database["ownershipPercentage"]
    #XGBoost model
    xgb_model = xgb.XGBRegressor(random_state=42,objective='reg:squarederror',n_jobs=2)
    xgb_model.fit(X, y)
    return xgb_model #Return builded model

def write_xgboost_models(transfer_points,fited_analytic_databases,best_features):
    xgboost_models = list(map(lambda analytic_database: build_xgboost(analytic_database), tqdm(fited_analytic_databases)))
    #Save XGBoost model and list of variables to be used by the model
    for ix, transfer_point in enumerate(transfer_points):
        joblib.dump([xgboost_models[ix],best_features[ix]], base_directory + "//Modelos entrenados//XGBOOST_"+transfer_point.upper()+str(".dat"))
    return xgboost_models
# =============================================================================
# FUNCTIONS TO TRAIN, EVALUATE AND WRITE MODELS IF APPLIES
# =============================================================================
def train_test_arima_models(startTrainDate,endTrainDate,periods,algorithmId=1):
    historic_ownership_values = get_historic_ownership_values_timeseries(startTrainDate,endTrainDate)
    transfer_points,analytic_databases = create_analytic_database_timeseries(algorithmId,historic_ownership_values)
    arima_evaluation_tables = list(map(lambda analytic_database: arima_evaluation_table(analytic_database,periods), tqdm(analytic_databases)))
    return transfer_points,analytic_databases,arima_evaluation_tables

def train_test_prophet_models(startTrainDate,endTrainDate,periods,algorithmId=2):
    historic_ownership_values = get_historic_ownership_values_timeseries(startTrainDate,endTrainDate)
    transfer_points,analytic_databases = create_analytic_database_timeseries(algorithmId,historic_ownership_values)
    prophet_evaluation_tables = list(map(lambda analytic_database: prophet_evaluation_table(analytic_database,periods), tqdm(analytic_databases)))
    return transfer_points,analytic_databases,prophet_evaluation_tables
    
def train_test_write_xgboost_models(startTrainDate,endTrainDate,periods,init_solutions_prob,pop_size,mutation_prob,elite_perc,pop_perc,generations):
    #Get historic ownership percentages values
    historic_ownership_values = get_historic_ownership_values_xgboost(startTrainDate,endTrainDate)
    #Get operative movements
    operative_movements = get_operative_movements(startTrainDate,endTrainDate)
    #Get operatives relationship dictionary
    op_relationship_dict = get_op_relationship_dict()
    transfer_points,analytic_databases = create_analytic_databases_xgboost(historic_ownership_values,operative_movements,op_relationship_dict)
    best_features = list(map(lambda analytic_database: features_selection(analytic_database,init_solutions_prob=init_solutions_prob,pop_size=pop_size,mutation_prob=mutation_prob,elite_perc=elite_perc,pop_perc=pop_perc,generations=generations,periods=periods), tqdm(analytic_databases)))
    #Insertar reversión a CAÑO-LIMÓN
    for ix in range(len(transfer_points)):
        if transfer_points[ix] == "CAÑO_LIMON_OCLC".lower() and 'FIELD-CAÑO LIMON - COVEÑAS-ARAGUANEY -BANADIA OBC'.lower() not in best_features[ix]:
            best_features[ix].append('FIELD-CAÑO LIMON - COVEÑAS-ARAGUANEY -BANADIA OBC'.lower())
    fited_analytic_databases = [analytic_databases[i][best_features[i]] for i in range(len(best_features))]
    xgboost_models = write_xgboost_models(transfer_points,fited_analytic_databases,best_features)    
    xgboost_evaluation_tables = list(map(lambda analytic_database: xgboost_evaluation_table(analytic_database,periods), tqdm(fited_analytic_databases)))
    
    return transfer_points,analytic_databases,fited_analytic_databases,best_features,xgboost_evaluation_tables,xgboost_models
#%%
# =============================================================================
# MAIN FUNCTION TO MAKE MODELS EVALUATION REPORT (POWER BI)
# =============================================================================
arima_transfer_points, arima_analytic_databases,arima_evaluation_tables = train_test_arima_models(startTrainDate_arima,endTrainDate_arima,evaluation_window)
prophet_transfer_points, prophet_analytic_databases,prophet_evaluation_tables = train_test_prophet_models(startTrainDate_prophet,endTrainDate_prophet,evaluation_window)
xgb_transfer_points,xgb_analytic_databases,xgb_fited_analytic_databases,xgb_best_features,xgb_evaluation_tables,xgboost_models = train_test_write_xgboost_models(startTrainDate_xgb,endTrainDate_xgb,evaluation_window,init_solutions_prob,pop_size,mutation_prob,elite_perc,pop_perc,generations)

#Join all transfer point tables for each model
arima_evaluation_tables = pd.concat(arima_evaluation_tables)
prophet_evaluation_tables = pd.concat(prophet_evaluation_tables)
xgb_evaluation_tables = pd.concat(xgb_evaluation_tables)

#Join all models tables for Power BI report
powerbi_report_tables = pd.concat([arima_evaluation_tables,prophet_evaluation_tables,xgb_evaluation_tables],sort=False).drop_duplicates()
#powerbi_report_tables = pd.concat([prophet_evaluation_tables,xgb_evaluation_tables],sort=False).drop_duplicates()
powerbi_report_tables.to_csv(base_directory + "\\model_evaluation.csv",index=False,encoding = 'latin1')

#Delete and write model evaluation tables
sql_delete_model_evaluation_table()
#Change np.nan by None to be interpretable as NULL by SQL
sql_report_tables = powerbi_report_tables.where(pd.notnull(powerbi_report_tables),None)
sql_write_model_evaluation(sql_report_tables)

table_inspec = sql_get_model_evaluation_table()

print("¡Ejecución completa!")
print("Total execution time: "+str(round(time.time()-t_start,2)) + " seconds")