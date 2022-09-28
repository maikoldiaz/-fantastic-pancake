// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGraphService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Graph.Interfaces
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// The graph service.
    /// </summary>
    public interface IGraphService
    {
        /// <summary>
        /// Initializes the specified graph settings.
        /// </summary>
        /// <param name="graphSettings">The graph settings.</param>
        void Initialize(GraphSettings graphSettings);
        /// <summary>
        /// Gets my picture base64 asynchronous.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="identity">The identity.</param>
        /// <returns>
        /// The Base 64 encoded picture stream.
        /// </returns>
        Task<string> GetMyPictureBase64Async(HttpContext httpContext, ClaimsIdentity identity);
    }
}
