// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateKeyData.cs" company="Microsoft">
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
    using System.Collections.Generic;

    using Microsoft.Azure.KeyVault.Models;

    /// <summary>
    /// The key operation result.
    /// </summary>
    [CLSCompliant(false)]
    public class CreateKeyData : KeyManagementData
    {
        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public KeyAttributes Attributes { get; set; }

        /// <summary>
        /// Gets or sets the key identifier.
        /// </summary>
        /// <value>
        /// The key identifier.
        /// </value>
        public string KeyIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the key operations.
        /// </summary>
        /// <value>
        /// The key operations.
        /// </value>
        public IEnumerable<string> KeyOperations { get; set; }

        /// <summary>
        /// Gets or sets the size of the key.
        /// </summary>
        /// <value>
        /// The size of the key.
        /// </value>
        public int? KeySize { get; set; }

        /// <summary>
        /// Gets or sets the type of the key.
        /// </summary>
        /// <value>
        /// The type of the key.
        /// </value>
        public string KeyType { get; set; }
    }
}