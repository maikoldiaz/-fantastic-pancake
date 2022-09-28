// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpClientProxy.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Services.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Host.UI.Auth.Interfaces;
    using Ecp.True.Host.UI.Services.Core.Interfaces;
    using ChaosConstants = Ecp.True.Chaos.Constants;

    /// <summary>
    /// The http client proxy.
    /// </summary>
    [IoCRegistration(IoCLifetime.Hierarchical)]
    [ExcludeFromCodeCoverage]
    public class HttpClientProxy : IHttpClientProxy
    {
        /// <summary>
        /// The HTTP client factory.
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// The Auth provider.
        /// </summary>
        private readonly IAuthProvider authProvider;

        /// <summary>
        /// The chaos manager.
        /// </summary>
        private readonly IChaosManager chaosManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientProxy" /> class.
        /// </summary>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="authProvider">The Auth provider.</param>
        /// <param name="chaosManager">The chaos manager.</param>
        public HttpClientProxy(IHttpClientFactory httpClientFactory, IAuthProvider authProvider, IChaosManager chaosManager)
        {
            this.httpClientFactory = httpClientFactory;
            this.authProvider = authProvider;
            this.chaosManager = chaosManager;
        }

        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="httpContent">The HTTP context.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task<HttpResponseMessage> SendAsync(HttpMethod method, Uri uri, HttpContent httpContent)
        {
            var httpClient = this.httpClientFactory.CreateClient(Constants.DefaultHttpClient);
            var token = await this.authProvider.GetCurrentUserAccessTokenAsync().ConfigureAwait(false);
            using (var requestMessage = new HttpRequestMessage(method, uri))
            {
                requestMessage.Headers.Accept.Clear();

                if (!string.IsNullOrEmpty(token))
                {
                    requestMessage.Headers.Authorization = token.ToBearer();
                }

                if (this.chaosManager.HasChaos)
                {
                    requestMessage.Headers.Add(ChaosConstants.ChaosHeaderName, this.chaosManager.ChaosValue);
                }

                requestMessage.Content = httpContent;

                var respose = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                return respose;
            }
        }
    }
}