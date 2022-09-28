// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeTagController.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Core;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using EfCore.Models;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The NodeController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Admin")]
    public class NodeTagController : ODataController
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private readonly INodeTagProcessor nodeTagProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeTagController" /> class.
        /// </summary>
        /// <param name="nodeTagProcessor">The node tag processor.</param>
        public NodeTagController(INodeTagProcessor nodeTagProcessor)
        {
            this.nodeTagProcessor = nodeTagProcessor;
        }

        /// <summary>
        /// Gets all node associations asynchronous.
        /// </summary>
        /// <returns>The Action Result.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("nodetags")]
        [ODataRoute("nodetags")]
        [TrueAuthorize(Role.Administrator, Role.Query, Role.ProfessionalSegmentBalances, Role.Chain, Role.Programmer)]
        public Task<IQueryable<NodeTag>> QueryAllAsync()
        {
            return this.nodeTagProcessor.QueryAllAsync<NodeTag>(null);
        }

        /// <summary>
        /// Associates the nodes.
        /// </summary>
        /// <param name="taggedNodeInfo">The tag nodes.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/nodetag")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> TagNodeAsync(TaggedNodeInfo taggedNodeInfo)
        {
            await this.nodeTagProcessor.TagNodeAsync(taggedNodeInfo).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.AssociationSuccess);
        }
    }
}
