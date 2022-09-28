// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateOwnershipNodeAttribute.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Flow.Api.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Host.Core;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// The Validate Ownership Node Attribute.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute" />
    [AttributeUsage(AttributeTargets.Method)]
    [CLSCompliant(false)]
    public sealed class ValidateOwnershipNodeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// On Action Executing.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <inheritdoc />
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(next, nameof(next));
            var approvalRequest = (NodeOwnershipApprovalRequest)context.ActionArguments["approvalRequest"];
            var repositoryFactory = (IRepositoryFactory)context.HttpContext.RequestServices.GetService(typeof(IRepositoryFactory));
            var errors = await ValidateOwnershipNodeAsync(approvalRequest, repositoryFactory).ConfigureAwait(false);
            if (errors.Count > 0)
            {
                context.Result = context.HttpContext.BuildErrorResult(errors);
                return;
            }

            await base.OnActionExecutionAsync(context, next).ConfigureAwait(false);
        }

        /// <summary>
        /// Validates the ownership node asynchronous.
        /// </summary>
        /// <param name="approvalRequest">The approval request.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <returns>The List Or Errors.</returns>
        private static async Task<List<ErrorInfo>> ValidateOwnershipNodeAsync(NodeOwnershipApprovalRequest approvalRequest, IRepositoryFactory repositoryFactory)
        {
            var errors = new List<ErrorInfo>();
            var ownershipNodeRepository = repositoryFactory.CreateRepository<OwnershipNode>();
            var ownershipNode = await ownershipNodeRepository.GetByIdAsync(approvalRequest.OwnershipNodeId).ConfigureAwait(false);

            if (ownershipNode == null)
            {
                errors.Add(new ErrorInfo(Entities.Constants.OwnershipNodeNotFound));
                return errors;
            }

            if (!ownershipNode.OwnershipStatus.Equals(OwnershipNodeStatusType.SUBMITFORAPPROVAL))
            {
                errors.Add(new ErrorInfo(Entities.Constants.InvalidNodeStateApproval));
            }
            else
            {
                if (!(approvalRequest.Status.EqualsIgnoreCase(OwnershipNodeStatusType.APPROVED.ToString()) ||
                    approvalRequest.Status.EqualsIgnoreCase(OwnershipNodeStatusType.REJECTED.ToString())))
                {
                    errors.Add(new ErrorInfo(Entities.Constants.InvalidRequestStatus));
                }
            }

            return errors;
        }
    }
}
