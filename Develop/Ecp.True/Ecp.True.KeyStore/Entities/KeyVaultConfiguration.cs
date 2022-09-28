// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyVaultConfiguration.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.KeyStore.Entities
{
    using System;
    using System.Security;

    /// <summary>
    /// Key vault configuration data.
    /// </summary>
    public class KeyVaultConfiguration : ICloneable
    {
        /// <summary>
        /// Gets or sets the name of the algorithm.
        /// </summary>
        /// <value>
        /// The name of the algorithm.
        /// </value>
        public string AlgorithmName { get; set; }

        /// <summary>
        /// Gets or sets the authentication mode.
        /// </summary>
        /// <value>
        /// The authentication mode.
        /// </value>
        public AuthenticationMode AuthenticationMode { get; set; }

        /// <summary>
        /// Gets or sets the certificate password.
        /// </summary>
        /// <value>
        /// The certificate password.
        /// </value>
        public SecureString CertificatePassword { get; set; }

        /// <summary>Gets or sets the certificate absolute path.</summary>
        /// <value>The certificate file path.</value>
        public string CertificatePath { get; set; }

        /// <summary>Gets or sets the certificate thumbprint.</summary>
        /// <value>The certificate thumbprint.</value>
        public string CertificateThumbprint { get; set; }

        /// <summary>
        /// Gets or sets the client authentication identifier.
        /// </summary>
        /// <value>
        /// The client authentication identifier.
        /// </value>
        public string ClientAuthId { get; set; }

        /// <summary>
        /// Gets or sets the client authentication secret.
        /// </summary>
        /// <value>
        /// The client authentication secret.
        /// </value>
        public string ClientAuthSecret { get; set; }

        /// <summary>
        /// Gets or sets the key identifier.
        /// </summary>
        /// <value>
        /// The key identifier.
        /// </value>
        public string KeyIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the name of the key.
        /// </summary>
        /// <value>
        /// The name of the key.
        /// </value>
        public string KeyName { get; set; }

        /// <summary>
        /// Gets or sets the key version.
        /// </summary>
        /// <value>
        /// The key version.
        /// </value>
        public string KeyVersion { get; set; }

        /// <summary>
        /// Gets or sets the vault address.
        /// </summary>
        /// <value>
        /// The vault address.
        /// </value>
        public string VaultAddress { get; set; }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}