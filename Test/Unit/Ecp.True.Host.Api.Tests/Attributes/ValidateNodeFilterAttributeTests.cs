// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateNodeFilterAttributeTests.cs" company="Microsoft">
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
    using System.Linq;
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

    /// <summary>
    /// The Validate Node Filter Attribute Tests.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Api.Tests.ControllerTestBase" />
    [TestClass]
    public sealed class ValidateNodeFilterAttributeTests : ControllerTestBase
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
        private Mock<IRepository<CategoryElement>> catElementRepoMock;

        /// <summary>
        /// The node repo mock.
        /// </summary>
        private Mock<IRepository<Node>> nodeRepoMock;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.SetupHttpContext();
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.resourceProviderMock = new Mock<IResourceProvider>();
            this.catElementRepoMock = new Mock<IRepository<CategoryElement>>();
            this.nodeRepoMock = new Mock<IRepository<Node>>();
            this.resourceProviderMock.Setup(r => r.GetResource(It.IsAny<string>())).Returns<string>(s => s);
        }

        /// <summary>
        /// Nodes the validation attribute return false for duplicate node storage location name when invoked.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task NodeValidationAttribute_ReturnFalse_For_DuplicateNodeStorageLocationName_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(false);
            var product = new StorageLocationProduct { StorageLocationProductId = 0, NodeStorageLocationId = 0, IsActive = false };

            var nodeStorageLocation1 = new NodeStorageLocation { NodeStorageLocationId = 0, IsActive = true, Name = "SL" };
            nodeStorageLocation1.Products.Add(product);

            var nodeStorageLocation2 = new NodeStorageLocation { NodeStorageLocationId = 0, IsActive = true, Name = "SL" };
            nodeStorageLocation2.Products.Add(product);

            var node = new Node { IsActive = true, NodeTypeId = 1, OperatorId = 10, SegmentId = 4 };
            node.NodeStorageLocations.Add(nodeStorageLocation1);
            node.NodeStorageLocations.Add(nodeStorageLocation2);
            var actionArguments = new Dictionary<string, object> { { "node", node } };

            var catElement = new CategoryElement { ElementId = 1, CategoryId = 1 };
            this.catElementRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(catElement);
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.catElementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

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
        /// Nodes the create validation attribute if node is active is false when invoked.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task NodeCreateValidationAttribute_IfNodeIsActive_Is_False_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(false);
            var node = new Node { IsActive = false, NodeTypeId = 1, OperatorId = 10, SegmentId = 4 };
            var actionArguments = new Dictionary<string, object> { { "node", node } };

            var catElement = new CategoryElement { ElementId = 1, CategoryId = 1 };
            this.catElementRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(catElement);
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.catElementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

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
        /// Nodes the create validation attribute if node identifier is not zero when invoked.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task NodeCreateValidationAttribute_IfNodeId_Is_Not_Zero_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(false);

            var node = new Node { IsActive = true, NodeId = 1, NodeTypeId = 1, OperatorId = 10, SegmentId = 4 };
            var actionArguments = new Dictionary<string, object> { { "node", node } };

            var catElement = new CategoryElement { ElementId = 1, CategoryId = 1 };
            this.catElementRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(catElement);
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.catElementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

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
        /// Nodes the create validation attribute if invalid node storage location verify node storage location identifier when invoked.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task NodeCreateValidationAttribute_IfInvalidNodeStorageLocation_Verify_NodeStorageLocationId_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(false);

            var nodeStorageLocation = new NodeStorageLocation { NodeStorageLocationId = 1 };
            var node = new Node { IsActive = true, NodeId = 0, NodeTypeId = 1, OperatorId = 10, SegmentId = 4 };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var actionArguments = new Dictionary<string, object> { { "node", node } };

            var catElement = new CategoryElement { ElementId = 1, CategoryId = 1 };
            this.catElementRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(catElement);
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.catElementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

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
        /// Nodes the create validation attribute if invalid node storage location verify node storage location is active when invoked.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task NodeCreateValidationAttribute_IfInvalidNodeStorageLocation_Verify_NodeStorageLocation_IsActive_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(false);
            var nodeStorageLocation = new NodeStorageLocation { NodeStorageLocationId = 0, IsActive = false };
            var node = new Node { IsActive = true, NodeId = 0, NodeTypeId = 1, OperatorId = 10, SegmentId = 4 };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var actionArguments = new Dictionary<string, object> { { "node", node } };

            var catElement = new CategoryElement { ElementId = 1, CategoryId = 1 };
            this.catElementRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(catElement);
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.catElementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

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
        /// Nodes the create validation attribute if invalid product locations verify product location identifier when invoked.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task NodeCreateValidationAttribute_IfInvalidProductLocations_Verify_ProductLocationId_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(false);

            var product = new StorageLocationProduct { StorageLocationProductId = 1 };

            var nodeStorageLocation = new NodeStorageLocation { NodeStorageLocationId = 0, IsActive = true };
            nodeStorageLocation.Products.Add(product);

            var node = new Node { IsActive = true, NodeId = 0, NodeTypeId = 1, OperatorId = 10, SegmentId = 4 };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var actionArguments = new Dictionary<string, object> { { "node", node } };

            var catElement = new CategoryElement { ElementId = 1, CategoryId = 1 };
            this.catElementRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(catElement);
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.catElementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

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
        /// Nodes the create validation attribute if invalid product locations verify node storage location identifier when invoked.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task NodeCreateValidationAttribute_IfInvalidProductLocations_Verify_NodeStorageLocationId_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(false);
            var product = new StorageLocationProduct { StorageLocationProductId = 0, NodeStorageLocationId = 1 };

            var nodeStorageLocation = new NodeStorageLocation { NodeStorageLocationId = 0, IsActive = true };
            nodeStorageLocation.Products.Add(product);

            var node = new Node { IsActive = true, NodeId = 0, NodeTypeId = 1, OperatorId = 10, SegmentId = 4 };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var actionArguments = new Dictionary<string, object> { { "node", node } };

            var catElement = new CategoryElement { ElementId = 1, CategoryId = 1 };
            this.catElementRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(catElement);
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.catElementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

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
        /// Nodes the create validation attribute if invalid product locations verify is active when invoked.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task NodeCreateValidationAttribute_IfInvalidProductLocations_Verify_IsActive_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(false);
            var product = new StorageLocationProduct { StorageLocationProductId = 0, NodeStorageLocationId = 0, IsActive = false };

            var nodeStorageLocation = new NodeStorageLocation { NodeStorageLocationId = 0, IsActive = true };
            nodeStorageLocation.Products.Add(product);

            var node = new Node { IsActive = true, NodeId = 0, NodeTypeId = 1, OperatorId = 10, SegmentId = 4 };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var actionArguments = new Dictionary<string, object> { { "node", node } };

            var catElement = new CategoryElement { ElementId = 1, CategoryId = 1 };
            this.catElementRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(catElement);
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.catElementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

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
        /// Nodes the create validation attribute validation succeeds when invoked.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task NodeCreateValidationAttribute_ValidationSucceeds_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(false);
            var product = new StorageLocationProduct { StorageLocationProductId = 0, NodeStorageLocationId = 0, IsActive = true };

            var nodeStorageLocation = new NodeStorageLocation { NodeStorageLocationId = 0, IsActive = true };
            nodeStorageLocation.Products.Add(product);

            var node = new Node { IsActive = true, NodeId = 0, NodeTypeId = 1, OperatorId = 10, SegmentId = 4 };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var actionArguments = new Dictionary<string, object> { { "node", node } };

            var catElement = new CategoryElement { ElementId = 1, CategoryId = 1 };
            this.catElementRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(catElement);
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.catElementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(() => null);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            var next = new ActionExecutionDelegate(() => Task.FromResult(CreateActionExecutedContext(actionExecutingContext)));

            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);
            var result = actionExecutingContext.Result;

            Assert.IsNull(result, "Result should be null");
        }

        /// <summary>
        /// Nodes create if capacity and unitId has value.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task NodeCreateValidationAttribute_IfUnitIdAndCapacityHasValue_ValidationSucceeds_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(false);
            var product = new StorageLocationProduct { StorageLocationProductId = 0, NodeStorageLocationId = 0, IsActive = true };

            var nodeStorageLocation = new NodeStorageLocation { NodeStorageLocationId = 0, IsActive = true };
            nodeStorageLocation.Products.Add(product);

            var node = new Node { IsActive = true, NodeId = 0, NodeTypeId = 1, OperatorId = 10, SegmentId = 4, Capacity = 11, UnitId = 31 };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var actionArguments = new Dictionary<string, object> { { "node", node } };

            var catElement = new CategoryElement { ElementId = 1, CategoryId = 1 };
            this.catElementRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(catElement);
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.catElementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(() => null);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            var next = new ActionExecutionDelegate(() => Task.FromResult(CreateActionExecutedContext(actionExecutingContext)));

            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);
            var result = actionExecutingContext.Result;

            Assert.IsNull(result, "Result should be null");
        }

        /// <summary>
        /// Nodes failed to create if capacity has value and unitId does not.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task NodeCreateValidationAttribute_OnlyCapacityHasValue_ValidationFails_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(false);
            var product = new StorageLocationProduct { StorageLocationProductId = 0, NodeStorageLocationId = 0, IsActive = true };

            var nodeStorageLocation = new NodeStorageLocation { NodeStorageLocationId = 0, IsActive = true };
            nodeStorageLocation.Products.Add(product);

            var node = new Node { IsActive = true, NodeId = 0, NodeTypeId = 1, OperatorId = 10, SegmentId = 4, Capacity = 11 };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var actionArguments = new Dictionary<string, object> { { "node", node } };

            var catElement = new CategoryElement { ElementId = 1, CategoryId = 1 };
            this.catElementRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(catElement);
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.catElementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            var next = new ActionExecutionDelegate(() => Task.FromResult(CreateActionExecutedContext(actionExecutingContext)));

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
        /// Nodes failed to create if unitId has value and capacity does not.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task NodeCreateValidationAttribute_OnlyUnitIdHasValue_ValidationFails_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(false);
            var product = new StorageLocationProduct { StorageLocationProductId = 0, NodeStorageLocationId = 0, IsActive = true };

            var nodeStorageLocation = new NodeStorageLocation { NodeStorageLocationId = 0, IsActive = true };
            nodeStorageLocation.Products.Add(product);

            var node = new Node { IsActive = true, NodeId = 0, NodeTypeId = 1, OperatorId = 10, SegmentId = 4, UnitId = 31 };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var actionArguments = new Dictionary<string, object> { { "node", node } };

            var catElement = new CategoryElement { ElementId = 1, CategoryId = 1 };
            this.catElementRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(catElement);
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.catElementRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            var next = new ActionExecutionDelegate(() => Task.FromResult(CreateActionExecutedContext(actionExecutingContext)));

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
        /// Nodes the update validation attribute if node identifier is zero when invoked.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task NodeUpdateValidationAttribute_IfNodeId_Is_Zero_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(true);
            var node = new Node { NodeId = 0 };
            var actionArguments = new Dictionary<string, object> { { "node", node } };

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";
            attribute.OnActionExecuting(actionExecutingContext);

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
        /// Nodes the update validation attribute if node identifier is not zero when invoked.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task NodeUpdateValidationAttribute_IfNodeId_Is_Not_Zero_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(true);
            var nodeStorageLocation = new NodeStorageLocation { NodeId = 0 };
            var node = new Node { NodeId = 1 };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var actionArguments = new Dictionary<string, object> { { "node", node } };
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

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
        /// Nodes the update validation attribute validation succeeds when invoked.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task NodeUpdateValidationAttribute_ValidationSucceeds_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(true);
            var nodeStorageLocation = new NodeStorageLocation { NodeId = 1 };
            var node = new Node { NodeId = 1 };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var actionArguments = new Dictionary<string, object> { { "node", node } };

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";

            var next = new ActionExecutionDelegate(() => Task.FromResult(CreateActionExecutedContext(actionExecutingContext)));
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);
            var result = actionExecutingContext.Result;

            Assert.IsNull(result, "Result should be null");
        }

        /// <summary>
        /// Nodes Validation succeeds if both Capacity and UnitId are given.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task NodeUpdateValidationAttribute_IfUnitIdAndCapacityHasValue_ValidationSucceeds_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(true);
            var nodeStorageLocation = new NodeStorageLocation { NodeId = 1 };
            var node = new Node { NodeId = 1, Capacity = 11, UnitId = 31 };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var actionArguments = new Dictionary<string, object> { { "node", node } };

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";

            var next = new ActionExecutionDelegate(() => Task.FromResult(CreateActionExecutedContext(actionExecutingContext)));
            await attribute.OnActionExecutionAsync(actionExecutingContext, next).ConfigureAwait(false);
            var result = actionExecutingContext.Result;

            Assert.IsNull(result, "Result should be null");
        }

        /// <summary>
        /// Nodes Validation fails if only Capacity and UnitId is not.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task NodeUpdateValidationAttribute_IOnlyCapacityHasValue_ValidationSucceeds_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(true);
            var nodeStorageLocation = new NodeStorageLocation { NodeId = 1 };
            var node = new Node { NodeId = 1, Capacity = 11 };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var actionArguments = new Dictionary<string, object> { { "node", node } };

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";

            var next = new ActionExecutionDelegate(() => Task.FromResult(CreateActionExecutedContext(actionExecutingContext)));
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
        /// Nodes Validation succeeds if UnitId is given and Capacity.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task NodeUpdateValidationAttribute_IfOnlyUnitHasValue_ValidationSucceeds_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(true);
            var nodeStorageLocation = new NodeStorageLocation { NodeId = 1 };
            var node = new Node { NodeId = 1, UnitId = 31 };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            var actionArguments = new Dictionary<string, object> { { "node", node } };

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            actionExecutingContext.HttpContext.RequestServices = serviceProviderMock.Object;
            actionExecutingContext.HttpContext.Request.Method = "POST";

            var next = new ActionExecutionDelegate(() => Task.FromResult(CreateActionExecutedContext(actionExecutingContext)));
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
        public async Task NodeCreateValidationAttribute_ValidationFailsWhenNodeTypeIdDoesNotExist_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(false);
            var product = new StorageLocationProduct { StorageLocationProductId = 0, NodeStorageLocationId = 0, IsActive = true };

            var nodeStorageLocation = new NodeStorageLocation { NodeStorageLocationId = 0, IsActive = true };
            nodeStorageLocation.Products.Add(product);

            var node = new Node { IsActive = true, NodeId = 0, NodeTypeId = 1, OperatorId = 10, SegmentId = 4 };
            node.NodeStorageLocations.Add(nodeStorageLocation);

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

            var actionArguments = new Dictionary<string, object> { { "node", node } };

            CategoryElement catElement = null;

            this.catElementRepoMock = new Mock<IRepository<CategoryElement>>();
            this.catElementRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(catElement);
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.catElementRepoMock.Object);

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
        /// Nodes the validation attribute return false for duplicate node name when invoked asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task NodeValidationAttribute_ReturnFalse_For_CreateDuplicateNodeName_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(false);
            var product = new StorageLocationProduct { StorageLocationProductId = 1, NodeStorageLocationId = 0, IsActive = false };

            var nodeStorageLocation1 = new NodeStorageLocation { NodeStorageLocationId = 0, IsActive = true, Name = "SL" };
            nodeStorageLocation1.Products.Add(product);

            var node = new Node { IsActive = true, NodeTypeId = 1, OperatorId = 10, SegmentId = 4, Name = "Node1" };
            node.NodeStorageLocations.Add(nodeStorageLocation1);
            var actionArguments = new Dictionary<string, object> { { "node", node } };

            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(node);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

            var catElement = new CategoryElement { ElementId = 1, CategoryId = 1 };
            this.catElementRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(catElement);
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.catElementRepoMock.Object);

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
            Assert.AreEqual(errorCodes.First().Message, Entities.Constants.NodeNameMustBeUnique);
        }

        /// <summary>
        /// Nodes the validation attribute return false for update duplicate node name when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task NodeValidationAttribute_ReturnFalse_For_UpdateDuplicateNodeName_WhenInvokedAsync()
        {
            var attribute = new ValidateNodeFilterAttribute(true);
            var product = new StorageLocationProduct { StorageLocationProductId = 1, NodeStorageLocationId = 0, IsActive = false };

            var nodeStorageLocation1 = new NodeStorageLocation { NodeStorageLocationId = 0, IsActive = true, Name = "SL" };
            nodeStorageLocation1.Products.Add(product);

            var node = new Node { NodeId = 1, IsActive = true, NodeTypeId = 1, OperatorId = 10, SegmentId = 4, Name = "Node1" };
            node.NodeStorageLocations.Add(nodeStorageLocation1);
            var actionArguments = new Dictionary<string, object> { { "node", node } };

            var dbNode = new Node { NodeId = 2, IsActive = true, NodeTypeId = 1, OperatorId = 10, SegmentId = 4, Name = "Node1" };
            this.nodeRepoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(dbNode);
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(this.nodeRepoMock.Object);

            var catElement = new CategoryElement { ElementId = 1, CategoryId = 1 };
            this.catElementRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CategoryElement, bool>>>())).ReturnsAsync(catElement);
            this.mockFactory.Setup(x => x.CreateRepository<CategoryElement>()).Returns(this.catElementRepoMock.Object);

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
            Assert.AreEqual(errorCodes.First().Message, Entities.Constants.NodeNameMustBeUnique);
        }

        /// <summary>
        /// Creates the action executed context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The ActionExecutedContext.</returns>
        private static ActionExecutedContext CreateActionExecutedContext(ActionExecutingContext context)
        {
            return new ActionExecutedContext(context, context.Filters, context.Controller)
            {
                Result = context.Result,
            };
        }
    }
}