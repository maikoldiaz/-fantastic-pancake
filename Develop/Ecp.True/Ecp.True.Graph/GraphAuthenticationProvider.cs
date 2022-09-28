// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphAuthenticationProvider.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Net.Http.Headers;

namespace Ecp.True.Graph
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Graph;

    /// <summary>
    /// The Azure AD auth provider.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GraphAuthenticationProvider : IAuthenticationProvider
    {
        private readonly Func<Task<string>> acquireAccessToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphAuthenticationProvider"/> class.
        /// </summary>
        /// <param name="acquireTokenCallback">he acquire token callback.</param>
        public GraphAuthenticationProvider(Func<Task<string>> acquireTokenCallback)
        {
            this.acquireAccessToken = acquireTokenCallback;
        }

        /// <summary>
        /// Authenticates the request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            string accessToken = await this.acquireAccessToken.Invoke().ConfigureAwait(false);

            // Append the access token to the request.
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
