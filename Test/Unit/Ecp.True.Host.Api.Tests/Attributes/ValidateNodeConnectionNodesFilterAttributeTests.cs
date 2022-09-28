// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateNodeConnectionNodesFilterAttributeTests.cs" company="Microsoft">
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
    using EntityConstants = Ecp.True.Entities.Constants;

    /// <summary>
    /// The node connection nodes filter attribute test class.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Api.Tests.ControllerTestBase" />
    [TestClass]
    public class ValidateNodeConnectionNodesFilterAttributeTests : ControllerTestBase
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
        /// Nodes the validation attribute return false for duplicate node storage location name when invoked.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ValidateNodeConnectionNodesFilterAttribute_ShowSucceedIfSourceNodeAndDestinationNodesAreValidAsync()
        {
            var attribute = new ValidateNodeConnectionAttribute(true, "nodeConnection");
            var nodes = new List<Node>() { new Node { NodeId = 1 }, new Node { NodeId = 2 } };
            var nodeConnection = new NodeConnection() { SourceNodeId = 1, DestinationNodeId = 2 };

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(nodes);

            var actionArguments = new Dictionary<string, object> { { "nodeConnection", nodeConnection } };
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.ActionContext.HttpContext.RequestServices = serviceProviderMock.Object;
            this.ActionContext.HttpContext.Request.Method = "POST";
            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);

            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(actionExecutedContext)).ConfigureAwait(false);

            Assert.IsNull(actionExecutingContext.Result);

            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Once);
            nodeRepoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>()), Times.Once);
            this.resourceProviderMock.Verify(m => m.GetResource(It.IsAny<string>()), Times.Never);
        }

        /// <summary>
        /// Nodes the validation attribute return false for duplicate node storage location name when invoked.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ValidateNodeConnectionNodesFilterAttribute_ShowReturnBadRequestfSourceNodeIsNotValidAsync()
        {
            var attribute = new ValidateNodeConnectionAttribute(true, "nodeConnection");
            var nodes = new List<Node>() { new Node { NodeId = 1 }, new Node { NodeId = 2 } };
            var nodeConnection = new NodeConnection() { SourceNodeId = 11, DestinationNodeId = 2 };

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(nodes);

            var actionArguments = new Dictionary<string, object> { { "nodeConnection", nodeConnection } };
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.ActionContext.HttpContext.RequestServices = serviceProviderMock.Object;
            this.ActionContext.HttpContext.Request.Method = "POST";

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);

            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(actionExecutedContext)).ConfigureAwait(false);
            var result = actionExecutingContext.Result as BadRequestObjectResult;
            var error = result.Value as ErrorResponse;

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.IsNotNull(error);
            Assert.AreEqual(EntityConstants.SourceNodeIdentifierNotFound, error.ErrorCodes.First().Message);

            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Once);
            nodeRepoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>()), Times.Once);
            this.resourceProviderMock.Verify(m => m.GetResource(It.Is<string>(s => s == EntityConstants.SourceNodeIdentifierNotFound)), Times.Once);
        }

        /// <summary>
        /// Nodes the validation attribute return false for duplicate node storage location name when invoked.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ValidateNodeConnectionNodesFilterAttribute_ShowReturnInvalidResponseIfDestinationNodeIsNotValidAsync()
        {
            var attribute = new ValidateNodeConnectionAttribute(true, "nodeConnection");
            var nodes = new List<Node>() { new Node { NodeId = 1 }, new Node { NodeId = 2 } };
            var nodeConnection = new NodeConnection() { SourceNodeId = 1, DestinationNodeId = 22 };

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(nodes);

            var actionArguments = new Dictionary<string, object> { { "nodeConnection", nodeConnection } };
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.ActionContext.HttpContext.RequestServices = serviceProviderMock.Object;
            this.ActionContext.HttpContext.Request.Method = "POST";

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);

            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(actionExecutedContext)).ConfigureAwait(false);

            var result = actionExecutingContext.Result as BadRequestObjectResult;
            var error = result.Value as ErrorResponse;

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.IsNotNull(error);
            Assert.AreEqual(EntityConstants.DestinationNodeIdentifierNotFound, error.ErrorCodes.First().Message);

            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Once);
            this.resourceProviderMock.Verify(m => m.GetResource(It.Is<string>(s => s == EntityConstants.DestinationNodeIdentifierNotFound)), Times.Once);
            nodeRepoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Validates the node connection nodes filter attribute show not throw exeption if source node and destination node is n valid.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ValidateNodeConnectionNodesFilterAttribute_ShowReturnValidResultIfSourceNodeAndDestinationNodeIsValidAsync()
        {
            var attribute = new ValidateNodeConnectionAttribute("sourceNodeId", "destinationId");
            var nodes = new List<Node>() { new Node { NodeId = 1 }, new Node { NodeId = 2 } };

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(nodes);

            var actionArguments = new Dictionary<string, object> { { "sourceNodeId", 1 }, { "destinationId", 2 } };
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.ActionContext.HttpContext.RequestServices = serviceProviderMock.Object;
            this.ActionContext.HttpContext.Request.Method = "GET";

            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);

            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(actionExecutedContext)).ConfigureAwait(false);

            Assert.IsNull(actionExecutingContext.Result);

            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Once);
            this.resourceProviderMock.Verify(m => m.GetResource(It.IsAny<string>()), Times.Never);
            nodeRepoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Validates the node connection nodes filter attribute show not throw exeption if source node and destination node is n valid.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ValidateNodeConnectionNodesFilterAttribute_ShowReturnErrors_WhenOwnershipValueIsInValidAsync()
        {
            var attribute = new ValidateNodeConnectionAttribute(true, "nodeConnection");
            var nodes = new List<Node>() { new Node { NodeId = 1 }, new Node { NodeId = 2 } };
            var nodeConnection = new NodeConnection()
            {
                SourceNodeId = 1,
                DestinationNodeId = 22,
            };

            nodeConnection.Initialize();

            var product = new NodeConnectionProduct();
            product.Initialize();

            product.Owners.Add(new NodeConnectionProductOwner { OwnershipPercentage = 10 });
            nodeConnection.Products.Add(product);

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(nodes);

            var actionArguments = new Dictionary<string, object> { { "nodeConnection", nodeConnection } };
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.ActionContext.HttpContext.RequestServices = serviceProviderMock.Object;
            this.ActionContext.HttpContext.Request.Method = "POST";
            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);

            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(actionExecutedContext)).ConfigureAwait(false);
            Assert.IsNotNull(actionExecutingContext.Result);
            Assert.IsInstanceOfType(actionExecutingContext.Result, typeof(BadRequestObjectResult));

            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Never);
            this.resourceProviderMock.Verify(m => m.GetResource(EntityConstants.ProductOwnerShipTotalValueValidation), Times.Once);
            nodeRepoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>()), Times.Never);
        }

        /// <summary>
        /// Validates the node connection nodes filter attribute show not throw exeption if source node and destination node is n valid.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ValidateNodeConnectionNodesFilterAttribute_ShowReturnValidResponse_WhenOwnershipValueIsValidAsync()
        {
            var attribute = new ValidateNodeConnectionAttribute(true, "nodeConnection");
            var nodes = new List<Node>() { new Node { NodeId = 1 }, new Node { NodeId = 2 } };
            var nodeConnection = new NodeConnection()
            {
                SourceNodeId = 1,
                DestinationNodeId = 2,
            };

            nodeConnection.Initialize();

            var product = new NodeConnectionProduct();
            product.Initialize();

            product.Owners.Add(new NodeConnectionProductOwner { OwnershipPercentage = 10 });
            product.Owners.Add(new NodeConnectionProductOwner { OwnershipPercentage = 90 });
            nodeConnection.Products.Add(product);

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(nodes);

            var actionArguments = new Dictionary<string, object> { { "nodeConnection", nodeConnection } };
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.ActionContext.HttpContext.RequestServices = serviceProviderMock.Object;
            this.ActionContext.HttpContext.Request.Method = "POST";
            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);

            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(actionExecutedContext)).ConfigureAwait(false);

            Assert.IsNull(actionExecutingContext.Result);

            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Once);
            this.resourceProviderMock.Verify(m => m.GetResource(EntityConstants.ProductOwnerShipTotalValueValidation), Times.Never);
            nodeRepoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Validates the node connection nodes filter attribute show not throw exeption if source node and destination node is n valid.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ValidateNodeConnectionNodesFilterAttribute_ShowReturnValidResponse_WhenNoProductsArePassedAsync()
        {
            var attribute = new ValidateNodeConnectionAttribute(true, "nodeConnection");
            var nodes = new List<Node>() { new Node { NodeId = 1 }, new Node { NodeId = 2 } };
            var nodeConnection = new NodeConnection()
            {
                SourceNodeId = 1,
                DestinationNodeId = 2,
            };

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(nodes);

            var actionArguments = new Dictionary<string, object> { { "nodeConnection", nodeConnection } };
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.ActionContext.HttpContext.RequestServices = serviceProviderMock.Object;
            this.ActionContext.HttpContext.Request.Method = "POST";
            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);

            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(actionExecutedContext)).ConfigureAwait(false);

            Assert.IsNull(actionExecutingContext.Result);

            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Once);
            this.resourceProviderMock.Verify(m => m.GetResource(EntityConstants.ProductOwnerShipTotalValueValidation), Times.Never);
            nodeRepoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Validates the node connection nodes filter attribute show not throw exeption if source node and destination node is n valid.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ValidateNodeConnectionNodesFilterAttribute_ShowReturnValidResponse_WhenEmptyProductsArePassedAsync()
        {
            var attribute = new ValidateNodeConnectionAttribute(true, "nodeConnection");
            var nodes = new List<Node>() { new Node { NodeId = 1 }, new Node { NodeId = 2 } };
            var nodeConnection = new NodeConnection()
            {
                SourceNodeId = 1,
                DestinationNodeId = 2,
            };
            nodeConnection.Initialize();

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(nodes);

            var actionArguments = new Dictionary<string, object> { { "nodeConnection", nodeConnection } };
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.ActionContext.HttpContext.RequestServices = serviceProviderMock.Object;
            this.ActionContext.HttpContext.Request.Method = "POST";
            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);

            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(actionExecutedContext)).ConfigureAwait(false);
            Assert.IsNull(actionExecutingContext.Result);

            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Once);
            this.resourceProviderMock.Verify(m => m.GetResource(EntityConstants.ProductOwnerShipTotalValueValidation), Times.Never);
            nodeRepoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Validates the node connection nodes filter attribute show not throw exeption if source node and destination node is n valid.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ValidateNodeConnectionNodesFilterAttribute_ShowReturnValidResponse_WhenNoOwnershipAsync()
        {
            var attribute = new ValidateNodeConnectionAttribute(true, "nodeConnection");
            var nodes = new List<Node>() { new Node { NodeId = 1 }, new Node { NodeId = 2 } };
            var nodeConnection = new NodeConnection()
            {
                SourceNodeId = 1,
                DestinationNodeId = 2,
            };

            nodeConnection.Initialize();

            var product = new NodeConnectionProduct();
            nodeConnection.Products.Add(product);

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(nodes);

            var actionArguments = new Dictionary<string, object> { { "nodeConnection", nodeConnection } };
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.ActionContext.HttpContext.RequestServices = serviceProviderMock.Object;
            this.ActionContext.HttpContext.Request.Method = "POST";
            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);

            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(actionExecutedContext)).ConfigureAwait(false);
            Assert.IsNull(actionExecutingContext.Result);

            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Once);
            this.resourceProviderMock.Verify(m => m.GetResource(EntityConstants.ProductOwnerShipTotalValueValidation), Times.Never);
            nodeRepoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Validates the node connection nodes filter attribute show not throw exeption if source node and destination node is n valid.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ValidateNodeConnectionNodesFilterAttribute_ShowReturnValidResponse_WhenEmptyOwnershipAsync()
        {
            var attribute = new ValidateNodeConnectionAttribute(true, "nodeConnection");
            var nodes = new List<Node>() { new Node { NodeId = 1 }, new Node { NodeId = 2 } };
            var nodeConnection = new NodeConnection()
            {
                SourceNodeId = 1,
                DestinationNodeId = 2,
            };

            nodeConnection.Initialize();

            var product = new NodeConnectionProduct();
            product.Initialize();

            nodeConnection.Products.Add(product);

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(nodes);

            var actionArguments = new Dictionary<string, object> { { "nodeConnection", nodeConnection } };
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.ActionContext.HttpContext.RequestServices = serviceProviderMock.Object;
            this.ActionContext.HttpContext.Request.Method = "POST";
            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);

            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(actionExecutedContext)).ConfigureAwait(false);
            Assert.IsNull(actionExecutingContext.Result);

            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Once);
            this.resourceProviderMock.Verify(m => m.GetResource(EntityConstants.ProductOwnerShipTotalValueValidation), Times.Never);
            nodeRepoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Validates the node connection nodes filter attribute show return valid response when is transfer false algorithm null.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ValidateNodeConnectionNodesFilterAttribute_ShowReturnValidResponse_WhenIsTransferFalseAlgorithmNullAsync()
        {
            var attribute = new ValidateNodeConnectionAttribute(true, "nodeConnection");
            var nodes = new List<Node>() { new Node { NodeId = 1 }, new Node { NodeId = 2 } };
            var nodeConnection = new NodeConnection()
            {
                SourceNodeId = 1,
                DestinationNodeId = 2,
                IsTransfer = false,
                AlgorithmId = null,
            };

            nodeConnection.Initialize();

            var product = new NodeConnectionProduct();
            product.Initialize();

            nodeConnection.Products.Add(product);

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(nodes);

            var actionArguments = new Dictionary<string, object> { { "nodeConnection", nodeConnection } };
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.ActionContext.HttpContext.RequestServices = serviceProviderMock.Object;
            this.ActionContext.HttpContext.Request.Method = "POST";
            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);

            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(actionExecutedContext)).ConfigureAwait(false);
            Assert.IsNull(actionExecutingContext.Result);

            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Once);
            nodeRepoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Validates the node connection nodes filter attribute show return valid response when is transfer true algorithm not null.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ValidateNodeConnectionNodesFilterAttribute_ShowReturnValidResponse_WhenIsTransferTrueAlgorithmNotNullAsync()
        {
            var attribute = new ValidateNodeConnectionAttribute(true, "nodeConnection");
            var nodes = new List<Node>() { new Node { NodeId = 1 }, new Node { NodeId = 2 } };
            var nodeConnection = new NodeConnection()
            {
                SourceNodeId = 1,
                DestinationNodeId = 2,
                IsTransfer = true,
                AlgorithmId = 2,
            };

            nodeConnection.Initialize();

            var product = new NodeConnectionProduct();
            product.Initialize();

            nodeConnection.Products.Add(product);

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(nodes);

            var actionArguments = new Dictionary<string, object> { { "nodeConnection", nodeConnection } };
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.ActionContext.HttpContext.RequestServices = serviceProviderMock.Object;
            this.ActionContext.HttpContext.Request.Method = "POST";
            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);

            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(actionExecutedContext)).ConfigureAwait(false);
            Assert.IsNull(actionExecutingContext.Result);

            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Once);
            nodeRepoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>()), Times.Once);
        }

        /// <summary>
        /// Validates the node connection nodes filter attribute show return errors when is transfer false algorithm not null.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ValidateNodeConnectionNodesFilterAttribute_ShowReturnErrors_WhenIsTransferFalseAlgorithmNotNullAsync()
        {
            var attribute = new ValidateNodeConnectionAttribute(true, "nodeConnection");
            var nodes = new List<Node>() { new Node { NodeId = 1 }, new Node { NodeId = 2 } };
            var nodeConnection = new NodeConnection()
            {
                SourceNodeId = 1,
                DestinationNodeId = 2,
                IsTransfer = false,
                AlgorithmId = 2,
            };

            nodeConnection.Initialize();

            var product = new NodeConnectionProduct();
            product.Initialize();

            nodeConnection.Products.Add(product);

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(nodes);

            var actionArguments = new Dictionary<string, object> { { "nodeConnection", nodeConnection } };
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.ActionContext.HttpContext.RequestServices = serviceProviderMock.Object;
            this.ActionContext.HttpContext.Request.Method = "POST";
            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);

            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(actionExecutedContext)).ConfigureAwait(false);
            Assert.IsNotNull(actionExecutingContext.Result);

            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Never);
            this.resourceProviderMock.Verify(m => m.GetResource(EntityConstants.NoTransferWithAlgorithmIdMessage), Times.Once);
            nodeRepoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>()), Times.Never);
        }

        /// <summary>
        /// Validates the node connection nodes filter attribute show return errors when is transfer true algorithm null.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ValidateNodeConnectionNodesFilterAttribute_ShowReturnErrors_WhenIsTransferTrueAlgorithmNullAsync()
        {
            var attribute = new ValidateNodeConnectionAttribute(true, "nodeConnection");
            var nodes = new List<Node>() { new Node { NodeId = 1 }, new Node { NodeId = 2 } };
            var nodeConnection = new NodeConnection()
            {
                SourceNodeId = 1,
                DestinationNodeId = 2,
                IsTransfer = true,
                AlgorithmId = null,
            };

            nodeConnection.Initialize();

            var product = new NodeConnectionProduct();
            product.Initialize();

            nodeConnection.Products.Add(product);

            var nodeRepoMock = new Mock<IRepository<Node>>();
            nodeRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>())).ReturnsAsync(nodes);

            var actionArguments = new Dictionary<string, object> { { "nodeConnection", nodeConnection } };
            this.mockFactory.Setup(x => x.CreateRepository<Node>()).Returns(nodeRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IRepositoryFactory)))).Returns(this.mockFactory.Object);
            serviceProviderMock.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IResourceProvider)))).Returns(this.resourceProviderMock.Object);

            this.ActionContext.HttpContext.RequestServices = serviceProviderMock.Object;
            this.ActionContext.HttpContext.Request.Method = "POST";
            var actionExecutingContext = new ActionExecutingContext(this.ActionContext, new List<IFilterMetadata>(), actionArguments, null);
            var actionExecutedContext = new ActionExecutedContext(this.ActionContext, new List<IFilterMetadata>(), null);

            await attribute.OnActionExecutionAsync(actionExecutingContext, () => Task.FromResult(actionExecutedContext)).ConfigureAwait(false);
            Assert.IsNotNull(actionExecutingContext.Result);

            this.mockFactory.Verify(m => m.CreateRepository<Node>(), Times.Never);
            this.resourceProviderMock.Verify(m => m.GetResource(EntityConstants.TransferWithNoAlgorithmIdMessage), Times.Once);
            nodeRepoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Node, bool>>>()), Times.Never);
        }
    }
}
