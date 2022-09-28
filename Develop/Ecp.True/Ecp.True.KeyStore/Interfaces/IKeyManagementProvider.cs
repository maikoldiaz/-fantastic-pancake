// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IKeyManagementProvider.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    using Ecp.True.KeyStore.Entities;

    using Microsoft.Azure.KeyVault.Models;

    /// <summary>
    /// The Key management provider interface.
    /// </summary>
    [CLSCompliant(false)]
    public interface IKeyManagementProvider
    {
        /// <summary>
        /// Creates the key.
        /// </summary>
        /// <param name="keyData">The key data.</param>
        /// <returns>The key bundle.</returns>
        Task<KeyBundle> CreateKeyAsync(CreateKeyData keyData);

        /// <summary>
        /// Deletes the key.
        /// </summary>
        /// <param name="keyData">The key data.</param>
        /// <returns>The key bundle.</returns>
        Task<KeyBundle> DeleteKeyAsync(KeyManagementData keyData);

        /// <summary>
        /// The get all keys asynchronously.
        /// </summary>
        /// <param name="keyData">
        /// The key data.
        /// </param>
        /// <returns>
        /// The <see cref="Task" />.
        /// </returns>
        Task<IEnumerable<KeyItem>> GetAllKeysAsync(GetKeyData keyData);

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="keyData">The key data.</param>
        /// <returns>The key bundle.</returns>
        Task<KeyBundle> GetKeyAsync(GetKeyData keyData);

        /// <summary>
        /// Updates the key.
        /// </summary>
        /// <param name="keyData">The key data.</param>
        /// <returns>The key bundle.</returns>
        Task<KeyBundle> UpdateKeyAsync(UpdateKeyData keyData);
    }
}