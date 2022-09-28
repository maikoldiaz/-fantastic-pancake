// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyVaultExtensionsWrapper.cs" company="Microsoft">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Ecp.True.Core.Attributes;
    using Ecp.True.KeyStore.Interfaces;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.KeyVault.Models;
    using Microsoft.Rest.Azure;

    /// <summary>
    /// Key Vault Extensions Wrapper.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    public class KeyVaultExtensionsWrapper : IKeyVaultExtensionsWrapper
    {
        /// <inheritdoc />
        public Task<DeletedSecretBundle> DeleteSecretAsync(IKeyVaultClient keyVaultClient, string vaultAddress, string secretName)
        {
            if (keyVaultClient == null)
            {
                throw new ArgumentNullException(nameof(keyVaultClient));
            }

            return keyVaultClient.DeleteSecretAsync(vaultAddress, secretName);
        }

        /// <inheritdoc />
        public Task<SecretBundle> GetSecretAsync(IKeyVaultClient keyVaultClient, string secretIdentifier)
        {
            if (keyVaultClient == null)
            {
                throw new ArgumentNullException(nameof(keyVaultClient));
            }

            return keyVaultClient.GetSecretAsync(secretIdentifier);
        }

        /// <inheritdoc />
        public Task<IPage<SecretItem>> GetSecretsAsync(IKeyVaultClient keyVaultClient, string vaultAddress)
        {
            if (keyVaultClient == null)
            {
                throw new ArgumentNullException(nameof(keyVaultClient));
            }

            return keyVaultClient.GetSecretsAsync(vaultAddress);
        }

        /// <inheritdoc />
        public Task<SecretBundle> SetSecretAsync(IKeyVaultClient keyVaultClient, string vaultAddress, string secretName, string secretValue)
        {
            if (keyVaultClient == null)
            {
                throw new ArgumentNullException(nameof(keyVaultClient));
            }

            return keyVaultClient.SetSecretAsync(vaultAddress, secretName, secretValue);
        }
    }
}