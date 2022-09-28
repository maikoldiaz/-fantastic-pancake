// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManagedIdentityTokenCredential.cs" company="Microsoft">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core.Attributes;

    using global::Azure.Core;
    using Microsoft.Azure.Services.AppAuthentication;

    /// <summary>
    /// The managed identity token provider.
    /// </summary>
    /// <seealso cref="Ecp.True.Proxies.Azure.TokenProvider" />
    [IoCRegistration(IoCLifetime.Hierarchical)]
    [ExcludeFromCodeCoverage]
    public class ManagedIdentityTokenCredential : TokenCredential
    {
        private readonly AzureServiceTokenProvider azureServiceTokenProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedIdentityTokenCredential" /> class.
        /// </summary>
        /// <param name="tenantId">The tenant ID.</param>
        public ManagedIdentityTokenCredential(string tenantId)
            : this(new AzureServiceTokenProvider())
        {
            this.TenantId = tenantId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedIdentityTokenCredential" /> class.
        /// </summary>
        /// <param name="azureServiceTokenProvider">The token provider.</param>
        public ManagedIdentityTokenCredential(AzureServiceTokenProvider azureServiceTokenProvider)
        {
            this.azureServiceTokenProvider = azureServiceTokenProvider;
        }

        /// <summary>
        /// Gets the tenant Identifier.
        /// </summary>
        private string TenantId { get; }

        /// <inheritdoc/>
        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            return new AccessToken(string.Empty, DateTimeOffset.UtcNow.AddMinutes(60));
        }

        /// <inheritdoc/>
        public async override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            string accessToken = await this.azureServiceTokenProvider.GetAccessTokenAsync("https://storage.azure.com/", this.TenantId).ConfigureAwait(false);
            return new AccessToken(accessToken, DateTimeOffset.UtcNow.AddMinutes(60));
        }
    }
}
