// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyVaultSecretClient.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Blockchain.Services
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Blockchain.Interfaces;
    using Ecp.True.Blockchain.SetUp;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.KeyVault.Models;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    /// <summary>
    /// The KeyVaultSecretClient.
    /// </summary>
    /// <seealso cref="Ecp.True.Blockchain.Interfaces.IKeyVaultSecretClient" />
    public class KeyVaultSecretClient : IKeyVaultSecretClient
    {
        /// <summary>
        /// The key vault client.
        /// </summary>
        private KeyVaultClient keyVaultClient;

        /// <summary>
        /// Initializes the specified application identifier.
        /// </summary>
        /// <param name="appID">The application identifier.</param>
        /// <param name="appSecret">The application secret.</param>
        public void Initialize(string appID, string appSecret)
        {
            this.keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(
                async (authority, resource, scope) =>
                {
                    AuthenticationContext authContext = new AuthenticationContext(authority);
                    ClientCredential credential = new ClientCredential(appID, appSecret);
                    AuthenticationResult result = await authContext.AcquireTokenAsync(resource, credential).ConfigureAwait(false);

                    if (result == null)
                    {
                        throw new InvalidOperationException("Failed to retrieve JWT token");
                    }

                    return result.AccessToken;
                }));
        }

        /// <summary>
        /// Checks the secrets asynchronous.
        /// </summary>
        /// <param name="keyVaultUrl">The key vault URL.</param>
        /// <param name="secretNames">The secret names.</param>
        /// <returns>
        /// The boolean.
        /// </returns>
        public async Task<bool> CheckSecretsAsync(string keyVaultUrl, string[] secretNames)
        {
            ArgumentValidators.ThrowIfNull(secretNames, nameof(secretNames));

            try
            {
                foreach (var secret in secretNames)
                {
                    await this.keyVaultClient.GetSecretAsync(keyVaultUrl, secret).ConfigureAwait(false);
                }

                return true;
            }
            catch (KeyVaultErrorException)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the secret asynchronous.
        /// </summary>
        /// <param name="keyVaultUrl">The key vault URL.</param>
        /// <param name="secretName">The secret name.</param>
        /// <returns>The secret.</returns>
        public async Task<string> GetSecretAsync(string keyVaultUrl, string secretName)
        {
            try
            {
                var secret = await this.keyVaultClient.GetSecretAsync(keyVaultUrl, secretName).ConfigureAwait(false);
                return secret.Value;
            }
            catch (KeyVaultErrorException)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Inserts the secret asynchronous.
        /// </summary>
        /// <param name="keyVaultURL">The key vault URL.</param>
        /// <param name="secretName">Name of the secret.</param>
        /// <param name="secretValue">The secret value.</param>
        /// <returns>The task.</returns>
        public async Task InsertSecretAsync(string keyVaultURL, string secretName, string secretValue)
        {
            await this.keyVaultClient.SetSecretAsync(keyVaultURL, secretName, secretValue).ConfigureAwait(false);
        }
    }
}