// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateBlockRequestAttributeTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Host.Admin.Api.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Api.Filter;
    using Ecp.True.Host.Api.Tests;
    using Ecp.True.Processors.Blockchain.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Validate File Registration Transaction Filter Attribute Tests.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Sap.Api.Tests.ControllerTestBase" />
    [TestClass]
    public sealed class ValidateBlockRequestAttributeTests : ControllerTestBase
    {
        /// <summary>
        /// The service provider mock.
        /// </summary>
        private Mock<IServiceProvider> serviceProviderMock;

        /// <summary>
        /// The configuration handler mock.
        /// </summary>
        private Mock<IBlockchainProcessor> processorMock;

        /// <summary>
        /// The resource provider mock.
        /// </summary>
        private Mock<IResourceProvider> resourceProviderMock;

        /// <summary>
        /// The attribute.
        /// </summary>
        private ValidateBlockRequestAttribute attribute;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.SetupHttpContext();
            this.serviceProviderMock = new Mock<IServiceProvider>();

            this.processorMock = new Mock<IBlockchainProcessor>();
            this.resourceProviderMock = new Mock<IResourceProvider>();

            this.processorMock.Setup(m => m.HasBlockAsync(It.IsAny<ulong>()));
            this.resourceProviderMock.Setup(r => r.GetResource(Entities.Constants.InvalidBlockchainPageSize)).Returns(Entities.Constants.InvalidBlockchainPageSize);
            this.resourceProviderMock.Setup(r => r.GetResource(Entities.Constants.InvalidBlockNumberSupplied)).Returns(Entities.Constants.InvalidBlockNumberSupplied);

            this.serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IBlockchainProcessor)))).Returns(this.processorMock.Object);
            this.serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.attribute = new ValidateBlockRequestAttribute();
        }

        /// <summary>
        /// Validates the sap request if movements count is zero when invoked asynchronous.
        /// </summary>
        /// <returns>Returns a task.</returns>
        [TestMethod]
        public async Task Validate_ShouldReturnError_WhenBlockNumberInvalidAsync()
        {
            var actionArguments = new Dictionary<string, object> { { "request", new BlockEventRequest { PageSize = 100, LastHead = 10000 } } };

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = this.serviceProviderMock.Object;

            this.processorMock.Setup(m => m.HasBlockAsync(10000)).ReturnsAsync(false);

            await this.attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(CreateActionExecutedContext(actionExecutingContext))).ConfigureAwait(false);

            var result = actionExecutingContext.Result;
            var badRequestObjectResult = result as BadRequestObjectResult;
            var badRequestObject = badRequestObjectResult.Value;
            var errorResponse = badRequestObject as ErrorResponse;
            var errorCodes = errorResponse.ErrorCodes;

            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequestObjectResult);
            Assert.IsNotNull(errorResponse);
            Assert.IsNotNull(errorCodes);
            Assert.AreEqual(1, errorCodes.Count());
            Assert.AreEqual(Entities.Constants.InvalidBlockNumberSupplied, errorCodes.ElementAt(0).Message);
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
