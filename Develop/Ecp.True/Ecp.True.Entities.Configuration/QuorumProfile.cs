// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuorumProfile.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Configuration
{
    using System;

    /// <summary>
    /// The Quorum profile.
    /// </summary>
    public class QuorumProfile
    {
        /// <summary>
        /// Gets or sets the RPC endpoint.
        /// </summary>
        /// <value>
        /// The RPC endpoint.
        /// </value>
        public string RpcEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>
        /// The public key.
        /// </value>
        public string PublicKey { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the private key.
        /// </summary>
        /// <value>
        /// The private key.
        /// </value>
        public string PrivateKey { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        /// <value>
        /// The tenant identifier.
        /// </value>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the resource identifier.
        /// </summary>
        /// <value>
        /// The resource identifier.
        /// </value>
        public string ResourceId { get; set; }

        /// <summary>
        /// Gets the authority.
        /// </summary>
        /// <value>
        /// The authority.
        /// </value>
        public Uri Authority => new Uri($"https://login.windows.net/{this.TenantId}");

        /// <summary>
        /// Gets the base address.
        /// </summary>
        /// <value>
        /// The base address.
        /// </value>
        public string BaseAddress
        {
            get
            {
                var uri = new Uri(this.RpcEndpoint);
                return $"{uri.Scheme}://{uri.Authority}";
            }
        }

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <value>
        /// The resource.
        /// </value>
        public string Resource
        {
            get
            {
                return $"{this.ResourceId}/.default";
            }
        }
    }
}
