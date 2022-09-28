// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecretConfigurationStore.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.KeyStore.Interfaces;

    /// <summary>
    /// The Secret configuration store.
    /// </summary>
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    [CLSCompliant(false)]
    public class SecretConfigurationStore : IConfigurationStore
    {
        /// <summary>
        /// The secret management provider.
        /// </summary>
        private readonly ISecretManagementProvider secretManagementProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretConfigurationStore" /> class.
        /// </summary>
        /// <param name="secretManagementProvider">The secret management provider.</param>
        public SecretConfigurationStore(ISecretManagementProvider secretManagementProvider)
        {
            ArgumentValidators.ThrowIfNull(secretManagementProvider, nameof(secretManagementProvider));
            this.secretManagementProvider = secretManagementProvider;
        }

        /// <summary>
        /// The get from store asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        /// The value.
        /// </returns>
        public async Task<T> GetFromStoreAsync<T>(string key)
        {
            ArgumentValidators.ThrowIfNullOrEmpty(key, nameof(key));
            var secretKey = key.Replace(ConfigurationConstants.SecretConfigurationPrefix, string.Empty, StringComparison.OrdinalIgnoreCase);
            var secret = await this.secretManagementProvider.GetSecretAsync(secretKey).ConfigureAwait(false);
            if (secret?.Value == null)
            {
                throw new InvalidOperationException($"Secret {key} not found in keyvault");
            }

            return ParseType<T>(secret.Value);
        }

        /// <summary>
        /// The get from store asynchronously.
        /// Considers caching depending on shouldCache parameter.
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="shouldCache">if set to <c>true</c> [should cache].</param>
        /// <returns>
        /// The value.
        /// </returns>
        public Task<T> GetFromStoreAsync<T>(string key, bool shouldCache)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public Task<IEnumerable<T>> GetAllFromStoreAsync<T>()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// The parse type.
        /// </summary>
        /// <param name="strValue">
        /// The string value.
        /// </param>
        /// <typeparam name="T">
        /// Type of setting.
        /// </typeparam>
        /// <returns>
        /// typed value.
        /// </returns>
        protected static T ParseType<T>(string strValue)
        {
            ArgumentValidators.ThrowIfNull(strValue, nameof(strValue));
            var typedValue = default(T);
            var parseResult = strValue.ParseType(typeof(T));

            if (parseResult.Item2 != null)
            {
                return typedValue;
            }

            if (parseResult.Item1 != null)
            {
                typedValue = (T)parseResult.Item1;
            }

            return typedValue;
        }
    }
}