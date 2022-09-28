// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateNodeConnectionAttribute.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
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
    using Ecp.True.Host.Core;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;
    using EntityConstants = Ecp.True.Entities.Constants;

    /// <summary>
    ///  The Node connection Validation Attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [CLSCompliant(false)]
    public sealed class ValidateNodeConnectionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The has node connection argument object.
        /// </summary>
        private readonly bool hasNodeConnectionArgumentObject;

        /// <summary>
        /// The node connection object argument name.
        /// </summary>
        private readonly string nodeConnectionObjectArgumentName;

        /// <summary>
        /// The source node identifier argument name.
        /// </summary>
        private readonly string sourceNodeIdArgumentName;

        /// <summary>
        /// The destination node identifier argument name.
        /// </summary>
        private readonly string destinationNodeIdArgumentName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateNodeConnectionAttribute" /> class.
        /// </summary>
        /// <param name="hasNodeConnectionArgumentObject">if set to <c>true</c> [has node connection argument object].</param>
        /// <param name="nodeConnectionObjectArgumentName">Name of the argument.</param>
        public ValidateNodeConnectionAttribute(bool hasNodeConnectionArgumentObject, string nodeConnectionObjectArgumentName)
        {
            this.hasNodeConnectionArgumentObject = hasNodeConnectionArgumentObject;
            this.nodeConnectionObjectArgumentName = nodeConnectionObjectArgumentName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateNodeConnectionAttribute"/> class.
        /// </summary>
        /// <param name="sourceNodeIdArgumentName">Name of the source node identifier argument.</param>
        /// <param name="destinationNodeIdArgumentName">Name of the destination node identifier argument.</param>
        public ValidateNodeConnectionAttribute(string sourceNodeIdArgumentName, string destinationNodeIdArgumentName)
        {
            this.sourceNodeIdArgumentName = sourceNodeIdArgumentName;
            this.destinationNodeIdArgumentName = destinationNodeIdArgumentName;
        }

        /// <inheritdoc />
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(next, nameof(next));

            int sourceNodeId;
            int destinationNodeId;
            IList<ErrorInfo> errors = new List<ErrorInfo>();

            var resourceProvider = (IResourceProvider)context.HttpContext.RequestServices.GetService(typeof(IResourceProvider));

            if (this.hasNodeConnectionArgumentObject)
            {
                var nodeConnection = (NodeConnection)context.ActionArguments[this.nodeConnectionObjectArgumentName];
                ArgumentValidators.ThrowIfNull(nodeConnection, nameof(nodeConnection));

                errors = ValidateOwnership(nodeConnection, resourceProvider, context.HttpContext);

                sourceNodeId = nodeConnection.SourceNodeId.GetValueOrDefault();
                destinationNodeId = nodeConnection.DestinationNodeId.GetValueOrDefault();

                if (nodeConnection.IsTransfer == false && nodeConnection.AlgorithmId != null)
                {
                    errors.Add(new ErrorInfo(resourceProvider.GetResource(EntityConstants.NoTransferWithAlgorithmIdMessage)));
                }

                if (nodeConnection.IsTransfer == true && nodeConnection.AlgorithmId == null)
                {
                    errors.Add(new ErrorInfo(resourceProvider.GetResource(EntityConstants.TransferWithNoAlgorithmIdMessage)));
                }
            }
            else
            {
                ArgumentValidators.ThrowIfNull(this.sourceNodeIdArgumentName, nameof(this.sourceNodeIdArgumentName));
                ArgumentValidators.ThrowIfNull(this.destinationNodeIdArgumentName, nameof(this.destinationNodeIdArgumentName));

                sourceNodeId = (int)context.ActionArguments[this.sourceNodeIdArgumentName];
                destinationNodeId = (int)context.ActionArguments[this.destinationNodeIdArgumentName];
            }

            if (errors.Count > 0)
            {
                context.Result = context.HttpContext.BuildErrorResult(errors);
                return;
            }

            var repositoryFactory = (IRepositoryFactory)context.HttpContext.RequestServices.GetService(typeof(IRepositoryFactory));
            errors = await ValidateConnectionAsync(repositoryFactory, resourceProvider, sourceNodeId, destinationNodeId).ConfigureAwait(false);

            if (errors.Count > 0)
            {
                context.Result = context.HttpContext.BuildErrorResult(errors);
                return;
            }

            await base.OnActionExecutionAsync(context, next).ConfigureAwait(false);
        }

        private static async Task<IList<ErrorInfo>> ValidateConnectionAsync(
            IRepositoryFactory repositoryFactory,
            IResourceProvider resourceProvider,
            int sourceNodeId,
            int destinationNodeId)
        {
            ArgumentValidators.ThrowIfNull(repositoryFactory, nameof(repositoryFactory));

            var errors = new List<ErrorInfo>();

            var nodeRepository = repositoryFactory.CreateRepository<Node>();
            var nodes = await nodeRepository.GetAllAsync(x => x.NodeId == sourceNodeId || x.NodeId == destinationNodeId).ConfigureAwait(false);

            if (!nodes.Any(x => x.NodeId == sourceNodeId))
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(EntityConstants.SourceNodeIdentifierNotFound)));
            }

            if (!nodes.Any(x => x.NodeId == destinationNodeId))
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(EntityConstants.DestinationNodeIdentifierNotFound)));
            }

            if (nodes.Any(x => x.IsActive == false))
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(EntityConstants.InvalidNodeStatusForConnection)));
            }

            return errors;
        }

        private static IList<ErrorInfo> ValidateOwnership(NodeConnection connection, IResourceProvider resourceProvider, HttpContext context)
        {
            var errors = new List<ErrorInfo>();
            if (!context.IsPost() || connection.Products == null || connection.Products.Count == 0)
            {
                return errors;
            }

            connection.Products.ForEach(p =>
            {
                // For every product with owners, ensure ownership percentage is always 100.
                if (p.Owners != null && p.Owners.Count > 0 && p.TotalOwnerShipValue != 100)
                {
                    errors.Add(new ErrorInfo(resourceProvider.GetResource(EntityConstants.ProductOwnerShipTotalValueValidation)));
                }
            });

            return errors;
        }
    }
}
