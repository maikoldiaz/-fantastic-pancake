// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeadLetterFilterAttribute.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core
{
    using System;
    using Ecp.True.Core;
    using Ecp.True.Proxies.Azure;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// The Dead letter filter attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [CLSCompliant(false)]
    public sealed class DeadLetterFilterAttribute : ActionFilterAttribute
    {
        /// <inheritdoc/>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            var deadLetterManager = (IDeadLetterManager)context.HttpContext.RequestServices.GetService(typeof(IDeadLetterManager));
            deadLetterManager.Initialize(true);
            base.OnActionExecuting(context);
        }
    }
}
