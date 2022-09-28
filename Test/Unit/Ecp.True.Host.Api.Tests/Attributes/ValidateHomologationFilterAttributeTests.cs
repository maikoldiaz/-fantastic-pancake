// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateHomologationFilterAttributeTests.cs" company="Microsoft">
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
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Api.Filter;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Validate Homologation Filter Attribute Tests.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Api.Tests.ControllerTestBase" />
    [TestClass]
    public sealed class ValidateHomologationFilterAttributeTests : ControllerTestBase
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
        /// The resource provider mock.
        /// </summary>
        private Mock<IRepository<Homologation>> homologationRepoMock;

        /// <summary>
        /// The resource provider mock.
        /// </summary>
        private Mock<IRepository<Node>> nodeRepoMock;

        /// <summary>
        /// The resource provider mock.
        /// </summary>
        private Mock<IRepository<Product>> productRepoMock;

        /// <summary>
        /// The resource provider mock.
        /// </summary>
        private Mock<IRepository<NodeStorageLocation>> nodeStorageLocationRepoMock;

        /// <summary>
        /// The resource provider mock.
        /// </summary>
        private Mock<IRepository<CategoryElement>> categoryElementRepoMock;

        /// <summary>
        /// The resource provider mock.
        /// </summary>
        private Mock<IRepository<HomologationGroup>> homologationGroupRepoMock;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.SetupHttpContext();
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.resourceProviderMock = new Mock<IResourceProvider>();
            this.homologationRepoMock = new Mock<IRepository<Homologation>>();
            this.nodeRepoMock = new Mock<IRepository<Node>>();
            this.productRepoMock = new Mock<IRepository<Product>>();
            this.nodeStorageLocationRepoMock = new Mock<IRepository<NodeStorageLocation>>();
            this.categoryElementRepoMock = new Mock<IRepository<CategoryElement>>();
            this.homologationGroupRepoMock = new Mock<IRepository<HomologationGroup>>();
            this.resourceProviderMock.Setup(r => r.GetResource(It.IsAny<string>())).Returns<string>(s => s);
        }

        /// <summary>
        /// Homologation validation attribute return false when homologation Id is not null when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task HomologationCreateValidationAttribute_IfHomologationId_Is_Not_Zero_WhenInvokedAsync()
        {
            var attribute = new ValidateHomologationFilterAttribute();
            var homologation = new Homologation { HomologationId = 1, SourceSystemId = (int)SystemType.TRUE, DestinationSystemId = (int)SystemType.SINOPER };
            var actionArguments = new Dictionary<string, object> { { "homologation", homologation } };

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
        }

        /// <summary>
        /// Homologation validation attribute return false for same source and destination system Id when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task HomologationCreateValidationAttribute_IfHomologationSourceAndDestinationSystemsAreSame_WhenInvokedAsync()
        {
            var attribute = new ValidateHomologationFilterAttribute();
            var homologation = new Homologation { SourceSystemId = (int)SystemType.TRUE, DestinationSystemId = (int)SystemType.TRUE };
            var actionArguments = new Dictionary<string, object> { { "homologation", homologation } };

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
        }

        /// <summary>
        /// Homologation validation attribute return false if source or destination system Id is null when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task HomologationCreateValidationAttribute_IfHomologationSourceOrDestinationSystemsIsNull_WhenInvokedAsync()
        {
            var attribute = new ValidateHomologationFilterAttribute();
            var homologation = new Homologation { SourceSystemId = (int)SystemType.TRUE, DestinationSystemId = null };
            var actionArguments = new Dictionary<string, object> { { "homologation", homologation } };

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
        }

        /// <summary>
        /// Homologation validation attribute return false for source or destination system not True when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task HomologationCreateValidationAttribute_IfHomologationSourceOrDestinationSystemsAreNotTrue_WhenInvokedAsync()
        {
            var attribute = new ValidateHomologationFilterAttribute();
            var homologation = new Homologation { SourceSystemId = (int)SystemType.EXCEL, DestinationSystemId = (int)SystemType.SINOPER };
            var actionArguments = new Dictionary<string, object> { { "homologation", homologation } };

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
        }

        /// <summary>
        /// Homologation validation attribute return false for source or destination system not True when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task HomologationCreateValidationAttribute_IfHomologationGroupTypeRepeated_WhenInvokedAsync()
        {
            var attribute = new ValidateHomologationFilterAttribute();
            var homologation = new Homologation { SourceSystemId = (int)SystemType.TRUE, DestinationSystemId = (int)SystemType.SINOPER };
            homologation.HomologationGroups.Add(new HomologationGroup() { GroupTypeId = 13 });
            homologation.HomologationGroups.Add(new HomologationGroup() { GroupTypeId = 13 });
            var actionArguments = new Dictionary<string, object> { { "homologation", homologation } };

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
        }

        /// <summary>
        /// Homologation validation attribute for foreign key reference when invoked.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task HomologationCreateValidationAttribute_CheckUniqness_ValidateFKeyDestinationTrueGroupTypeNode_WhenInvokedAsync()
        {
            var attribute = new ValidateHomologationFilterAttribute();
            var homologation = new Homologation { SourceSystemId = (int)SystemType.SINOPER, DestinationSystemId = (int)SystemType.TRUE };
            var homologationGroup = new HomologationGroup { GroupTypeId = 13 };
            homologationGroup.HomologationDataMapping.Add(new HomologationDataMapping { SourceValue = "93", DestinationValue = "81" });
            homologationGroup.HomologationDataMapping.Add(new HomologationDataMapping { SourceValue = "93", DestinationValue = "81" });
            homologation.HomologationGroups.Add(homologationGroup);
            var node = new Node();
            HomologationGroup group = null;

            var actionArguments = new Dictionary<string, object> { { "homologation", homologation } };
            this.homologationRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Homologation, bool>>>())).ReturnsAsync(homologation);
            this.homologationGroupRepoMock.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<HomologationGroup, bool>>>())).ReturnsAsync(group);
            this.nodeRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Expression<Func<Homologation, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.HomologationRepository.SingleOrDefaultAsync(It.IsAny<Expression<Func<Homologation, bool>>>())).ReturnsAsync(homologation);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);
            this.mockFactory.Setup(x => x.CreateRepository<HomologationGroup>()).Returns(this.homologationGroupRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

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
        /// Homologation validation attribute for foreign key reference when invoked.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task HomologationCreateValidationAttribute_CheckUniqness_ValidateFKeyDestinationTrueGroupTypeProducts_WhenInvokedAsync()
        {
            var attribute = new ValidateHomologationFilterAttribute();
            var homologation = new Homologation { SourceSystemId = (int)SystemType.SINOPER, DestinationSystemId = (int)SystemType.TRUE };
            var homologationGroup = new HomologationGroup { GroupTypeId = 14 };
            homologationGroup.HomologationDataMapping.Add(new HomologationDataMapping { SourceValue = "93", DestinationValue = "81" });
            homologationGroup.HomologationDataMapping.Add(new HomologationDataMapping { SourceValue = "93", DestinationValue = "81" });
            homologation.HomologationGroups.Add(homologationGroup);
            var product = new Product();
            HomologationGroup group = null;

            var actionArguments = new Dictionary<string, object> { { "homologation", homologation } };
            this.homologationRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Homologation, bool>>>())).ReturnsAsync(homologation);
            this.homologationGroupRepoMock.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<HomologationGroup, bool>>>())).ReturnsAsync(group);
            this.productRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Expression<Func<Homologation, bool>>>())).ReturnsAsync(product);
            this.mockFactory.Setup(x => x.HomologationRepository.SingleOrDefaultAsync(It.IsAny<Expression<Func<Homologation, bool>>>())).ReturnsAsync(homologation);
            this.mockFactory.Setup(x => x.CreateRepository<Product>()).Returns(this.productRepoMock.Object);
            this.mockFactory.Setup(x => x.CreateRepository<HomologationGroup>()).Returns(this.homologationGroupRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

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
        /// Homologation validation attribute for foreign key reference when invoked.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task HomologationCreateValidationAttribute_CheckUniqness_ValidateFKeyDestinationTrueGroupTypeNodeStorageLocation_WhenInvokedAsync()
        {
            var attribute = new ValidateHomologationFilterAttribute();
            var homologation = new Homologation { SourceSystemId = (int)SystemType.SINOPER, DestinationSystemId = (int)SystemType.TRUE };
            var homologationGroup = new HomologationGroup { GroupTypeId = 15 };
            homologationGroup.HomologationDataMapping.Add(new HomologationDataMapping { SourceValue = "93", DestinationValue = "81" });
            homologationGroup.HomologationDataMapping.Add(new HomologationDataMapping { SourceValue = "93", DestinationValue = "81" });
            homologation.HomologationGroups.Add(homologationGroup);
            var nodeStorageLocation = new NodeStorageLocation();
            HomologationGroup group = null;

            var actionArguments = new Dictionary<string, object> { { "homologation", homologation } };
            this.homologationRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Homologation, bool>>>())).ReturnsAsync(homologation);
            this.nodeStorageLocationRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Expression<Func<Homologation, bool>>>())).ReturnsAsync(nodeStorageLocation);
            this.homologationGroupRepoMock.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<HomologationGroup, bool>>>())).ReturnsAsync(group);
            this.mockFactory.Setup(x => x.HomologationRepository.SingleOrDefaultAsync(It.IsAny<Expression<Func<Homologation, bool>>>())).ReturnsAsync(homologation);
            this.mockFactory.Setup(x => x.CreateRepository<NodeStorageLocation>()).Returns(this.nodeStorageLocationRepoMock.Object);
            this.mockFactory.Setup(x => x.CreateRepository<HomologationGroup>()).Returns(this.homologationGroupRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

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
        /// Homologation validation attribute for foreign key reference when invoked.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task HomologationCreateValidationAttribute_CheckUniqness_ValidateFKeySourceTrueGroupTypeNode_WhenInvokedAsync()
        {
            var attribute = new ValidateHomologationFilterAttribute();
            var homologation = new Homologation { SourceSystemId = (int)SystemType.TRUE, DestinationSystemId = (int)SystemType.SINOPER };
            var homologationGroup = new HomologationGroup { GroupTypeId = 13 };
            homologationGroup.HomologationDataMapping.Add(new HomologationDataMapping { SourceValue = "93", DestinationValue = "81" });
            homologationGroup.HomologationDataMapping.Add(new HomologationDataMapping { SourceValue = "93", DestinationValue = "81" });
            homologation.HomologationGroups.Add(homologationGroup);
            var node = new Node();
            HomologationGroup group = null;

            var actionArguments = new Dictionary<string, object> { { "homologation", homologation } };
            this.homologationRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Homologation, bool>>>())).ReturnsAsync(homologation);
            this.nodeRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Expression<Func<Homologation, bool>>>())).ReturnsAsync(node);
            this.homologationGroupRepoMock.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<HomologationGroup, bool>>>())).ReturnsAsync(group);
            this.mockFactory.Setup(x => x.HomologationRepository.SingleOrDefaultAsync(It.IsAny<Expression<Func<Homologation, bool>>>())).ReturnsAsync(homologation);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);
            this.mockFactory.Setup(x => x.CreateRepository<HomologationGroup>()).Returns(this.homologationGroupRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

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
        /// Homologation validation attribute for foreign key reference when invoked.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task HomologationCreateValidationAttribute_CheckUniqness_ValidateFKeySourceTrueGroupTypeProducts_WhenInvokedAsync()
        {
            var attribute = new ValidateHomologationFilterAttribute();
            var homologation = new Homologation { SourceSystemId = (int)SystemType.TRUE, DestinationSystemId = (int)SystemType.SINOPER };
            var homologationGroup = new HomologationGroup { GroupTypeId = 14 };
            homologationGroup.HomologationDataMapping.Add(new HomologationDataMapping { SourceValue = "93", DestinationValue = "81" });
            homologationGroup.HomologationDataMapping.Add(new HomologationDataMapping { SourceValue = "93", DestinationValue = "81" });
            homologation.HomologationGroups.Add(homologationGroup);
            var product = new Product();
            HomologationGroup group = null;

            var actionArguments = new Dictionary<string, object> { { "homologation", homologation } };
            this.homologationRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Homologation, bool>>>())).ReturnsAsync(homologation);
            this.productRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Expression<Func<Homologation, bool>>>())).ReturnsAsync(product);
            this.homologationGroupRepoMock.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<HomologationGroup, bool>>>())).ReturnsAsync(group);
            this.mockFactory.Setup(x => x.HomologationRepository.SingleOrDefaultAsync(It.IsAny<Expression<Func<Homologation, bool>>>())).ReturnsAsync(homologation);
            this.mockFactory.Setup(x => x.CreateRepository<Product>()).Returns(this.productRepoMock.Object);
            this.mockFactory.Setup(x => x.CreateRepository<HomologationGroup>()).Returns(this.homologationGroupRepoMock.Object);
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

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
        /// Homologation validation attribute for foreign key reference when invoked.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task HomologationCreateValidationAttribute_CheckUniqness_ValidateFKeySourceTrueGroupTypeNodeStorageLocation_WhenInvokedAsync()
        {
            var attribute = new ValidateHomologationFilterAttribute();
            var homologation = new Homologation { SourceSystemId = (int)SystemType.TRUE, DestinationSystemId = (int)SystemType.SINOPER };
            var homologationGroup = new HomologationGroup { GroupTypeId = 15 };
            homologationGroup.HomologationDataMapping.Add(new HomologationDataMapping { SourceValue = "93", DestinationValue = "81" });
            homologationGroup.HomologationDataMapping.Add(new HomologationDataMapping { SourceValue = "93", DestinationValue = "81" });
            homologation.HomologationGroups.Add(homologationGroup);
            var nodeStorageLocation = new NodeStorageLocation();
            HomologationGroup group = null;
            var actionArguments = new Dictionary<string, object> { { "homologation", homologation } };
            this.homologationRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Homologation, bool>>>())).ReturnsAsync(homologation);
            this.nodeStorageLocationRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Expression<Func<Homologation, bool>>>())).ReturnsAsync(nodeStorageLocation);
            this.homologationGroupRepoMock.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<HomologationGroup, bool>>>())).ReturnsAsync(group);
            this.mockFactory.Setup(x => x.HomologationRepository.SingleOrDefaultAsync(It.IsAny<Expression<Func<Homologation, bool>>>())).ReturnsAsync(homologation);
            this.mockFactory.Setup(x => x.CreateRepository<NodeStorageLocation>()).Returns(this.nodeStorageLocationRepoMock.Object);
            this.mockFactory.Setup(x => x.CreateRepository<HomologationGroup>()).Returns(this.homologationGroupRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

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
    }
}
