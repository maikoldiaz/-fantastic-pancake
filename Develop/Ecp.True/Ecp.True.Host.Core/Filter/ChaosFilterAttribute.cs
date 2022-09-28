// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChaosFilterAttribute.cs" company="Microsoft">
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
    using Ecp.True.Chaos;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Core;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// The Chaos filter attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [CLSCompliant(false)]
    public sealed class ChaosFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The type.
        /// </summary>
        private readonly ChaosType type;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChaosFilterAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public ChaosFilterAttribute(ChaosType type)
        {
            this.type = type;
        }

        /// <inheritdoc/>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            var chaosManager = (IChaosManager)context.HttpContext.RequestServices.GetService(typeof(IChaosManager));
            if (!chaosManager.HasChaos)
            {
                base.OnActionExecuting(context);
                return;
            }

            var message = chaosManager.TryTriggerChaos(this.type.ToString());

            if (string.IsNullOrWhiteSpace(message))
            {
                base.OnActionExecuting(context);
                return;
            }

            context.Result = context.HttpContext.BuildChaosError(message);
        }
    }
}
