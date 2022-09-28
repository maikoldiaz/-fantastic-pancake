// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphServiceClientFactory.cs" company="Microsoft">
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
    using Ecp.True.Graph.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Graph;
    using Microsoft.Identity.Web;
    using System;

    /// <summary>
    /// The graph service client.
    /// </summary>
    // [IoCRegistration(IoCLifetime.ContainerControlled)]
    public class GraphServiceClientFactory : IGraphServiceClientFactory
    {
        /// <summary>
        /// The graph info.
        /// </summary>
        private GraphSettings graphSettings;

        /// <summary>
        /// The token acquisition.
        /// </summary>
        private readonly ITokenAcquisition tokenAcquisition;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphServiceClientFactory" /> class.
        /// </summary>
        /// <param name="tokenAcquisition">The token acquisition.</param>
        public GraphServiceClientFactory(IConfiguration configuration, ITokenAcquisition tokenAcquisition)
        {
            if (configuration != null)
            {
                this.graphSettings = new GraphSettings();
                configuration.Bind("GraphApi", this.graphSettings);
            }
            this.tokenAcquisition = tokenAcquisition;
        }

        /// <summary>
        /// Initializes the specified graph settings.
        /// </summary>
        /// <param name="graph">The graph settings.</param>
        public void Initialize(GraphSettings graph)
        {
            this.graphSettings = graph;
        }

        /// <inheritdoc/>
        public GraphServiceClient AuthenticatedGraphClient()
        {
            if (this.graphSettings == null)
            {
                throw new ArgumentException(nameof(this.graphSettings));
            }
            return new GraphServiceClient(this.graphSettings.GraphApiPath, new GraphAuthenticationProvider(
                async () =>
                {
                    string result = await this.tokenAcquisition.GetAccessTokenForUserAsync(new[] { this.graphSettings.GraphScope }).ConfigureAwait(false);
                    return result;
                }));
        }
    }
}
