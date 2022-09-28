// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHttpClientProxy.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.OwnershipRules.Interfaces
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// The Http Client Proxy.
    /// </summary>
    public interface IHttpClientProxy
    {
        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="httpContent">Content of the HTTP.</param>
        /// <param name="token">The token.</param>
        /// <param name="isZipped">The zipped.</param>
        /// <param name="timeoutInMinutes">The timeout in minutes.</param>
        /// <returns>
        /// Returns Http Response Message.
        /// </returns>
        Task<HttpResponseMessage> SendAsync(HttpMethod method, Uri uri, HttpContent httpContent, string token, bool isZipped, int timeoutInMinutes);
    }
}
