// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateNodeFilterAttribute.cs" company="Microsoft">
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
    using Ecp.True.Host.Core;
    using EfCore.Models;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// The Node Create Validation Attribute.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute" />
    [AttributeUsage(AttributeTargets.Method)]
    [CLSCompliant(false)]
    public sealed class ValidateNodeFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The update.
        /// </summary>
        private readonly bool update;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateNodeFilterAttribute"/> class.
        /// </summary>
        /// <param name="update">if set to <c>true</c> [update].</param>
        public ValidateNodeFilterAttribute(bool update)
        {
            this.update = update;
        }

        /// <summary>
        /// On Action Executing.
        /// </summary>
        /// <param name="context">Filter context.</param>
        /// <param name="next">Next.</param>
        /// <returns>A task.</returns>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            var node = (Node)context.ActionArguments["node"];
            var resourceProvider = (IResourceProvider)context.HttpContext.RequestServices.GetService(typeof(IResourceProvider));

            var repositoryFactory = (IRepositoryFactory)context.HttpContext.RequestServices.GetService(typeof(IRepositoryFactory));

            var errors = this.update ?
                await ValidateUpdateNodeAsync(node, resourceProvider, repositoryFactory).ConfigureAwait(false) :
                await ValidateCreateNodeAsync(node, resourceProvider, repositoryFactory).ConfigureAwait(false);

            if (errors.Count > 0)
            {
                context.Result = context.HttpContext.BuildErrorResult(errors);
            }

            await base.OnActionExecutionAsync(context, next).ConfigureAwait(false);
        }

        /// <summary>
        /// Validates the update node data.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="resourceProvider">The resource provider.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <returns>
        /// The list of errorInfo.
        /// </returns>
        private static async Task<List<ErrorInfo>> ValidateUpdateNodeAsync(Node node, IResourceProvider resourceProvider, IRepositoryFactory repositoryFactory)
        {
            var errors = new List<ErrorInfo>();

            var existingNode = await GetNodeByNameAsync(node.Name, repositoryFactory).ConfigureAwait(false);

            if (existingNode != null && existingNode.NodeId != node.NodeId)
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.NodeNameMustBeUnique)));
            }

            if (node.NodeId == 0)
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.InvalidNodeId)));
            }

            if (node.NodeStorageLocations.Any(s => s.NodeId == 0))
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.InvalidNodeStorageLocations)));
            }

            DoValidate(node, resourceProvider, errors);

            return errors;
        }

        /// <summary>
        /// Validates the create node data.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="resourceProvider">The resource provider.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <returns>
        /// The collection of ErrorInfo.
        /// </returns>
        private static async Task<List<ErrorInfo>> ValidateCreateNodeAsync(Node node, IResourceProvider resourceProvider, IRepositoryFactory repositoryFactory)
        {
            ArgumentValidators.ThrowIfNull(repositoryFactory, nameof(repositoryFactory));

            var errors = new List<ErrorInfo>();

            var categoryElementRepository = repositoryFactory.CreateRepository<CategoryElement>();
            var existingNode = await GetNodeByNameAsync(node.Name, repositoryFactory).ConfigureAwait(false);

            if (existingNode != null)
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.NodeNameMustBeUnique)));
            }

            await ValidateNodeTagPropertiesAsync(node, resourceProvider, errors, categoryElementRepository).ConfigureAwait(false);

            if (node.IsActive == false)
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.NodeMustBeActive)));
            }

            if (node.NodeId != 0)
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.InvalidNodeId)));
            }

            if (node.NodeStorageLocations.Any(s => s.NodeStorageLocationId != 0 || s.IsActive != true))
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.InvalidNodeStorageLocations)));
            }

            if (node.NodeStorageLocations.SelectMany(s => s.Products).Any(p => p.StorageLocationProductId != 0 || p.IsActive != true))
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.InvalidProductLocations)));
            }

            DoValidate(node, resourceProvider, errors);

            return errors;
        }

        /// <summary>
        /// Gets the node by name asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <returns>
        /// The Node.
        /// </returns>
        private static async Task<Node> GetNodeByNameAsync(string name, IRepositoryFactory repositoryFactory)
        {
            var nodeRepository = repositoryFactory.CreateRepository<Node>();
            return await nodeRepository.SingleOrDefaultAsync(a => a.Name == name).ConfigureAwait(false);
        }

        private static async Task ValidateNodeTagPropertiesAsync(
            Node node,
            IResourceProvider resourcesProvider,
            ICollection<ErrorInfo> errors,
            IRepository<CategoryElement> categoryElementRepo)
        {
            ArgumentValidators.ThrowIfNull(categoryElementRepo, nameof(categoryElementRepo));

            if (node.NodeTypeId == null)
            {
                errors.Add(new ErrorInfo(resourcesProvider.GetResource(Entities.Constants.NodeTypeRequired)));
            }
            else
            {
                var nodeTypeElement = await categoryElementRepo.FirstOrDefaultAsync(x => x.CategoryId == (int)NodeTagType.NodeType && x.ElementId == node.NodeTypeId).ConfigureAwait(false);

                if (nodeTypeElement == null)
                {
                    errors.Add(new ErrorInfo(resourcesProvider.GetResource(Entities.Constants.NodeTypeDoesNotExist)));
                }
            }

            if (node.SegmentId == null)
            {
                errors.Add(new ErrorInfo(resourcesProvider.GetResource(Entities.Constants.SegmentRequired)));
            }
            else
            {
                var segmentElement = await categoryElementRepo.FirstOrDefaultAsync(x => x.CategoryId == (int)NodeTagType.Segment && x.ElementId == node.SegmentId).ConfigureAwait(false);

                if (segmentElement == null)
                {
                    errors.Add(new ErrorInfo(resourcesProvider.GetResource(Entities.Constants.SegmentDoesNotExist)));
                }
            }

            if (node.OperatorId == null)
            {
                errors.Add(new ErrorInfo(resourcesProvider.GetResource(Entities.Constants.OperatorRequired)));
            }
            else
            {
                var operatorElement = await categoryElementRepo.FirstOrDefaultAsync(x => x.CategoryId == (int)NodeTagType.Operator && x.ElementId == node.OperatorId).ConfigureAwait(false);

                if (operatorElement == null)
                {
                    errors.Add(new ErrorInfo(resourcesProvider.GetResource(Entities.Constants.OperatorDoesNotExist)));
                }
            }
        }

        private static void DoValidate(Node node, IResourceProvider resourceProvider, IList<ErrorInfo> errors)
        {
            var names = node.NodeStorageLocations.Select(x => x.Name);
            if (names.Count() != names.Distinct().Count())
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.StorageNameMustBeUnique)));
            }

            if (node.Capacity != null && node.UnitId == null)
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.NodeCapacityUnitRequiredValidation)));
            }

            if (node.Capacity == null && node.UnitId != null)
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.NodeCapacityRequiredValidation)));
            }
        }
    }
}