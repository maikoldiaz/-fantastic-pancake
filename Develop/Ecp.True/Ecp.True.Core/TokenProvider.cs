// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TokenProvider.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Core.Interfaces;
    using Microsoft.Identity.Client;

    /// <summary>
    /// The token provider.
    /// </summary>
    /// <seealso cref="Ecp.True.Core.Interfaces.ITokenProvider" />
    [ExcludeFromCodeCoverage]
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    public class TokenProvider : ITokenProvider
    {
        /// <inheritdoc/>
        public async Task<string> GetAppTokenAsync(string tenantId, string resource, string clientId, string clientSecret)
        {
            var app = ConfidentialClientApplicationBuilder.Create(clientId)
                            .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
                            .WithClientSecret(clientSecret)
                            .Build();
            var scopes = new[] { resource };
            var result = await app.AcquireTokenForClient(scopes).ExecuteAsync().ConfigureAwait(false);

            return result.AccessToken;
        }
    }
}
