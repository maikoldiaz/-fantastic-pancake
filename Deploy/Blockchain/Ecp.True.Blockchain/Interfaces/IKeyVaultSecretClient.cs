// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IKeyVaultSecretClient.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Blockchain.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// The IKeyVaultSecretClient.
    /// </summary>
    public interface IKeyVaultSecretClient
    {
        /// <summary>
        /// Initializes the specified application identifier.
        /// </summary>
        /// <param name="appID">The application identifier.</param>
        /// <param name="appSecret">The application secret.</param>
        void Initialize(string appID, string appSecret);

        /// <summary>
        /// Checks the secrets asynchronous.
        /// </summary>
        /// <param name="keyVaultUrl">The key vault URL.</param>
        /// <param name="secretNames">The secret names.</param>
        /// <returns>The boolean.</returns>
        Task<bool> CheckSecretsAsync(string keyVaultUrl, string[] secretNames);

        /// <summary>
        /// Inserts the secret asynchronous.
        /// </summary>
        /// <param name="keyVaultURL">The key vault URL.</param>
        /// <param name="secretName">Name of the secret.</param>
        /// <param name="secretValue">The secret value.</param>
        /// <returns>The task.</returns>
        Task InsertSecretAsync(string keyVaultURL, string secretName, string secretValue);

        /// <summary>
        /// Gets the secret asynchronous.
        /// </summary>
        /// <param name="keyVaultUrl">The key vault URL.</param>
        /// <param name="secretName">Name of the secret.</param>
        /// <returns>The secret.</returns>
        Task<string> GetSecretAsync(string keyVaultUrl, string secretName);
    }
}
