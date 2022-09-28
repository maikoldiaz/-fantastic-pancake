﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetKeyData.cs" company="Microsoft">
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
    /// <summary>
    /// The key operation result.
    /// </summary>
    public class GetKeyData : KeyManagementData
    {
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
        public string KeyVersion { get; set; }
    }
}