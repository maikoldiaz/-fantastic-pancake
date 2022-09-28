// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthProvider.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Auth
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Host.UI.Auth.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Identity.Web;

    /// <summary>
    /// The Azure AD auth provider.
    /// </summary>
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    [ExcludeFromCodeCoverage]
    public class AuthProvider : IAuthProvider
    {
        /// <summary>
        /// The scopes.
        /// </summary>
        private readonly string[] scopes;

        /// <summary>
        /// The application.
        /// </summary>
        private readonly ITokenAcquisition tokenAcquisition;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="tokenAcquisition">Thetoken acquisition.</param>
        public AuthProvider(IConfiguration configuration, ITokenAcquisition tokenAcquisition)
        {
            ArgumentValidators.ThrowIfNull(configuration, nameof(configuration));

            this.scopes = new[] { configuration["EcoPetrolApi:EcoPetrolApiScope"] };
            this.tokenAcquisition = tokenAcquisition;
        }

        /// <summary>
        /// Get access token.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<string> GetCurrentUserAccessTokenAsync()
        {
            var accessToken = await this.tokenAcquisition.GetAccessTokenForUserAsync(this.scopes).ConfigureAwait(false);
            return accessToken;
        }

        /// <summary>
        /// Get access token.
        /// </summary>
        /// <param name="scopes">The scopes.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<string> GetUserAccessTokenAsync(string[] scopes)
        {
            var accessToken = await this.tokenAcquisition.GetAccessTokenForUserAsync(scopes).ConfigureAwait(false);
            return accessToken;
        }
    }
}
