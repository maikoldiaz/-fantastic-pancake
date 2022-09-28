// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AntiforgeryRequestTokenFilterAttribute.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Filters
{
    using System;
    using Ecp.True.Core;
    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// The antiforgery request token filter attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AntiforgeryRequestTokenFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The antiforgery.
        /// </summary>
        private readonly IAntiforgery antiforgery;

        /// <summary>
        /// Initializes a new instance of the <see cref="AntiforgeryRequestTokenFilterAttribute"/> class.
        /// </summary>
        /// <param name="antiforgery">The anti forgery.</param>
        public AntiforgeryRequestTokenFilterAttribute(IAntiforgery antiforgery)
        {
            this.antiforgery = antiforgery;
        }

        /// <inheritdoc/>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            var tokens = this.antiforgery.GetAndStoreTokens(context.HttpContext);
            if (context.HttpContext != null)
            {
                context.HttpContext.Response.Headers.Append("XSRF-TOKEN", tokens.RequestToken);
                context.HttpContext.Response.Headers.Remove("X-Forwarded-Host");
                context.HttpContext.Response.Headers.Remove("X-Host");
                context.HttpContext.Response.Headers.Remove("X-Forwarded-Server");
                context.HttpContext.Response.Headers.Remove("X-Powered-By");
                context.HttpContext.Response.Headers.Remove("X-AspNet-Version");
                context.HttpContext.Response.Headers.Remove("X-AspNetMvc-Version");
            }
        }
    }
}
