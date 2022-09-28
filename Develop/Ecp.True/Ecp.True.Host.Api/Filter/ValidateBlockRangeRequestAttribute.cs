// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateBlockRangeRequestAttribute.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Core;
    using Ecp.True.Processors.Blockchain.Interfaces;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// The validate block request attribute.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute" />
    [AttributeUsage(AttributeTargets.Method)]
    [CLSCompliant(false)]
    public sealed class ValidateBlockRangeRequestAttribute : ActionFilterAttribute
    {
        /// <inheritdoc/>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            var request = (BlockRangeRequest)context.ActionArguments["request"];
            var resourceProvider = (IResourceProvider)context.HttpContext.RequestServices.GetService(typeof(IResourceProvider));
            var processor = (IBlockchainProcessor)context.HttpContext.RequestServices.GetService(typeof(IBlockchainProcessor));

            var isHeadValid = await processor.HasBlockAsync(request.HeadBlock).ConfigureAwait(false);
            var isTailValid = await processor.HasBlockAsync(request.TailBlock).ConfigureAwait(false);

            if (!isHeadValid || !isTailValid)
            {
                context.Result = context.HttpContext.BuildErrorResult(resourceProvider.GetResource(Entities.Constants.InvalidBlockNumberSupplied));
                return;
            }

            await base.OnActionExecutionAsync(context, next).ConfigureAwait(false);
        }
    }
}
