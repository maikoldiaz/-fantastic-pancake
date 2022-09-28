// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LegacyHttpClientProvider.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core
{
    using System.Net.Http;

    /// <summary>
    /// The legacy http client provider.
    /// </summary>
    public static class LegacyHttpClientProvider
    {
        /// <summary>
        /// Gets the cached HTTP Client.
        /// </summary>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/azure/azure-functions/manage-connections#use-static-clients.
        /// </remarks>
        /// <value>
        /// The http client.
        /// </value>
        public static HttpClient Current { get; } = new HttpClient();
    }
}
