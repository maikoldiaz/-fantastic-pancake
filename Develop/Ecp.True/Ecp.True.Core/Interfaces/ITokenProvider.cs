// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITokenProvider.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// The AD token provider.
    /// </summary>
    public interface ITokenProvider
    {
        /// <summary>
        /// Gets the azure active directory app access token asynchronous.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <returns>
        /// The access token.
        /// </returns>
        Task<string> GetAppTokenAsync(string tenantId, string resource, string clientId, string clientSecret);
    }
}
