// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateNodeConnectionListAttribute.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Host.Core.Result;
    using Microsoft.AspNetCore.Mvc.Filters;
    using EntityConstants = Ecp.True.Entities.Constants;

    /// <summary>
    /// The Node connection list Validation Attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [CLSCompliant(false)]
    public sealed class ValidateNodeConnectionListAttribute : ActionFilterAttribute
    {
        private IResourceProvider resourceProvider;
        private IRepository<Node> nodeRepository;

        /// <inheritdoc/>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            this.Initialize(context);

            await this.ValidateSourceAndDestinationNodesAsync(context).ConfigureAwait(false);

            await base.OnActionExecutionAsync(context, next).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the source and DestinationNodes.
        /// </summary>
        /// <param name="nodeConnectionList">The NodeConnectionList.</param>
        /// <returns>The IDs enumerable.</returns>
        private static IEnumerable<int?> GetSourceAndDestinationNodes(IReadOnlyCollection<NodeConnection> nodeConnectionList)
        {
            return nodeConnectionList.Select(c => c.SourceNodeId)
                .Union(nodeConnectionList.Select(c => c.DestinationNodeId)).ToList();
        }

        /// <summary>
        /// Gets the action argument.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="context">The cation executing context.</param>
        /// <param name="name">The name of the argument.</param>
        /// <returns>The action argument.</returns>
        private static T GetActionArgument<T>(ActionExecutingContext context, string name)
        {
            return (T)context.ActionArguments[name];
        }

        /// <summary>
        /// Gets the specified service.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="context">The executing context.</param>
        /// <returns>The specified service.</returns>
        private static T GetService<T>(ActionExecutingContext context)
        {
            return (T)context.HttpContext.RequestServices.GetService(typeof(T));
        }

        /// <summary>
        /// Builds the error response with the aggregated errors.
        /// </summary>
        /// <param name="context">The cation executing context.</param>
        /// <param name="infoList">The nodeConnectionInfoList DTO that aggregates the errors.</param>
        private static void BuildErrorResponse(ActionExecutingContext context, List<NodeConnectionInfo> infoList)
        {
            if (infoList.Any())
            {
                context.Result = new MultiStatusResult(infoList);
            }
        }

        /// <summary>
        /// Adds the error info collection to the NodeConnectionInfo DTO.
        /// </summary>
        /// <param name="errors">The collection of errors.</param>
        /// <param name="info">The NodeConnectionInfo DTO.</param>
        /// <param name="infoList">The list of NodeConnectionInfoDTOs.</param>
        private static void AddErrors(ICollection<ErrorInfo> errors, NodeConnectionInfo info, List<NodeConnectionInfo> infoList)
        {
            if (errors.Any())
            {
                info.Errors = new ErrorResponse(errors.ToList());
                info.Status = EntityInfoCreationStatus.Error;
                infoList.Add(info);
            }
        }

        /// <summary>
        /// Gets the existing and active nodes asynchronously.
        /// </summary>
        /// <param name="nodes">The node id enumerable.</param>
        /// <returns>The existing active nodes.</returns>
        private async Task<List<int>> GetExistingActiveNodesAsync(IEnumerable<int?> nodes)
        {
            return (await this.nodeRepository
                    .GetAllAsync(
                        x => nodes.Contains(x.NodeId)
                        && x.IsActive.GetValueOrDefault()).ConfigureAwait(false))
                .Select(n => n.NodeId).ToList();
        }

        /// <summary>
        /// Validates the source and destination nodes.
        /// </summary>
        /// <param name="context">The action executing context.</param>
        /// <returns>The task.</returns>
        private async Task ValidateSourceAndDestinationNodesAsync(ActionExecutingContext context)
        {
            var nodeConnectionList = GetActionArgument<List<NodeConnection>>(context, "nodeConnections");
            var nodes = GetSourceAndDestinationNodes(nodeConnectionList);
            var existingNodes = await this.GetExistingActiveNodesAsync(nodes).ConfigureAwait(false);

            var infoList = new List<NodeConnectionInfo>();

            foreach (var nodeConnection in nodeConnectionList)
            {
                ICollection<ErrorInfo> errors = new List<ErrorInfo>();
                var info = new NodeConnectionInfo
                {
                    NodeConnection = nodeConnection,
                };

                this.ValidateNodeExists(existingNodes, nodeConnection.SourceNodeId, errors, EntityConstants.SourceNodeIdentifierNotFound);
                this.ValidateNodeExists(existingNodes, nodeConnection.DestinationNodeId, errors, EntityConstants.DestinationNodeIdentifierNotFound);

                AddErrors(errors, info, infoList);
            }

            BuildErrorResponse(context, infoList);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="context">The cation executing context.</param>
        private void Initialize(ActionExecutingContext context)
        {
            var repoFactory = GetService<IRepositoryFactory>(context);
            this.nodeRepository = repoFactory.CreateRepository<Node>();
            this.resourceProvider = GetService<IResourceProvider>(context);
        }

        /// <summary>
        /// Validates whether a node exists in a collection and adds the appropriate error.
        /// </summary>
        /// <param name="existingNodes">The existing node connection.</param>
        /// <param name="nodeId">The node id.</param>
        /// <param name="errors">The list of errors.</param>
        /// <param name="errorMessage">The error message.</param>
        private void ValidateNodeExists(List<int> existingNodes, int? nodeId, ICollection<ErrorInfo> errors, string errorMessage)
        {
            if (!existingNodes.Contains(nodeId.GetValueOrDefault()))
            {
                errors.Add(new ErrorInfo(this.resourceProvider.GetResource(errorMessage)));
            }
        }
    }
}