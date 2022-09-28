// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IKeyEncryptor.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    using Ecp.True.KeyStore.Entities;

    using Microsoft.Azure.KeyVault.Models;

    /// <summary>
    /// The Key encryption interface.
    /// </summary>
    [CLSCompliant(false)]
    public interface IKeyEncryptor
    {
        /// <summary>
        /// Decrypts the key asynchronous.
        /// </summary>
        /// <param name="keyOperationData">The key operation data.</param>
        /// <returns>The key operation result.</returns>
        Task<KeyOperationResult> DecryptKeyAsync(KeyOperationData keyOperationData);

        /// <summary>
        /// Encrypts the key asynchronous.
        /// </summary>
        /// <param name="keyOperationData">The key operation data.</param>
        /// <returns>The key operation result.</returns>
        Task<KeyOperationResult> EncryptKeyAsync(KeyOperationData keyOperationData);

        /// <summary>
        /// Unwraps the key asynchronous.
        /// </summary>
        /// <param name="keyOperationData">The key operation data.</param>
        /// <returns>The key operation result.</returns>
        Task<KeyOperationResult> UnWrapKeyAsync(KeyOperationData keyOperationData);

        /// <summary>
        /// Wraps the key asynchronous.
        /// </summary>
        /// <param name="keyOperationData">The key operation data.</param>
        /// <returns>The key operation result.</returns>
        Task<KeyOperationResult> WrapKeyAsync(KeyOperationData keyOperationData);
    }
}