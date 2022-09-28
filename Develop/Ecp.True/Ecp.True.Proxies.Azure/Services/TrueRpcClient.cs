// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrueRpcClient.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Azure.Services
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Nethereum.JsonRpc.Client;
    using Nethereum.JsonRpc.Client.RpcMessages;
    using Newtonsoft.Json;

    /// <summary>
    /// The Custom RPC client for auth token.
    /// </summary>
    /// <seealso cref="Nethereum.JsonRpc.Client.RpcClient" />
    public class TrueRpcClient : RpcClient
    {
        /// <summary>
        /// The profile.
        /// </summary>
        private readonly QuorumProfile profile;

        /// <summary>
        /// The HTTP client factory.
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// The token provider.
        /// </summary>
        private readonly ITokenProvider tokenProvider;

        /// <summary>
        /// The settings.
        /// </summary>
        private readonly JsonSerializerSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrueRpcClient" /> class.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="tokenProvider">The token provider.</param>
        public TrueRpcClient(QuorumProfile profile, IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
            : base(new Uri(profile?.BaseAddress))
        {
            this.profile = profile;
            this.httpClientFactory = httpClientFactory;
            this.tokenProvider = tokenProvider;
            this.settings = DefaultJsonSerializerSettingsFactory.BuildDefaultJsonSerializerSettings();
        }

        /// <inheritdoc/>
        protected override async Task<RpcResponseMessage> SendAsync(RpcRequestMessage request, string route = null)
        {
            try
            {
                var httpClient = await this.GetHttpClientAsync().ConfigureAwait(false);

                using var httpContent = new StringContent(JsonConvert.SerializeObject(request, this.settings), Encoding.UTF8, "application/json");
                using var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(ConnectionTimeout);

                var httpResponseMessage = await httpClient.PostAsync(route, httpContent, cancellationTokenSource.Token).ConfigureAwait(false);
                httpResponseMessage.EnsureSuccessStatusCode();

                var stream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);

                using var streamReader = new StreamReader(stream);
                using var reader = new JsonTextReader(streamReader);
                var serializer = JsonSerializer.Create(this.settings);
                var message = serializer.Deserialize<RpcResponseMessage>(reader);

                return message;
            }
            catch (TaskCanceledException ex)
            {
                var exception = new RpcClientTimeoutException($"Rpc timeout after {ConnectionTimeout.TotalMilliseconds} milliseconds", ex);
                throw exception;
            }
        }

        private async Task<HttpClient> GetHttpClientAsync()
        {
            var httpClient = this.httpClientFactory.CreateClient(Constants.DefaultHttpClient);
            httpClient.BaseAddress = new Uri(this.profile.RpcEndpoint);

            var token = await this.tokenProvider.GetAppTokenAsync(
                                                    this.profile.TenantId,
                                                    this.profile.Resource,
                                                    this.profile.ClientId,
                                                    this.profile.ClientSecret).ConfigureAwait(false);
            httpClient.DefaultRequestHeaders.Authorization = token.ToBearer();

            return httpClient;
        }
    }
}
