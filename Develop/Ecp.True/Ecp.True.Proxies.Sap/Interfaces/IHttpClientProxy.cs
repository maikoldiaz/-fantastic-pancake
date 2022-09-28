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

namespace Ecp.True.Proxies.Sap.Interfaces
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
        /// <param name="name">Name of the user.</param>
        /// <param name="password">The user password.</param>
        /// <returns>The http response message.</returns>
        Task<HttpResponseMessage> SendAsync(HttpMethod method, Uri uri, HttpContent httpContent, string name, string password);
    }
}
