﻿// --------------------------------------------------------------------------------------------------------------------
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

namespace Ecp.True.Proxies.Azure
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using ChaosConstants = Ecp.True.Chaos.Constants;

    /// <summary>
    /// The Http Client Proxy.
    /// </summary>
    /// <seealso cref="Ecp.True.Proxies.Availability.Interfaces.IHttpClientProxy" />
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    [ExcludeFromCodeCoverage]
    public class HttpClientProxy : IHttpClientProxy
    {
        /// <summary>
        /// The HTTP client factory.
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// The chaos manager.
        /// </summary>
        private readonly IChaosManager chaosManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientProxy"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="chaosManager">The chaos manager.</param>
        public HttpClientProxy(IHttpClientFactory httpClientFactory, IChaosManager chaosManager)
        {
            this.httpClientFactory = httpClientFactory;
            this.chaosManager = chaosManager;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> SendAsync(HttpMethod method, Uri uri, HttpContent httpContent, string token)
        {
            ArgumentValidators.ThrowIfNull(method, nameof(method));
            ArgumentValidators.ThrowIfNull(uri, nameof(uri));

            var httpClient = this.httpClientFactory.CreateClient(Constants.DefaultHttpClient);
            var content = httpContent;
            using (var requestMessage = new HttpRequestMessage(method, uri))
            {
                requestMessage.Headers.Accept.Clear();
                requestMessage.Headers.Authorization = token.ToBearer();

                if (this.chaosManager.HasChaos)
                {
                    requestMessage.Headers.Add(ChaosConstants.ChaosHeaderName, this.chaosManager.ChaosValue);
                }

                requestMessage.Content = content;

                return await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            }
        }
    }
}