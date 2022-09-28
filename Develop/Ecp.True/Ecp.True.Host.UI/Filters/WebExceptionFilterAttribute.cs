// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebExceptionFilterAttribute.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Host.Core;
    using Ecp.True.Logging;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Identity.Client;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    /// <summary>
    /// The MSAL token acquisition exception filter.
    /// </summary>
    /// <seealso cref="WebExceptionFilterAttribute" />
    [AttributeUsage(AttributeTargets.Class)]
    [CLSCompliant(false)]
    public sealed class WebExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ITrueLogger<WebExceptionFilterAttribute> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebExceptionFilterAttribute"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public WebExceptionFilterAttribute(ITrueLogger<WebExceptionFilterAttribute> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// On Exception.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <inheritdoc />
        public override void OnException(ExceptionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            if (context.Exception is MsalUiRequiredException
                || context.Exception is AdalSilentTokenAcquisitionException)
            {
                this.logger.LogError(context.Exception, Constants.UnhandledErrorMessage, Constants.UnhandledExceptionTag);
                if (!context.HttpContext.Request.Headers.TryGetValue(Constants.TrueOriginHeader, out var headerValue))
                {
                    var returnPath = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString.ToString();

                    // Send user to Sign out which will force re-login
                    context.Result = new RedirectToRouteResult(
                            new RouteValueDictionary
                            {
                                {
                                    "area", string.Empty
                                },
                                {
                                    "controller", "Account"
                                },
                                {
                                    "action", "SignOut"
                                },
                                {
                                    "returnPath", returnPath
                                },
                            });

                    return;
                }

                context.HttpContext.Response.OnStarting(() =>
                {
                    context.HttpContext.Response.Headers.Add(Constants.RedirectPathOnAuthFailureHeader, Constants.RedirectPathOnAuthFailurePath);
                    return Task.CompletedTask;
                });

                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();

                return;
            }

            this.logger.LogError(context.Exception, context.Exception.Message);

            var trueWebException = context.Exception as TrueWebException;
            if (trueWebException != null)
            {
                context.Result = context.HttpContext.BuildErrorResult(trueWebException);
                return;
            }

            var unauthorizedAccessException = context.Exception as UnauthorizedAccessException;
            if (unauthorizedAccessException != null)
            {
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                return;
            }

            context.Result = new StatusCodeResult(500);
        }
    }
}
