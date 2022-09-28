// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IKeyVaultExtensionsWrapper.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.KeyStore.Interfaces
{
    using System.Threading.Tasks;

    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.KeyVault.Models;
    using Microsoft.Rest.Azure;

    /// <summary>
    /// Key Vault Extensions Wrapper Interface.
    /// </summary>
    public interface IKeyVaultExtensionsWrapper
    {
        /// <summary>
        /// Delete Secret Async.
        /// </summary>
        /// <param name="keyVaultClient">The key vault client.</param>
        /// <param name="vaultAddress">The vault address.</param>
        /// <param name="secretName">The secret name.</param>
        /// <returns>The Deleted Secret Bundle.</returns>
        Task<DeletedSecretBundle> DeleteSecretAsync(IKeyVaultClient keyVaultClient, string vaultAddress, string secretName);

        /// <summary>
        /// Get Secret Async.
        /// </summary>
        /// <param name="keyVaultClient">The key vault client.</param>
        /// <param name="secretIdentifier">The secret identifier.</param>
        /// <returns>The secret bundle.</returns>
        Task<SecretBundle> GetSecretAsync(IKeyVaultClient keyVaultClient, string secretIdentifier);

        /// <summary>
        /// Get Secrets Async.
        /// </summary>
        /// <param name="keyVaultClient">The key vault client.</param>
        /// <param name="vaultAddress">The vault address.</param>
        /// <returns>Collection of secret items.</returns>
        Task<IPage<SecretItem>> GetSecretsAsync(IKeyVaultClient keyVaultClient, string vaultAddress);

        /// <summary>
        /// Set Secret Async.
        /// </summary>
        /// <param name="keyVaultClient">The key vault client.</param>
        /// <param name="vaultAddress">The vault address.</param>
        /// <param name="secretName">The secret name.</param>
        /// <param name="secretValue">The secret value.</param>
        /// <returns>The secret bundle.</returns>
        Task<SecretBundle> SetSecretAsync(IKeyVaultClient keyVaultClient, string vaultAddress, string secretName, string secretValue);
    }
}