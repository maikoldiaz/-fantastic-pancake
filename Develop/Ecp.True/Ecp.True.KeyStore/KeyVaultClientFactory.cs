// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyVaultClientFactory.cs" company="Microsoft">
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
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Ioc.Interfaces;
    using Ecp.True.KeyStore.Entities;
    using Ecp.True.KeyStore.Interfaces;
    using Ecp.True.Logging;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.Services.AppAuthentication;
    using Microsoft.Rest.TransientFaultHandling;

    /// <summary>
    /// Creates and returns a key vault client instance based on the key vault configuration.
    /// </summary>
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    [CLSCompliant(false)]
    [ExcludeFromCodeCoverage]
    public class KeyVaultClientFactory : IKeyVaultClientFactory
    {
        /// <summary>
        /// The key vault clients.
        /// </summary>
        private static readonly ConcurrentDictionary<string, KeyVaultClient> KeyVaultClients = new ConcurrentDictionary<string, KeyVaultClient>();

        /// <summary>
        /// The key vault context.
        /// </summary>
        private readonly IKeyVaultContext keyVaultContext;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly IResolver resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyVaultClientFactory" /> class.
        /// </summary>
        /// <param name="keyVaultContext">
        /// The key Vault Context.
        /// </param>
        /// <param name="resolver">The logger.</param>
        public KeyVaultClientFactory(IKeyVaultContext keyVaultContext, IResolver resolver)
        {
            this.keyVaultContext = keyVaultContext;
            this.resolver = resolver;
        }

        private ITrueLogger<KeyVaultClientFactory> Logger => this.resolver.GetInstance<ITrueLogger<KeyVaultClientFactory>>();

        /// <summary>
        /// Gets the key vault client.
        /// </summary>
        /// <returns>
        /// Key vault client instance.
        /// </returns>
        public IKeyVaultClient GetKeyVaultClient()
        {
            ArgumentValidators.ThrowIfNull(this.keyVaultContext, nameof(this.keyVaultContext));
            if (!this.keyVaultContext.Initialized || this.keyVaultContext.Settings == default(KeyVaultConfiguration))
            {
                throw new InvalidOperationException("Key vault context not initialized!");
            }

            ArgumentValidators.ThrowIfNullOrEmpty(this.keyVaultContext.Settings.VaultAddress, nameof(this.keyVaultContext.Settings.VaultAddress));
            this.Logger.LogInformation($"Calling Keyvault to Get Client");

            // Reusing key vault client across concurrent requests to prevent TCP socket exhaustion
            return KeyVaultClients.GetOrAdd(this.keyVaultContext.Settings.VaultAddress, CreateKeyVaultClient);
        }

        /// <summary>
        /// The initialize settings.
        /// </summary>
        /// <param name="keyVaultConfiguration">
        /// The key vault configuration.
        /// </param>
        public void InitializeSettings(KeyVaultConfiguration keyVaultConfiguration)
        {
            this.keyVaultContext.InitializeSettings(keyVaultConfiguration);
        }

        /// <summary>Gets the key vault client configuration.</summary>
        /// <returns>Key Vault client configuration.</returns>
        public KeyVaultConfiguration GetKeyVaultClientConfiguration()
        {
            if (!this.keyVaultContext.Initialized || this.keyVaultContext.Settings == default(KeyVaultConfiguration))
            {
                throw new InvalidOperationException("Key vault context not initialized!");
            }

            return this.keyVaultContext.Settings;
        }

        /// <summary>
        /// Creates the key vault client.
        /// </summary>
        /// <param name="vaultAddress">The vault address.</param>
        /// <returns>The keyvault client.</returns>
        private static KeyVaultClient CreateKeyVaultClient(string vaultAddress)
        {
            var tokenProvider = new AzureServiceTokenProvider();
            var callback = new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback);
            var client = new KeyVaultClient(callback, LegacyHttpClientProvider.Current);

            var exponentialBackOff = new ExponentialBackoffRetryStrategy(5, TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(16.0), TimeSpan.FromSeconds(2.0));
            client.SetRetryPolicy(new RetryPolicy<HttpStatusCodeErrorDetectionStrategy>(exponentialBackOff));

            return client;
        }
    }
}
