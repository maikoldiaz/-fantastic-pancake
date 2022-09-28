// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrueAuthorizeAttributeTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The TrueAuthorizeAttributeTests.
    /// </summary>
    [TestClass]
    public class TrueAuthorizeAttributeTests
    {
        /// <summary>
        /// The mock configuration handler.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// The mock business context.
        /// </summary>
        private Mock<IBusinessContext> mockBusinessContext;

        /// <summary>
        /// The true authorize attribute.
        /// </summary>
        private TrueAuthorizeAttribute trueAuthorizeAttribute;

        /// <summary>
        /// The HTTP context.
        /// </summary>
        private HttpContext httpContext;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.httpContext = new DefaultHttpContext();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockBusinessContext = new Mock<IBusinessContext>();
        }

        /// <summary>
        /// Trues the authorize attribute should return authorize user if user is in authorized roles.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task TrueAuthorizeAttribute_ShouldReturnAuthorizeUser_IfUserIsInAuthorizedRolesAsync()
        {
            // Arrange
            var roles = new List<Role> { Role.Administrator };
            this.trueAuthorizeAttribute = new TrueAuthorizeAttribute(roles.ToArray());

            var identity = new GenericIdentity("groups");
            identity.AddClaim(new Claim("groups", "a7479414-8c25-4afc-a7b4-b7652032649d"));
            var claimsPrincipal = new GenericPrincipal(identity, null);
            this.httpContext.User = claimsPrincipal;
            this.SetupHttpContext();
            var actionContext = new ActionContext();
            actionContext.HttpContext = this.httpContext;
            actionContext.RouteData = new RouteData();
            actionContext.ActionDescriptor = new ControllerActionDescriptor();

            var filter = new Mock<IList<IFilterMetadata>>();

            AuthorizationFilterContext authorizationFilterContext = new AuthorizationFilterContext(actionContext, filter.Object);

            authorizationFilterContext.HttpContext = this.httpContext;

            await this.trueAuthorizeAttribute.OnAuthorizationAsync(authorizationFilterContext).ConfigureAwait(false);

            // Assert
            Assert.IsNull(authorizationFilterContext.Result);
        }

        /// <summary>
        /// Trues the authorize attribute should return un authorize if user is not in authorized roles.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task TrueAuthorizeAttribute_ShouldReturnUnAuthorize_IfUserIsNotInAuthorizedRolesAsync()
        {
            // Arrange
            var roles = new List<Role> { Role.Administrator };
            this.trueAuthorizeAttribute = new TrueAuthorizeAttribute(roles.ToArray());

            var identity = new GenericIdentity("groups");
            identity.AddClaim(new Claim("groups", "e2577c59-a827-4a84-8e65-1a79ec752c61"));
            var claimsPrincipal = new GenericPrincipal(identity, null);
            this.httpContext.User = claimsPrincipal;
            this.SetupHttpContext();
            var actionContext = new ActionContext();
            actionContext.HttpContext = this.httpContext;
            actionContext.RouteData = new RouteData();
            actionContext.ActionDescriptor = new ControllerActionDescriptor();

            var filter = new Mock<IList<IFilterMetadata>>();

            AuthorizationFilterContext authorizationFilterContext = new AuthorizationFilterContext(actionContext, filter.Object);

            authorizationFilterContext.HttpContext = this.httpContext;

            await this.trueAuthorizeAttribute.OnAuthorizationAsync(authorizationFilterContext).ConfigureAwait(false);

            var result = (Core.UnauthorizedResult)authorizationFilterContext.Result;

            Assert.IsNotNull(authorizationFilterContext.Result);
            Assert.AreEqual((int)System.Net.HttpStatusCode.Forbidden, result.StatusCode);
            Assert.AreEqual(AuthorizationErrorCode.NoAccess, result.ErrorCode);
        }

        /// <summary>
        /// Trues the authorize attribute should return authorize if no roles are given.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task TrueAuthorizeAttribute_ShouldReturnAuthorize_IfNoRolesAreGivenAsync()
        {
            // Arrange
            var roles = new List<Role>();
            this.trueAuthorizeAttribute = new TrueAuthorizeAttribute(roles.ToArray());

            var identity = new GenericIdentity("groups");
            identity.AddClaim(new Claim("groups", "e2577c59-a827-4a84-8e65-1a79ec752c61"));
            var claimsPrincipal = new GenericPrincipal(identity, null);
            this.httpContext.User = claimsPrincipal;
            this.SetupHttpContext();
            var actionContext = new ActionContext();
            actionContext.HttpContext = this.httpContext;
            actionContext.RouteData = new RouteData();
            actionContext.ActionDescriptor = new ControllerActionDescriptor();

            var filter = new Mock<IList<IFilterMetadata>>();

            AuthorizationFilterContext authorizationFilterContext = new AuthorizationFilterContext(actionContext, filter.Object);

            authorizationFilterContext.HttpContext = this.httpContext;

            await this.trueAuthorizeAttribute.OnAuthorizationAsync(authorizationFilterContext).ConfigureAwait(false);

            Assert.IsNull(authorizationFilterContext.Result);
        }

        /// <summary>
        /// Setups the HTTP context.
        /// </summary>
        protected void SetupHttpContext()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();

            var serviceScope = new Mock<IServiceScope>();
            serviceScope.Setup(x => x.ServiceProvider).Returns(mockServiceProvider.Object);

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory.Setup(x => x.CreateScope()).Returns(serviceScope.Object);

            var roleConfig = new UserRoleSettings();
            roleConfig.Mapping.Add(Role.Administrator, "a7479414-8c25-4afc-a7b4-b7652032649d");
            roleConfig.Mapping.Add(Role.Approver, "0e8952c4-c7cc-44dd-81e5-1b08fbc8caf6");
            roleConfig.Mapping.Add(Role.ProfessionalSegmentBalances, "1cade21c-daea-46e4-b634-e3aa652c8e1e");
            roleConfig.Mapping.Add(Role.Programmer, "bc28029f-4156-44ef-ac68-2d83eb5102da");
            roleConfig.Mapping.Add(Role.Query, "f27b4d64-a4ba-4322-8d01-1555847e29d8");

            mockServiceProvider
                .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(serviceScopeFactory.Object);

            this.mockConfigurationHandler.Setup(m => m.GetConfigurationAsync<UserRoleSettings>(ConfigurationConstants.UserRoleSettings)).ReturnsAsync(roleConfig);

            mockServiceProvider
                .Setup(x => x.GetService(typeof(IConfigurationHandler)))
                .Returns(this.mockConfigurationHandler.Object);

            this.mockBusinessContext.Setup(m => m.Populate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()));

            mockServiceProvider
                .Setup(x => x.GetService(typeof(IBusinessContext)))
                .Returns(this.mockBusinessContext.Object);

            this.httpContext.RequestServices = mockServiceProvider.Object;
        }
    }
}
