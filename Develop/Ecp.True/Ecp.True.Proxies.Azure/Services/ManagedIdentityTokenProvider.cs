// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManagedIdentityTokenProvider.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Azure
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Core.Attributes;
    using Microsoft.Azure.ServiceBus.Primitives;
    using Microsoft.Azure.Services.AppAuthentication;

    /// <summary>
    /// The managed identity token provider.
    /// </summary>
    /// <seealso cref="Ecp.True.Proxies.Azure.TokenProvider" />
    [IoCRegistration(IoCLifetime.Hierarchical)]
    public class ManagedIdentityTokenProvider : TokenProvider
    {
        private readonly AzureServiceTokenProvider azureServiceTokenProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedIdentityTokenProvider" /> class.
        /// </summary>
        /// <param name="tenantId">The tenant ID.</param>
        public ManagedIdentityTokenProvider(string tenantId)
            : this(new AzureServiceTokenProvider())
        {
            this.TenantId = tenantId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedIdentityTokenProvider" /> class.
        /// </summary>
        /// <param name="azureServiceTokenProvider">The token provider.</param>
        public ManagedIdentityTokenProvider(AzureServiceTokenProvider azureServiceTokenProvider)
        {
            this.azureServiceTokenProvider = azureServiceTokenProvider;
        }

        /// <summary>
        /// Gets the tenant Identifier.
        /// </summary>
        private string TenantId { get; }

        /// <summary>
        /// Gets a <see cref="SecurityToken"/> for the given audience and duration.
        /// </summary>
        /// <param name="appliesTo">The URI which the access token applies to.</param>
        /// <param name="timeout">The time span that specifies the timeout value for the message that gets the security token.</param>
        /// <returns><see cref="SecurityToken"/>The token.</returns>
        public async override Task<SecurityToken> GetTokenAsync(string appliesTo, TimeSpan timeout)
        {
            string accessToken = await this.azureServiceTokenProvider.GetAccessTokenAsync("https://servicebus.azure.net/", this.TenantId).ConfigureAwait(false);
            return new JsonSecurityToken(accessToken, appliesTo);
        }
    }
}
