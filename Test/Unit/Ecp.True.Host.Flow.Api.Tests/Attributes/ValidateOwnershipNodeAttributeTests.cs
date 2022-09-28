// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateOwnershipNodeAttributeTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Flow.Api.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Host.Flow.Api.Filter;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Validate Node Filter Attribute Tests.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Api.Tests.ControllerTestBase" />
    [TestClass]
    public class ValidateOwnershipNodeAttributeTests : ControllerTestBase
    {
        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The resource provider mock.
        /// </summary>
        private Mock<IRepository<OwnershipNode>> ownershipNodeRepoMock;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.SetupHttpContext();
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.ownershipNodeRepoMock = new Mock<IRepository<OwnershipNode>>();
        }

        /// <summary>
        /// Validates the ownership node attribute if ownership node is null when invoked asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task ValidateOwnershipNodeAttribute_IfOwnershipNode_Is_Null_WhenInvokedAsync()
        {
            var attribute = new ValidateOwnershipNodeAttribute();
            var nodeOwnershipApprovalRequest = new NodeOwnershipApprovalRequest();
            var actionArguments = new Dictionary<string, object> { { "approvalRequest", nodeOwnershipApprovalRequest } };
            OwnershipNode ownershipNode = null;
            this.ownershipNodeRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ownershipNode);
            this.mockFactory.Setup(x => x.CreateRepository<OwnershipNode>()).Returns(this.ownershipNodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);
            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
        }

        /// <summary>
        /// Validates the ownership node attribute if ownership node staus is submitfor approval when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ValidateOwnershipNodeAttribute_IfOwnershipNodeStaus_IsNot_SubmitforApproval_WhenInvokedAsync()
        {
            var attribute = new ValidateOwnershipNodeAttribute();
            var nodeOwnershipApprovalRequest = new NodeOwnershipApprovalRequest();
            var actionArguments = new Dictionary<string, object> { { "approvalRequest", nodeOwnershipApprovalRequest } };
            var ownershipNode = new OwnershipNode { OwnershipStatus = Entities.Enumeration.OwnershipNodeStatusType.SENT };
            this.ownershipNodeRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ownershipNode);
            this.mockFactory.Setup(x => x.CreateRepository<OwnershipNode>()).Returns(this.ownershipNodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);
            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
        }

        [TestMethod]
        public async Task ValidateOwnershipNodeAttribute_IfapprovalRequestStaus_IsNot_Approved_WhenInvokedAsync()
        {
            var attribute = new ValidateOwnershipNodeAttribute();
            var nodeOwnershipApprovalRequest = new NodeOwnershipApprovalRequest { Status = OwnershipNodeStatusType.FAILED.ToString() };
            var actionArguments = new Dictionary<string, object> { { "approvalRequest", nodeOwnershipApprovalRequest } };
            var ownershipNode = new OwnershipNode { OwnershipStatus = Entities.Enumeration.OwnershipNodeStatusType.SUBMITFORAPPROVAL };
            this.ownershipNodeRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ownershipNode);
            this.mockFactory.Setup(x => x.CreateRepository<OwnershipNode>()).Returns(this.ownershipNodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            var next = new ActionExecutionDelegate(() => { throw null; });
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);
            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(badRequestObjectResult, "Bad request result should not be null");
            Assert.IsNotNull(errorResponse, "Error response should not be null");
            Assert.IsNotNull(errorCodes, "Error Code should not be null");
        }

        /// <summary>
        /// Validates the ownership node attribute success when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ValidateOwnershipNodeAttribute_Success_WhenInvokedAsync()
        {
            var attribute = new ValidateOwnershipNodeAttribute();
            var nodeOwnershipApprovalRequest = new NodeOwnershipApprovalRequest { Status = OwnershipNodeStatusType.APPROVED.ToString() };
            var actionArguments = new Dictionary<string, object> { { "approvalRequest", nodeOwnershipApprovalRequest } };
            var ownershipNode = new OwnershipNode { OwnershipStatus = Entities.Enumeration.OwnershipNodeStatusType.SUBMITFORAPPROVAL };
            this.ownershipNodeRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ownershipNode);
            this.mockFactory.Setup(x => x.CreateRepository<OwnershipNode>()).Returns(this.ownershipNodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            var next = new ActionExecutionDelegate(() => Task.FromResult(CreateActionExecutedContext(actionExecutingContext)));

            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);
            var result = actionExecutingContext.Result;

            Assert.IsNull(result, "Result should be null");
        }

        private static ActionExecutedContext CreateActionExecutedContext(ActionExecutingContext context)
        {
            return new ActionExecutedContext(context, context.Filters, context.Controller)
            {
                Result = context.Result,
            };
        }
    }
}
