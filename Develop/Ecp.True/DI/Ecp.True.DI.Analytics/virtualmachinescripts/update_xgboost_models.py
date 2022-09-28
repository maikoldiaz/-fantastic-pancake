#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
@author: DataScientist
Script creado para la actualización de archivos de entrenamiento de modelos
xgboost (.dat) en TRUE
"""

#Import libraries
from azure.storage.blob import BlockBlobService
from Secrets import get_secret
import joblib   
import glob2
import sys
def blob_storage_connect():
    print('Creating BLOB Access')
    # secret = storageconnectionstring
    storage = get_secret('msi', 'storageconnectionstring').value
    accountIndex = storage.find('AccountName')
    accountStart = storage.find('=', accountIndex) + 1
    accountEnd = storage.find(';', accountIndex)
    account = storage[accountStart : accountEnd]
    key_index = storage.find('AccountKey')
    key_start = storage.find('=', key_index) + 1
    key_end = storage.find(';', key_index)
    key = storage[key_start : key_end]
    block_blob_service = BlockBlobService(account_name=account, account_key=key)
    print('Blob Storage Connection Successful') 
    return  block_blob_service

#Function used to get a list of .dat files in a base directory
def get_files_list(base_directory):
    file_list = glob2.glob(base_directory + "\\**\\*.dat")
    return file_list

#Function used to get the list of blobs in a container
def get_container_blob_list(block_blob_service):
    container_name = "workfiles"
    blob_files = block_blob_service.list_blobs(container_name) 
    file_names = [blob.name for blob in blob_files]
    print('Blob Storage files listed') 
    return file_names
    
#Function used to update a container with XGBoost models
def update_blob_files(base_directory,block_blob_service):
    container_name = "workfiles"
    file_paths = get_files_list(base_directory)
    file_names = [path.split("\\")[-1] for path in file_paths]
    for ix, path in enumerate(file_paths):
        block_blob_service.create_blob_from_path(container_name,file_names[ix],path)
    return print('Blob Storage files updated')

#Funcion used to delete a blob file in container
def delete_blob_file(blob_name,block_blob_service):
    container_name = "workfiles"
    block_blob_service.delete_blob(container_name, blob_name)
    return print('Blob Storage file deleted')

block_blob_service = blob_storage_connect()
archivos_blob_iniciales = get_container_blob_list(block_blob_service)
base_directory = "\\".join(sys.argv[0].split("\\")[:-1]) + "\\Salidas entrenamiento"

path_modelos = get_files_list(base_directory)
update_blob_files(base_directory,block_blob_service)
archivos_blob_finales = get_container_blob_list(block_blob_service)

print("Se actualizaron los "+ str(len(archivos_blob_finales)) + " archivos .dat")