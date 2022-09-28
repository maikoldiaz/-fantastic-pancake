// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateReportFilterAttributeTest.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Host.Api.Filter;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ValidateReportFilterAttributeTest : ControllerTestBase
    {
        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The resource provider mock.
        /// </summary>
        private Mock<IResourceProvider> resourceProviderMock;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockFactory = new Mock<IRepositoryFactory>();

            this.resourceProviderMock = new Mock<IResourceProvider>();
            this.resourceProviderMock.Setup(r => r.GetResource(It.IsAny<string>())).Returns<string>(s => s);

            this.SetupHttpContext();
        }

        /// <summary>
        /// Validates the report filter attribute show succeed if report execution does not exists asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task ValidateReportFilterAttribute_ShowSucceedIfReportExecutionDoesNotExistsAsync()
        {
            var attribute = new ValidateReportFilterAttribute();
            var reportExecution = new ReportExecution
            {
                CategoryId = 12,
                ElementId = 23,
                NodeId = 23,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
                ReportTypeId = Entities.Enumeration.ReportType.BeforeCutOff,
            };

            var mockReportExecutionRepository = new Mock<IRepository<ReportExecution>>();
            mockReportExecutionRepository.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<ReportExecution, bool>>>())).ReturnsAsync(0);

            this.mockFactory.Setup(x => x.CreateRepository<ReportExecution>()).Returns(mockReportExecutionRepository.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);
            var actionArguments = new Dictionary<string, object> { { "execution", reportExecution } };
            this.ActionContext.HttpContext.RequestServices = serviceProviderMock.Object;
            this.ActionContext.HttpContext.Request.Method = "POST";
            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);

            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(actionExecutedContext)).ConfigureAwait(false);

            Assert.IsNull(actionExecutingContext.Result);

            this.mockFactory.Verify(m => m.CreateRepository<ReportExecution>(), Times.Once);
            mockReportExecutionRepository.Verify(r => r.GetCountAsync(It.IsAny<Expression<Func<ReportExecution, bool>>>()), Times.Once);
            this.resourceProviderMock.Verify(m => m.GetResource(It.IsAny<string>()), Times.Never);
        }

        /// <summary>
        /// Validates the report filter attribute show fail if report execution exists asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateReportFilterAttribute_ShowFailIfReportExecutionExistsAsync()
        {
            var attribute = new ValidateReportFilterAttribute();
            var reportExecution = new ReportExecution
            {
                CategoryId = 12,
                ElementId = 23,
                NodeId = 23,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
                ReportTypeId = Entities.Enumeration.ReportType.BeforeCutOff,
            };

            var mockReportExecutionRepository = new Mock<IRepository<ReportExecution>>();
            mockReportExecutionRepository.Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<ReportExecution, bool>>>())).ReturnsAsync(1);

            this.mockFactory.Setup(x => x.CreateRepository<ReportExecution>()).Returns(mockReportExecutionRepository.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);
            var actionArguments = new Dictionary<string, object> { { "execution", reportExecution } };
            this.ActionContext.HttpContext.RequestServices = serviceProviderMock.Object;
            this.ActionContext.HttpContext.Request.Method = "POST";
            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);

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

            this.mockFactory.Verify(m => m.CreateRepository<ReportExecution>(), Times.Once);
            mockReportExecutionRepository.Verify(r => r.GetCountAsync(It.IsAny<Expression<Func<ReportExecution, bool>>>()), Times.Once);
            this.resourceProviderMock.Verify(m => m.GetResource(It.IsAny<string>()), Times.Once);
        }
    }
}
