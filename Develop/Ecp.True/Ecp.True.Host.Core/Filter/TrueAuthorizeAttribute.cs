// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrueAuthorizeAttribute.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// The TrueAuthorize Attribute.
    /// </summary>
    /// <seealso cref="AuthorizeAttribute" />
    /// <seealso cref="IAuthorizationFilter" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class TrueAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrueAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="allowedRoles">The allowed roles.</param>
        public TrueAuthorizeAttribute(params Role[] allowedRoles)
        {
            this.AllowedRoles = allowedRoles.Select(role => role);
        }

        /// <summary>
        /// Gets the allowed roles.
        /// </summary>
        /// <value>
        /// The allowed roles.
        /// </value>
        public IEnumerable<Role> AllowedRoles { get; }

        /// <summary>
        /// Called early in the filter pipeline to confirm request is authorized.
        /// </summary>
        /// <param name="context">The <see cref="AuthorizationFilterContext" />.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            var configurationHandler = (IConfigurationHandler)context.HttpContext.RequestServices.GetService(typeof(IConfigurationHandler));
            var rolesConfig = await configurationHandler.GetConfigurationAsync<UserRoleSettings>(ConfigurationConstants.UserRoleSettings).ConfigureAwait(false);
            var userRoles = rolesConfig.Mapping.Where(a => this.AllowedRoles.Any(role => a.Key == role)).Select(d => d.Value);

            var principal = context.HttpContext.User;
            if (principal == null)
            {
                context.Result = new UnauthorizedResult((int)System.Net.HttpStatusCode.Unauthorized, AuthorizationErrorCode.Unauthorized);
                return;
            }

            BuildContext(context.HttpContext, rolesConfig.Mapping);

            if (!userRoles.Any())
            {
                return;
            }

            var authorized = principal.Claims.Any(claim => claim.Type == "groups" && userRoles.Any(groupObjectId => claim.Value.Contains(groupObjectId, StringComparison.InvariantCulture)));
            if (!authorized)
            {
                context.Result = new UnauthorizedResult((int)System.Net.HttpStatusCode.Forbidden, AuthorizationErrorCode.NoAccess);
            }
        }

        /// <summary>
        /// Populates the user information.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="rolesConfig">The roles configuration.</param>
        private static void BuildContext(HttpContext httpContext, Dictionary<Role, string> rolesConfig)
        {
            var principal = httpContext.User;
            var groupIds = principal.Claims.Where(claim => claim.Type == "groups").Select(a => a.Value);
            var roles = rolesConfig.Where(a => groupIds.Any(r => a.Value == r)).Select(d => d.Key.ToString());

            var businessContext = (IBusinessContext)httpContext.RequestServices.GetService(typeof(IBusinessContext));
            businessContext.Populate(principal.FindFirst("name")?.Value, principal.Identity.Name, roles);
        }
    }
}
