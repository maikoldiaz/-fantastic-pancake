// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Controllers
{
    using System;
    using Ecp.True.Core;
    using Ecp.True.Logging;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    /// <summary>
    /// The account controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<AccountController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public AccountController(ITrueLogger<AccountController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Signs the user in..
        /// </summary>
        /// <param name="returnPath">The return path.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        [HttpGet]
        public IActionResult SignIn(string returnPath = "/")
        {
            if (string.IsNullOrEmpty(returnPath))
            {
                this.logger.LogError($"Return path not found, signing out. Business Context: {JsonConvert.SerializeObject(this.User)}", Constants.AuthenticationLogTag);
                return this.RedirectToAction(nameof(AccountController.SignOut), "Account");
            }

            if (!this.User.Identity.IsAuthenticated)
            {
                var redirectUrl = returnPath.Equals("/", StringComparison.OrdinalIgnoreCase) ? returnPath : this.Url.Action(nameof(this.SignIn), "Account", new { returnPath }, this.Request.Scheme);
                return this.Challenge(
                    new AuthenticationProperties { RedirectUri = redirectUrl },
                    OpenIdConnectDefaults.AuthenticationScheme);
            }

            if (!returnPath.Equals("/", StringComparison.OrdinalIgnoreCase) && this.Url.IsLocalUrl(returnPath))
            {
                this.logger.LogInformation($"Redirecting to return path: {returnPath}", Constants.AuthenticationLogTag);
                return this.Redirect(returnPath);
            }

            return this.RedirectToAction(nameof(AppController.Index), "App");
        }

        /// <summary>
        /// Represents an event that is raised when the sign-out operation is complete.
        /// </summary>
        /// <param name="returnPath">The return path.</param>
        /// <param name="forceSignout">if set to <c>true</c> [force signout].</param>
        /// <returns>
        /// The action result.
        /// </returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignOut(string returnPath = "/", bool forceSignout = false)
        {
            ArgumentValidators.ThrowIfNull(returnPath, nameof(returnPath));

            string callbackUrl = returnPath.Equals("/", StringComparison.OrdinalIgnoreCase) ?
                this.Url.Action(nameof(this.SignIn), "Account") : this.Url.Action(nameof(this.SignIn), "Account", new { returnPath }, this.Request.Scheme);

            if (this.User.Identity.IsAuthenticated || forceSignout)
            {
                return this.SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                OpenIdConnectDefaults.AuthenticationScheme,
                CookieAuthenticationDefaults.AuthenticationScheme);
            }

            if (callbackUrl.StartsWith(this.Request.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                callbackUrl = new Uri(callbackUrl).PathAndQuery;
            }

            if (this.Url.IsLocalUrl(callbackUrl))
            {
                return this.Redirect(callbackUrl);
            }

            return new UnauthorizedResult();
        }

        /// <summary>
        /// Signed out user.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        public IActionResult SignedOut()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return this.RedirectToAction(nameof(AppController.Index), "App");
            }

            return this.View();
        }
    }
}
