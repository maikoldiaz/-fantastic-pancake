// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISecretManagementProvider.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Ecp.True.KeyStore.Entities;

    using Microsoft.Azure.KeyVault.Models;

    /// <summary>
    /// The Secret management provider interface.
    /// </summary>
    [CLSCompliant(false)]
    public interface ISecretManagementProvider
    {
        /// <summary>
        /// Deletes the secret.
        /// </summary>
        /// <param name="secretData">
        /// The secret data.
        /// </param>
        /// <returns>
        /// The secret.
        /// </returns>
        Task<SecretBundle> DeleteSecretAsync(SecretData secretData);

        /// <summary>
        /// Gets all secrets.
        /// </summary>
        /// <param name="secretData">The secret data.</param>
        /// <returns>
        /// List of all secret.
        /// </returns>
        Task<IEnumerable<SecretItem>> GetAllSecretsAsync(SecretData secretData);

        /// <summary>
        /// Gets the secret.
        /// </summary>
        /// <param name="key">
        /// The secret key.
        /// </param>
        /// <returns>
        /// The secret.
        /// </returns>
        Task<SecretBundle> GetSecretAsync(string key);

        /// <summary>
        /// The initialize settings.
        /// </summary>
        /// <param name="keyVaultConfiguration">
        /// The key vault configuration.
        /// </param>
        void InitializeSettings(KeyVaultConfiguration keyVaultConfiguration);

        /// <summary>
        /// Sets the secret.
        /// </summary>
        /// <param name="secretData">
        /// The secret data.
        /// </param>
        /// <returns>
        /// The secret.
        /// </returns>
        Task<SecretBundle> SetSecretAsync(SetSecretData secretData);
    }
}