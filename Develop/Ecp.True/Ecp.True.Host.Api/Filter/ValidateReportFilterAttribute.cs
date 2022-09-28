// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateReportFilterAttribute.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Host.Core;
    using Ecp.True.Processors.Core;
    using Microsoft.AspNetCore.Mvc.Filters;
    using EntityConstants = Ecp.True.Entities.Constants;

    /// <summary>
    /// Validate Report Filter Attribute.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute" />
    [AttributeUsage(AttributeTargets.Method)]
    [CLSCompliant(false)]
    public sealed class ValidateReportFilterAttribute : ActionFilterAttribute
    {
        /// <inheritdoc/>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            var reportExecution = (ReportExecution)context.ActionArguments["execution"];
            var resourceProvider = (IResourceProvider)context.HttpContext.RequestServices.GetService(typeof(IResourceProvider));
            var repositoryFactory = (IRepositoryFactory)context.HttpContext.RequestServices.GetService(typeof(IRepositoryFactory));

            var repository = repositoryFactory.CreateRepository<ReportExecution>();
            var hash = IdGenerator.GenerateReportHash(reportExecution);

            var count = await repository.GetCountAsync(a => a.Hash == hash && a.StatusTypeId == StatusType.PROCESSING).ConfigureAwait(false);

            if (count == 0)
            {
                await base.OnActionExecutionAsync(context, next).ConfigureAwait(false);
                return;
            }

            context.Result = context.HttpContext.BuildErrorResult(resourceProvider.GetResource(EntityConstants.ReportAlreadyProcessing));
        }
    }
}
