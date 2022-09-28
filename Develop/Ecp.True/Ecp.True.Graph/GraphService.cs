// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Graph
{
    using System;
    using System.IO;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Ecp.True.Graph.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Graph;

    /// <summary>
    /// The graph service to access user information using Graph API.
    /// </summary>
    public class GraphService : IGraphService
    {
        /// <summary>
        /// The graph client.
        /// </summary>
        private readonly IGraphServiceClientFactory graphServiceClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphService" /> class.
        /// </summary>
        /// <param name="graphServiceClientFactory">The graph service client factory.</param>
        public GraphService(IGraphServiceClientFactory graphServiceClientFactory)
        {
            this.graphServiceClientFactory = graphServiceClientFactory;
        }

        public void Initialize(GraphSettings graphSettings)
        {
            this.graphServiceClientFactory.Initialize(graphSettings);
        }

        /// <inheritdoc/>
        public async Task<string> GetMyPictureBase64Async(HttpContext httpContext, ClaimsIdentity identity)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            try
            {
                var client = this.graphServiceClientFactory.AuthenticatedGraphClient();
                var pictureStream = await client.Me.Photo.Content.Request().GetAsync().ConfigureAwait(false);

                // Copy stream to MemoryStream object so that it can be converted to byte array.
                using (var ms = new MemoryStream())
                {
                    await pictureStream.CopyToAsync(ms).ConfigureAwait(false);

                    var pictureByteArray = ms.ToArray();
                    var pictureBase64 = Convert.ToBase64String(pictureByteArray);

                    return "data:image/jpeg;base64, " + pictureBase64;
                }
            }
            catch (ServiceException)
            {
                return null;
            }
        }
    }
}
