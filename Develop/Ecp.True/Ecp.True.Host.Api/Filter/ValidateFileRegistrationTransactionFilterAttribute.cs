// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateFileRegistrationTransactionFilterAttribute.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Core;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// The Validate File Registration Transaction Filter Attribute.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute" />
    [AttributeUsage(AttributeTargets.Method)]
    [CLSCompliant(false)]
    public sealed class ValidateFileRegistrationTransactionFilterAttribute : ActionFilterAttribute
    {
        /// <inheritdoc/>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(next, nameof(next));

            var retryIds = (int[])context.ActionArguments["retryIds"];
            var resourceProvider = (IResourceProvider)context.HttpContext.RequestServices.GetService(typeof(IResourceProvider));
            var repositoryFactory = (IRepositoryFactory)context.HttpContext.RequestServices.GetService(typeof(IRepositoryFactory));

            var errors = await ValidateFileRegistrationTransactionsAsync(retryIds, resourceProvider, repositoryFactory).ConfigureAwait(false);

            if (errors.Count > 0)
            {
                context.Result = context.HttpContext.BuildErrorResult(errors);
                return;
            }

            await base.OnActionExecutionAsync(context, next).ConfigureAwait(false);
        }

        private static async Task<List<ErrorInfo>> ValidateFileRegistrationTransactionsAsync(int[] retryIds, IResourceProvider resourceProvider, IRepositoryFactory repositoryFactory)
        {
            var errors = new List<ErrorInfo>();
            var repository = repositoryFactory.CreateRepository<FileRegistrationTransaction>();

            var tasks = new List<Task>();

            retryIds.ForEach(e => tasks.Add(ValidateAsync(e, resourceProvider, errors, repository)));
            await Task.WhenAll(tasks).ConfigureAwait(false);

            return errors;
        }

        private static async Task ValidateAsync(int retryId, IResourceProvider resourceProvider, List<ErrorInfo> errors, IRepository<FileRegistrationTransaction> repository)
        {
            FileRegistrationTransaction fileRegistrationTransaction;

            try
            {
                fileRegistrationTransaction = await repository.GetByIdAsync(retryId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                errors.Add(new ErrorInfo(ex.Message));
                return;
            }

            if (string.IsNullOrEmpty(fileRegistrationTransaction?.BlobPath) || fileRegistrationTransaction?.StatusTypeId != StatusType.FAILED)
            {
                errors.Add(new ErrorInfo(resourceProvider.GetResource(Entities.Constants.RetryMessageError)));
            }
        }
    }
}
