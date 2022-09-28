"""
    Example of using a User Service Principal to get secrets from KeyVault
    
    Azure Key Vault client library for Python
    https://docs.microsoft.com/en-us/azure/key-vault/quick-create-python

"""
import os
from azure.keyvault.secrets import SecretClient
from azure.identity import DefaultAzureCredential, ManagedIdentityCredential, ClientSecretCredential

# VAULT_URL must be in the format 'https://<vaultname>.vault.azure.net'
def get_secret(security_scheme, secret_id):

    credentials = get_credentials(security_scheme) 
    # Create a Subscription Client 
    VAULT_URL = os.environ['KEY_VAULT']
    

    secret_client = SecretClient(VAULT_URL, credential=credentials)

    # Get the secret from KeyVault
    secret = secret_client.get_secret(secret_id)

    return secret

def get_credentials(security_scheme):
    if (security_scheme == 'sp'):
		#DefaultAzureCredential() expects the following environment variables for the User Service Principal
        
        credentials = ClientSecretCredential(client_id=os.environ['CLIENT_ID'],
                                                  client_secret=os.environ['CLIENT_SECRET'],
                                                  tenant_id=os.environ['TENANT_ID'])
        # AZURE_CLIENT_ID
        # AZURE_CLIENT_SECRET
        # AZURE_TENANT_ID
    elif (security_scheme == 'msi'):
        credentials = ManagedIdentityCredential()

    return credentials