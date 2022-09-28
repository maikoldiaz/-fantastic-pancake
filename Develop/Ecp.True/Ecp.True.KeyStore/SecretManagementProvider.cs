// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecretManagementProvider.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.KeyStore
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Ioc.Interfaces;
    using Ecp.True.KeyStore.Entities;
    using Ecp.True.KeyStore.Interfaces;
    using Ecp.True.Logging;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.KeyVault.Models;

    /// <summary>
    /// The Secret management provider class.
    /// </summary>
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    [CLSCompliant(false)]
    public class SecretManagementProvider : ISecretManagementProvider
    {
        /// <summary>
        /// The secret format.
        /// </summary>
        private const string SecretFormat = "{0}/secrets/{1}";

        /// <summary>
        /// The sanitizer regex.
        /// </summary>
        private static readonly Regex SanitizerRegex = new Regex("(?:[^a-z0-9 ]|(?<=['\"]))", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <summary>
        /// The key vault client factory.
        /// </summary>
        private readonly IKeyVaultClientFactory keyVaultClientFactory;

        /// <summary>
        /// The key vault extension wrapper.
        /// </summary>
        private readonly IKeyVaultExtensionsWrapper keyVaultExtensionsWrapper;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly IResolver resolver;

        /// <summary>
        /// The key vault client.
        /// </summary>
        private IKeyVaultClient keyVaultClient;

        /// <summary>
        /// The vault URI.
        /// </summary>
        private string vaultAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretManagementProvider" /> class.
        /// </summary>
        /// <param name="resolver">
        /// The resolver.
        /// </param>
        /// <param name="keyVaultClientFactory">
        /// The key vault client factory.
        /// </param>
        /// <param name="keyVaultExtensionsWrapper">
        /// The key vault extension wrapper.
        /// </param>
        public SecretManagementProvider(IResolver resolver, IKeyVaultClientFactory keyVaultClientFactory, IKeyVaultExtensionsWrapper keyVaultExtensionsWrapper)
        {
            ArgumentValidators.ThrowIfNull(keyVaultClientFactory, nameof(keyVaultClientFactory));
            ArgumentValidators.ThrowIfNull(keyVaultExtensionsWrapper, nameof(keyVaultExtensionsWrapper));
            this.resolver = resolver;
            this.keyVaultClientFactory = keyVaultClientFactory;
            this.keyVaultExtensionsWrapper = keyVaultExtensionsWrapper;
        }

        private ITrueLogger<SecretManagementProvider> Logger => this.resolver.GetInstance<ITrueLogger<SecretManagementProvider>>();

        /// <summary>
        /// Sets the secret.
        /// </summary>
        /// <param name="secretData">The secret data.</param>
        /// <returns>
        /// The secret.
        /// </returns>
        public async Task<SecretBundle> SetSecretAsync(SetSecretData secretData)
        {
            if (secretData == null)
            {
                this.Logger.LogError(new ArgumentNullException(nameof(secretData)), $"SecretData is null");
                return null;
            }

            try
            {
                var secret = await this.keyVaultExtensionsWrapper.SetSecretAsync(
                    this.keyVaultClient,
                    secretData.VaultAddress,
                    SanitizeSecretName(secretData.SecretName),
                    secretData.SecretValue.Trim()).ConfigureAwait(false);

                this.Logger.LogInformation(Constants.SecretHasBeenCreated);
                return secret;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Exception while creating secret '{secretData.SecretName}' in KeyVault.");
                throw;
            }
        }

        /// <summary>
        /// The initialize settings.
        /// </summary>
        /// <param name="keyVaultConfiguration">
        /// The key vault configuration.
        /// </param>
        public void InitializeSettings(KeyVaultConfiguration keyVaultConfiguration)
        {
            ArgumentValidators.ThrowIfNull(keyVaultConfiguration, nameof(keyVaultConfiguration));
            this.keyVaultClientFactory.InitializeSettings(keyVaultConfiguration);
            this.keyVaultClient ??= this.keyVaultClientFactory.GetKeyVaultClient();
            this.vaultAddress = keyVaultConfiguration.VaultAddress;
        }

        /// <summary>
        /// Gets the secret.
        /// </summary>
        /// <param name="key">The secret key.</param>
        /// <returns>
        /// The secret.
        /// </returns>
        public async Task<SecretBundle> GetSecretAsync(string key)
        {
            try
            {
                ArgumentValidators.ThrowIfNullOrEmpty(this.vaultAddress, nameof(this.vaultAddress));
                var secretIdentifier = string.Format(CultureInfo.InvariantCulture, SecretFormat, this.vaultAddress, key);
                var sanitizedIdentifier = SanitizeSecretName(secretIdentifier);
                ArgumentValidators.ThrowIfNullOrEmpty(sanitizedIdentifier, nameof(sanitizedIdentifier));
                return await this.keyVaultExtensionsWrapper.GetSecretAsync(this.keyVaultClient, sanitizedIdentifier).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Exception while retrieving secret '{key}' from KeyVault.");
                throw;
            }
        }

        /// <summary>
        /// Deletes the secret.
        /// </summary>
        /// <param name="secretData">The secret data.</param>
        /// <returns>
        /// The secret.
        /// </returns>
        public async Task<SecretBundle> DeleteSecretAsync(SecretData secretData)
        {
            if (secretData == null)
            {
                this.Logger.LogError(new ArgumentNullException(nameof(secretData)), $"SecretData is null");
                return null;
            }

            try
            {
                var secret = await this.keyVaultExtensionsWrapper.DeleteSecretAsync(
                    this.keyVaultClient,
                    secretData.VaultAddress,
                    SanitizeSecretName(secretData.SecretName)).ConfigureAwait(false);

                this.Logger.LogInformation(Constants.SecretHasBeenDeleted);
                return secret;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Exception while deleting '{secretData.SecretName}' from KeyVault.");
                throw;
            }
        }

        /// <summary>
        /// Gets all secrets.
        /// </summary>
        /// <param name="secretData">The secret data.</param>
        /// <returns>
        /// List of all secret.
        /// </returns>
        public async Task<IEnumerable<SecretItem>> GetAllSecretsAsync(SecretData secretData)
        {
            if (secretData == null)
            {
                this.Logger.LogError(new ArgumentNullException(nameof(secretData)), $"SecretData is null");
                return null;
            }

            var result = new List<SecretItem>();
            try
            {
                var response = await this.keyVaultExtensionsWrapper.GetSecretsAsync(this.keyVaultClient, secretData.VaultAddress).ConfigureAwait(false);
                result.AddRange(response);
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Exception while retrieving all secrets from KeyVault.");
                throw;
            }

            return result;
        }

        /// <summary>
        /// Removes the special characters.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>String after removing all special character.</returns>
        private static string RemoveSpecialCharacters(string input)
        {
            return SanitizerRegex.Replace(input, string.Empty);
        }

        /// <summary>
        /// The sanitize secret name.
        /// </summary>
        /// <param name="secretName">
        /// The secret name.
        /// </param>
        /// <returns>
        /// The <see cref="string" />.
        /// </returns>
        private static string SanitizeSecretName(string secretName)
        {
            var keyNamePart = secretName.Contains("/", StringComparison.OrdinalIgnoreCase) ? secretName.Substring(secretName.LastIndexOf('/') + 1) : secretName;

            var keyNamePartSanitized = RemoveSpecialCharacters(keyNamePart).ToLowerInvariant();
            return secretName.Replace(keyNamePart, keyNamePartSanitized, StringComparison.OrdinalIgnoreCase);
        }
    }
}