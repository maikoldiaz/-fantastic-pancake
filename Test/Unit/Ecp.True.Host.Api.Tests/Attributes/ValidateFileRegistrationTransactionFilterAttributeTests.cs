// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateFileRegistrationTransactionFilterAttributeTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Host.Api.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Host.Api.Filter;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Validate File Registration Transaction Filter Attribute Tests.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Api.Tests.ControllerTestBase" />
    [TestClass]
    public sealed class ValidateFileRegistrationTransactionFilterAttributeTests : ControllerTestBase
    {
        /// <summary>
        /// The service provider mock.
        /// </summary>
        private Mock<IServiceProvider> serviceProviderMock;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The resource provider mock.
        /// </summary>
        private Mock<IResourceProvider> resourceProviderMock;

        /// <summary>
        /// The file registration transaction repository mock.
        /// </summary>
        private Mock<IRepository<FileRegistrationTransaction>> fileRegistrationTransactionRepository;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.SetupHttpContext();
            this.serviceProviderMock = new Mock<IServiceProvider>();

            this.mockFactory = new Mock<IRepositoryFactory>();
            this.resourceProviderMock = new Mock<IResourceProvider>();

            this.fileRegistrationTransactionRepository = new Mock<IRepository<FileRegistrationTransaction>>();
            this.resourceProviderMock.Setup(r => r.GetResource(It.IsAny<string>())).Returns<string>(s => s);

            this.mockFactory.Setup(x => x.CreateRepository<FileRegistrationTransaction>()).Returns(this.fileRegistrationTransactionRepository.Object);

            this.serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            this.serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);
        }

        /// <summary>
        /// Validates the file registration transaction filter attribute if homologation identifier is not zero when invoked asynchronous.
        /// </summary>
        /// <returns>Returns a task.</returns>
        [TestMethod]
        public async Task ValidateFileRegistrationTransactionFilterAttribute_IfBlobPath_Is_Empty_WhenInvokedAsync()
        {
            var attribute = new ValidateFileRegistrationTransactionFilterAttribute();
            var fileRegistrationTransaction = new FileRegistrationTransaction { BlobPath = string.Empty, StatusTypeId = StatusType.PROCESSING, FileRegistrationTransactionId = 1 };
            var actionArguments = new Dictionary<string, object> { { "retryIds", new int[] { 1 } } };
            this.fileRegistrationTransactionRepository.Setup(s => s.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(fileRegistrationTransaction);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);
            actionExecutingContext.HttpContext.RequestServices = this.serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(actionExecutedContext)).ConfigureAwait(false);

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
        /// Validates the file registration transaction filter attribute if status type is not failed when invoked asynchronous.
        /// </summary>
        /// <returns>Returns a task.</returns>
        [TestMethod]
        public async Task ValidateFileRegistrationTransactionFilterAttribute_IfStatusType_Is_NotFailed_WhenInvokedAsync()
        {
            var attribute = new ValidateFileRegistrationTransactionFilterAttribute();
            var fileRegistrationTransaction = new FileRegistrationTransaction { BlobPath = "true", StatusTypeId = StatusType.PROCESSING, FileRegistrationTransactionId = 1 };
            var actionArguments = new Dictionary<string, object> { { "retryIds", new int[] { 1 } } };
            this.fileRegistrationTransactionRepository.Setup(s => s.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(fileRegistrationTransaction);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);
            actionExecutingContext.HttpContext.RequestServices = this.serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(actionExecutedContext)).ConfigureAwait(false);

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
        /// Validates the file registration transaction filter attribute should pass when invoked asynchronous.
        /// </summary>
        /// <returns>Returns a task.</returns>
        [TestMethod]
        public async Task ValidateFileRegistrationTransactionFilterAttribute_ShouldPass_WhenInvokedAsync()
        {
            var attribute = new ValidateFileRegistrationTransactionFilterAttribute();
            var fileRegistrationTransaction = new FileRegistrationTransaction { BlobPath = "true", StatusTypeId = StatusType.FAILED, FileRegistrationTransactionId = 1 };
            var actionArguments = new Dictionary<string, object> { { "retryIds", new int[] { 1 } } };
            this.fileRegistrationTransactionRepository.Setup(s => s.GetByIdAsync(It.IsAny<object>())).ReturnsAsync(fileRegistrationTransaction);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = this.serviceProviderMock.Object;
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
