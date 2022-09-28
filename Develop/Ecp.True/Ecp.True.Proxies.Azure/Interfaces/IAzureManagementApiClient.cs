// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAzureManagementApiClient.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Entities.Configuration;

    /// <summary>
    /// The I AzureManagementApi Client.
    /// </summary>
    public interface IAzureManagementApiClient
    {
        /// <summary>
        /// Initializes the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        void Initialize(AvailabilitySettings configuration);

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <returns>The string..</returns>
        Task<string> GetAsync(Uri uri);
    }
}
