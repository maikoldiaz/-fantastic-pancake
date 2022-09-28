// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISapClient.cs" company="Microsoft">
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
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Configuration;

    /// <summary>
    /// The Ownership Rule Client Interface.
    /// </summary>
    public interface ISapClient
    {
        /// <summary>
        /// Posts the asynchronous.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="payload">The payload.</param>
        /// <returns>Returns Http Response message.</returns>
        Task<HttpResponseMessage> PostAsync(string path, object payload);

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Returns Http Response message.</returns>
        Task<HttpResponseMessage> GetAsync(string path);

        /// <summary>
        /// Initializes the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        void Initialize(SapSettings configuration);
    }
}