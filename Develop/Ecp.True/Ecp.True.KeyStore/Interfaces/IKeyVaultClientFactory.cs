// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IKeyVaultClientFactory.cs" company="Microsoft">
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

    using Ecp.True.KeyStore.Entities;

    using Microsoft.Azure.KeyVault;

    /// <summary>
    /// Key Vault client factory.
    /// </summary>
    [CLSCompliant(false)]
    public interface IKeyVaultClientFactory
    {
        /// <summary>
        /// Gets the key vault client.
        /// </summary>
        /// <returns>
        /// Key vault client instance.
        /// </returns>
        IKeyVaultClient GetKeyVaultClient();

        /// <summary>
        /// Gets the key vault client configuration.
        /// </summary>
        /// <returns>
        /// Key Vault client configuration.
        /// </returns>
        KeyVaultConfiguration GetKeyVaultClientConfiguration();

        /// <summary>
        /// The initialize settings.
        /// </summary>
        /// <param name="keyVaultConfiguration">
        /// The key vault configuration.
        /// </param>
        void InitializeSettings(KeyVaultConfiguration keyVaultConfiguration);
    }
}